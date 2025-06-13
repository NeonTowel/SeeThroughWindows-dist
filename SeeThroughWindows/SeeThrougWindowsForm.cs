using System.ComponentModel;
using System.Diagnostics;
using SeeThroughWindows.Core;
using SeeThroughWindows.Themes;
using SeeThroughWindows.Services;
using SeeThroughWindows.Models;
using SeeThroughWindows.Infrastructure;

namespace SeeThroughWindows
{
  public partial class SeeThrougWindowsForm : Form
  {
    #region Dependencies
    private readonly IApplicationService _applicationService;
    private readonly IHotkeyManager _hotkeyManager;
    private readonly ISettingsManager _settingsManager;
    private readonly IUpdateChecker _updateChecker;
    #endregion

    #region Constants
    private const string ToolName = "SeeThroughWindows";
    private const short DEFAULT_SEMITRANSPARENT = 64;
    #endregion

    #region Data members
    // Flag indicating we're really closing the application
    private bool exitingApplication = false;
    // Flag indicating we're still loading
    private bool loading = false;
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    public SeeThrougWindowsForm(
        IApplicationService applicationService,
        IHotkeyManager hotkeyManager,
        ISettingsManager settingsManager,
        IUpdateChecker updateChecker)
    {
      // Store dependencies
      _applicationService = applicationService;
      _hotkeyManager = hotkeyManager;
      _settingsManager = settingsManager;
      _updateChecker = updateChecker;

      // Set loading flag to prevent event handlers from firing during initialization
      this.loading = true;

      InitializeComponent();

      // Set the version text dynamically
      var version = Application.ProductVersion;
      var bitness = Environment.Is64BitProcess ? "64-bit" : "32-bit";
      helpLink.Text = $"See Through Windows ({bitness}) v{version} by MOBZystems";

      // Initialize theme manager and load saved theme settings
      ThemeManager.Initialize();

      // Apply theme
      ThemeManager.ApplyTheme(this);
      ThemeManager.ThemeChanged += OnThemeChanged;

      // Set up custom status bar layout
      SetupStatusBarLayout();

      // Populate combo boxes
      PopulateThemeComboBox();
      PopulateAccentColorComboBox();

      // Set up the icon
      notifyIcon.Icon = this.Icon;

      // Initialize services
      InitializeServices();

      // Populate the hotkey combo box
      PopulateHotkeyComboBox();

      // Set the default transparency value
      transparencyTrackBar.Value = DEFAULT_SEMITRANSPARENT;

      // Load settings and initialize UI
      LoadAndApplySettings();

      // Clear loading flag
      this.loading = false;

      // Update the UI
      UpdateUI();
    }
    #endregion

    #region Service Integration
    /// <summary>
    /// Initialize all services and wire up event handlers
    /// </summary>
    private void InitializeServices()
    {
      // Initialize the application service
      _applicationService.Initialize();

      // Wire up application service events
      _applicationService.NotificationRequested += OnNotificationRequested;      // Wire up hotkey manager events
      _hotkeyManager.UserHotkeyPressed += (s, e) => _applicationService.HandleUserHotkeyPress();
      _hotkeyManager.MaximizeHotkeyPressed += (s, e) => _applicationService.HandleMaximizeHotkeyPress();
      _hotkeyManager.MinimizeHotkeyPressed += (s, e) => _applicationService.HandleMinimizeHotkeyPress();
      _hotkeyManager.PreviousScreenHotkeyPressed += (s, e) => _applicationService.HandlePreviousScreenHotkeyPress();
      _hotkeyManager.NextScreenHotkeyPressed += (s, e) => _applicationService.HandleNextScreenHotkeyPress();
      _hotkeyManager.MoreTransparentHotkeyPressed += (s, e) => _applicationService.HandleMoreTransparentHotkeyPress();
      _hotkeyManager.LessTransparentHotkeyPressed += (s, e) => _applicationService.HandleLessTransparentHotkeyPress();

      // Wire up update checker events
      _updateChecker.UpdateAvailable += OnUpdateAvailable;
    }

    /// <summary>
    /// Handle notification requests from the application service
    /// </summary>
    private void OnNotificationRequested(object? sender, NotificationEventArgs e)
    {
      notifyIcon.ShowBalloonTip(3000, "See Through Windows", e.Message, e.Icon);
    }    /// <summary>
    /// Handle update available events from the update checker
    /// </summary>
    private void OnUpdateAvailable(object? sender, UpdateAvailableEventArgs e)
    {
      updateAvailableLink.Text = $"Update Available: {e.Version}";
      updateAvailableLink.Visible = true;
      CenterStatusBarContent();
    }

    /// <summary>
    /// Load settings and apply them to the UI
    /// </summary>
    private void LoadAndApplySettings()
    {
      var settings = _applicationService.GetCurrentSettings();

      // Apply transparency setting
      transparencyTrackBar.Value = settings.SemiTransparentValue;

      // Apply checkbox settings
      clickThroughCheckBox.Checked = settings.ClickThrough;
      topMostCheckBox.Checked = settings.TopMost;

      // Apply hotkey settings
      shiftCheckBox.Checked = settings.HotkeySettings.Shift;
      controlCheckBox.Checked = settings.HotkeySettings.Control;
      altCheckBox.Checked = settings.HotkeySettings.Alt;
      windowsCheckBox.Checked = settings.HotkeySettings.Windows;
      hotKeyComboBox.SelectedItem = settings.HotkeySettings.KeyCode;

      // Apply feature enable settings
      sendToMonitorEnabledCheckBox.Checked = settings.EnableLeftRight;
      minMaxEnabledCheckBox.Checked = settings.EnableUpDown;
      enableChangeTransparencyCheckbox.Checked = settings.EnablePageUpDown;

      // Initialize hotkey manager with current settings
      _hotkeyManager.Initialize(this, settings.HotkeySettings);

      // Set hotkey feature states
      _hotkeyManager.SetMonitorHotkeysEnabled(settings.EnableLeftRight);
      _hotkeyManager.SetMinMaxHotkeysEnabled(settings.EnableUpDown);
      _hotkeyManager.SetTransparencyHotkeysEnabled(settings.EnablePageUpDown);

      // Apply theme settings
      ThemeManager.SetTheme(settings.ThemeFlavor, settings.AccentColor);

      // Update transparency label
      UpdateTransparency();
    }

    /// <summary>
    /// Save current settings using the application service
    /// </summary>
    private void SaveCurrentSettings()
    {
      if (loading) return;

      var settings = new AppSettings
      {
        SemiTransparentValue = (short)transparencyTrackBar.Value,
        ClickThrough = clickThroughCheckBox.Checked,
        TopMost = topMostCheckBox.Checked,
        HotkeySettings = new HotkeySettings
        {
          KeyCode = (Keys)(hotKeyComboBox.SelectedItem ?? Keys.Z),
          Shift = shiftCheckBox.Checked,
          Control = controlCheckBox.Checked,
          Alt = altCheckBox.Checked,
          Windows = windowsCheckBox.Checked
        },
        EnableLeftRight = sendToMonitorEnabledCheckBox.Checked,
        EnableUpDown = minMaxEnabledCheckBox.Checked,
        EnablePageUpDown = enableChangeTransparencyCheckbox.Checked,
        ThemeFlavor = ThemeManager.CurrentFlavor,
        AccentColor = ThemeManager.CurrentAccentColor
      };

      _applicationService.UpdateSettings(settings);
    }
    #endregion

    #region UI Helper Methods
    /// <summary>
    /// Populate the hotkey combo box with available keys
    /// </summary>
    private void PopulateHotkeyComboBox()
    {
      // Add letter keys
      for (Keys k = Keys.A; k <= Keys.Z; k++)
      {
        hotKeyComboBox.Items.Add(k);
      }

      // Add function keys
      for (Keys k = Keys.F1; k <= Keys.F12; k++)
      {
        hotKeyComboBox.Items.Add(k);
      }

      // Add number keys
      for (Keys k = Keys.D0; k <= Keys.D9; k++)
      {
        hotKeyComboBox.Items.Add(k);
      }

      // Add numpad keys
      for (Keys k = Keys.NumPad0; k <= Keys.NumPad9; k++)
      {
        hotKeyComboBox.Items.Add(k);
      }

      // Set the default value
      hotKeyComboBox.SelectedItem = Keys.Z;
    }

    /// <summary>
    /// Update the UI based on current state
    /// </summary>
    private void UpdateUI()
    {
      var hijackedWindowCount = _applicationService.GetHijackedWindowCount();
      restoreAllButton.Enabled = hijackedWindowCount > 0;
    }

    /// <summary>
    /// Update the transparency value and label
    /// </summary>
    private void UpdateTransparency()
    {
      var transparencyValue = (short)transparencyTrackBar.Value;
      int percentage = (int)Math.Round((double)transparencyValue / 255.0 * 100.0);
      transparencyValueLabel.Text = $"{percentage}%";
    }

    /// <summary>
    /// Apply the transparency to the form itself (for preview)
    /// </summary>
    private void ApplyTransparency()
    {
      this.Opacity = ((double)transparencyTrackBar.Value) / 255;
    }
    #endregion

    #region Status Bar Layout Methods
    /// <summary>
    /// Set up custom layout for the status bar to center icon and text
    /// </summary>
    private void SetupStatusBarLayout()
    {
      // Remove docking from status bar elements to allow custom positioning
      helpLink.Dock = DockStyle.None;
      updateAvailableLink.Dock = DockStyle.None;

      // Handle layout events to maintain centering
      panel1.Layout += Panel1_Layout;
      panel1.Resize += Panel1_Resize;
    }

    /// <summary>
    /// Custom layout handler for status bar centering
    /// </summary>
    private void Panel1_Layout(object? sender, LayoutEventArgs e)
    {
      CenterStatusBarContent();
    }

    /// <summary>
    /// Handle resize events to maintain centering
    /// </summary>
    private void Panel1_Resize(object? sender, EventArgs e)
    {
      CenterStatusBarContent();
    }

    /// <summary>
    /// Center the icon and text in the status bar
    /// </summary>
    private void CenterStatusBarContent()
    {
      if (panel1 == null || pictureBox1 == null || helpLink == null) return;

      // Calculate total content width (icon + spacing + text)
      var iconWidth = pictureBox1.Width;
      var textWidth = helpLink.Width;
      var spacing = 8; // Space between icon and text
      var totalContentWidth = iconWidth + spacing + textWidth;

      // Calculate starting position to center the content
      var startX = (panel1.ClientSize.Width - totalContentWidth) / 2;

      // Position the icon
      pictureBox1.Location = new Point(
          Math.Max(panel1.Padding.Left, startX),
          (panel1.ClientSize.Height - pictureBox1.Height) / 2
      );

      // Position the text next to the icon
      helpLink.Location = new Point(
          pictureBox1.Right + spacing,
          (panel1.ClientSize.Height - helpLink.Height) / 2
      );

      // Position update link (if visible) to the right of the main text
      if (updateAvailableLink.Visible)
      {
        updateAvailableLink.Location = new Point(
            helpLink.Right + spacing,
            (panel1.ClientSize.Height - updateAvailableLink.Height) / 2
        );
      }
    }
    #endregion

    #region Overridden Methods
    /// <summary>
    /// Handle form load event
    /// </summary>
    protected override async void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      // Apply the current theme
      ThemeManager.ApplyTheme(this);

      // Check for updates using the service
      try
      {
        await _updateChecker.CheckForUpdatesAsync();
      }
      catch
      {
        // Ignore errors checking for updates
      }
    }

    /// <summary>
    /// Make sure we hide when the user wants to close,
    /// except when this.exitingApplication
    /// </summary>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      base.OnFormClosing(e);

      // When closing the form, we don't exit (but hide in the task bar)
      // EXCEPT when we're actually exiting the app OR when we're pressing Shift
      if (e.CloseReason == CloseReason.UserClosing && !this.exitingApplication && ((Control.ModifierKeys & Keys.Shift) == 0))
      {
        // No - save settings and hide ourselves
        SaveCurrentSettings();

        e.Cancel = true;
        Hide();
      }
    }

    /// <summary>
    /// Clean up after ourselves when we really do close
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);

      // Clean up through application service
      _applicationService.RestoreAllWindows();

      // Clean up hotkey manager
      _hotkeyManager?.Dispose();
    }
    #endregion

    #region UI Event Handlers
    /// <summary>
    /// Handle form shown event
    /// </summary>
    private void SeeThrougWindowsForm_Shown(object sender, EventArgs e)
    {
      // Make sure we hide ourselves when started minimized
      if (this.WindowState == FormWindowState.Minimized)
      {
        // This causes a brief flash but swapping WindowState and Hide causes the form
        // to stay minimized and to never become Normal
        this.WindowState = FormWindowState.Normal;
        this.Hide();
      }
    }

    /// <summary>
    /// Update the transparency when the track bar changes
    /// </summary>
    private void transparencyTrackBar_ValueChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      UpdateTransparency();

      if (previewCheckBox.Checked)
        ApplyTransparency();

      SaveCurrentSettings();
    }

    /// <summary>
    /// Preview the current transparency, or reset it
    /// </summary>
    private void previewCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      if (previewCheckBox.Checked)
        ApplyTransparency();
      else
        this.Opacity = 1.0;
    }

    /// <summary>
    /// Update the hotkey settings when UI controls change
    /// </summary>
    private void UpdateHotKey(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      // Create hotkey settings from UI
      var hotkeySettings = new HotkeySettings
      {
        KeyCode = (Keys)(hotKeyComboBox.SelectedItem ?? Keys.Z),
        Shift = shiftCheckBox.Checked,
        Control = controlCheckBox.Checked,
        Alt = altCheckBox.Checked,
        Windows = windowsCheckBox.Checked
      };

      // Update the hotkey manager
      _hotkeyManager.UpdateUserHotkey(hotkeySettings);

      // Save settings
      SaveCurrentSettings();
    }

    /// <summary>
    /// Enable topmost if clickthrough
    /// </summary>
    private void clickThroughCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      this.topMostCheckBox.Enabled = this.clickThroughCheckBox.Checked;
      SaveCurrentSettings();
    }

    /// <summary>
    /// Save settings when topmost checkbox changes
    /// </summary>
    private void topMostCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      SaveCurrentSettings();
    }

    /// <summary>
    /// (Un)register hotkeys for window minimize/maximize
    /// </summary>
    private void minMaxEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      _hotkeyManager.SetMinMaxHotkeysEnabled(minMaxEnabledCheckBox.Checked);
      SaveCurrentSettings();
    }

    /// <summary>
    /// (Un)register hotkeys for move window
    /// </summary>
    private void sendToMonitorEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      _hotkeyManager.SetMonitorHotkeysEnabled(sendToMonitorEnabledCheckBox.Checked);
      SaveCurrentSettings();
    }

    /// <summary>
    /// (Un)register hotkeys for window transparency change
    /// </summary>
    private void enableChangeTransparencyCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      _hotkeyManager.SetTransparencyHotkeysEnabled(enableChangeTransparencyCheckbox.Checked);
      SaveCurrentSettings();
    }

    /// <summary>
    /// Restore all transparent windows
    /// </summary>
    private void restoreAllButton_Click(object sender, EventArgs e)
    {
      _applicationService.RestoreAllWindows();
      UpdateUI();
    }

    /// <summary>
    /// Surf to the web site
    /// </summary>
    private void helpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        var psi = new ProcessStartInfo("http://www.mobzystems.com/Tools/SeeThroughWindows");
        psi.UseShellExecute = true;
        Process.Start(psi);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    /// <summary>
    /// Show ourselves when the notify icon was clicked
    /// </summary>
    private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        if (!this.Visible)
          Show();

        Activate();
      }
    }

    /// <summary>
    /// Really close the form from the exit menu
    /// </summary>
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.exitingApplication = true;
      Close();

      Application.Exit();
    }

    /// <summary>
    /// Handle update available link click
    /// </summary>
    private void updateAvailableLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        var pi = new ProcessStartInfo($"https://www.mobzystems.com/Tools/{ToolName}");
        pi.UseShellExecute = true;
        Process.Start(pi);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Could not open web page", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    #endregion

    #region Theme Methods
    private void PopulateThemeComboBox()
    {
      themeComboBox.Items.Clear();

      foreach (CatppuccinTheme.Flavor flavor in Enum.GetValues<CatppuccinTheme.Flavor>())
      {
        themeComboBox.Items.Add(new ThemeComboBoxItem(flavor));
      }

      // Set current selection
      for (int i = 0; i < themeComboBox.Items.Count; i++)
      {
        if (themeComboBox.Items[i] is ThemeComboBoxItem item && item.Flavor == ThemeManager.CurrentFlavor)
        {
          themeComboBox.SelectedIndex = i;
          break;
        }
      }
    }

    private void PopulateAccentColorComboBox()
    {
      accentColorComboBox.Items.Clear();

      foreach (CatppuccinTheme.AccentColor accentColor in Enum.GetValues<CatppuccinTheme.AccentColor>())
      {
        accentColorComboBox.Items.Add(new AccentColorComboBoxItem(accentColor));
      }

      // Set current selection
      for (int i = 0; i < accentColorComboBox.Items.Count; i++)
      {
        if (accentColorComboBox.Items[i] is AccentColorComboBoxItem item && item.AccentColor == ThemeManager.CurrentAccentColor)
        {
          accentColorComboBox.SelectedIndex = i;
          break;
        }
      }
    }

    private void themeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (loading) return;

      if (themeComboBox.SelectedItem is ThemeComboBoxItem selectedItem)
      {
        ThemeManager.SetTheme(selectedItem.Flavor, ThemeManager.CurrentAccentColor);
      }
    }

    private void accentColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (loading) return;

      if (accentColorComboBox.SelectedItem is AccentColorComboBoxItem selectedItem)
      {
        ThemeManager.SetTheme(ThemeManager.CurrentFlavor, selectedItem.AccentColor);
      }
    }

    private void OnThemeChanged(object? sender, EventArgs e)
    {
      // Apply theme to this form
      ThemeManager.ApplyTheme(this);

      // Force a complete refresh of all controls, especially the trackbar
      this.Refresh();

      // Specifically refresh the transparency trackbar to ensure accent color updates
      transparencyTrackBar.Invalidate();
      transparencyTrackBar.Update();
    }

    private class ThemeComboBoxItem
    {
      public CatppuccinTheme.Flavor Flavor { get; }

      public ThemeComboBoxItem(CatppuccinTheme.Flavor flavor)
      {
        Flavor = flavor;
      }

      public override string ToString()
      {
        return CatppuccinTheme.GetFlavorDisplayName(Flavor);
      }
    }

    private class AccentColorComboBoxItem
    {
      public CatppuccinTheme.AccentColor AccentColor { get; }

      public AccentColorComboBoxItem(CatppuccinTheme.AccentColor accentColor)
      {
        AccentColor = accentColor;
      }

      public override string ToString()
      {
        return CatppuccinTheme.GetAccentColorDisplayName(AccentColor);
      }
    }
    #endregion
  }
}
