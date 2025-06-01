using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace SeeThroughWindows.Themes
{
    /// <summary>
    /// Manages theme application and persistence for the application
    /// </summary>
    public static class ThemeManager
    {
        private const string THEME_REGISTRY_KEY = "Theme";
        private const string ACCENT_REGISTRY_KEY = "AccentColor";
        private const string REGROOT = "Software\\MOBZystems\\MOBZXRay\\";

        public static CatppuccinTheme.Flavor CurrentFlavor { get; private set; } = CatppuccinTheme.Flavor.Latte;
        public static CatppuccinTheme.AccentColor CurrentAccentColor { get; private set; } = CatppuccinTheme.AccentColor.Sky;
        public static CatppuccinTheme.ThemeColors CurrentTheme { get; private set; } = CatppuccinTheme.GetThemeColors(CatppuccinTheme.Flavor.Latte, CatppuccinTheme.AccentColor.Sky);

        public static event EventHandler? ThemeChanged;

        /// <summary>
        /// Initialize the theme manager and load saved theme
        /// </summary>
        public static void Initialize()
        {
            LoadThemeFromRegistry();
            ApplyCurrentTheme();
        }

        /// <summary>
        /// Set the current theme flavor and accent color
        /// </summary>
        public static void SetTheme(CatppuccinTheme.Flavor flavor, CatppuccinTheme.AccentColor accentColor = CatppuccinTheme.AccentColor.Lavender)
        {
            CurrentFlavor = flavor;
            CurrentAccentColor = accentColor;
            CurrentTheme = CatppuccinTheme.GetThemeColors(flavor, accentColor);
            SaveThemeToRegistry();
            ThemeChanged?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Apply the current theme to a form and all its controls
        /// </summary>
        public static void ApplyTheme(Form form)
        {
            if (form == null) return;

            // Apply theme to the form itself
            form.BackColor = CurrentTheme.Background;
            form.ForeColor = CurrentTheme.Text;

            // Improve font rendering for softer appearance
            if (form.Font.Size <= 9) // Only adjust small fonts
            {
                form.Font = new Font(form.Font.FontFamily, form.Font.Size, FontStyle.Regular, GraphicsUnit.Point);
            }

            // Apply theme to all controls recursively
            ApplyThemeToControls(form.Controls);

            // Force repaint of the entire form to ensure all custom paint handlers use new colors
            form.Invalidate(true);
        }

        /// <summary>
        /// Apply theme to a collection of controls
        /// </summary>
        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                ApplyThemeToControl(control);

                // Recursively apply to child controls
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }

        /// <summary>
        /// Apply theme to a specific control based on its type
        /// </summary>
        private static void ApplyThemeToControl(Control control)
        {
            // Set basic colors for all controls
            control.BackColor = CurrentTheme.Background;
            control.ForeColor = CurrentTheme.Text;

            // Improve font rendering for all controls
            if (control.Font.Size <= 9) // Only adjust small fonts
            {
                control.Font = new Font(control.Font.FontFamily, control.Font.Size, FontStyle.Regular, GraphicsUnit.Point);
            }

            switch (control)
            {
                case Button button:
                    ApplyButtonTheme(button);
                    break;

                case CheckBox checkBox:
                    ApplyCheckBoxTheme(checkBox);
                    break;

                case ComboBox comboBox:
                    ApplyComboBoxTheme(comboBox);
                    break;

                case TrackBar trackBar:
                    ApplyTrackBarTheme(trackBar);
                    break;

                case GroupBox groupBox:
                    ApplyGroupBoxTheme(groupBox);
                    break;

                case Panel panel:
                    ApplyPanelTheme(panel);
                    break;

                case LinkLabel linkLabel:
                    ApplyLinkLabelTheme(linkLabel);
                    break;

                case Label label:
                    ApplyLabelTheme(label);
                    break;

                case PictureBox pictureBox:
                    ApplyPictureBoxTheme(pictureBox);
                    break;

                case ContextMenuStrip contextMenu:
                    ApplyContextMenuTheme(contextMenu);
                    break;
            }

            // Force repaint for TrackBars to update accent colors
            if (control is TrackBar tb)
            {
                // Re-apply the theme to ensure paint handlers are updated
                ApplyTrackBarTheme(tb);
            }
        }

        private static void ApplyButtonTheme(Button button)
        {
            button.BackColor = CurrentTheme.ButtonBackground;
            button.ForeColor = CurrentTheme.ButtonText;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = CurrentTheme.SubtleBorder;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = CurrentTheme.ButtonHover;
            button.FlatAppearance.MouseDownBackColor = CurrentTheme.Accent;

            // Increase button padding for more generous spacing
            button.Padding = new Padding(12, 6, 12, 6); // Add internal padding
        }

        private static void ApplyCheckBoxTheme(CheckBox checkBox)
        {
            checkBox.BackColor = CurrentTheme.Background;
            checkBox.ForeColor = CurrentTheme.Text;
            checkBox.FlatStyle = FlatStyle.Flat;
            checkBox.FlatAppearance.BorderColor = CurrentTheme.CheckboxBorder;
            checkBox.FlatAppearance.BorderSize = 1;
            checkBox.FlatAppearance.CheckedBackColor = CurrentTheme.Accent;

            // Add more spacing between checkboxes
            checkBox.Margin = new Padding(4, 6, 4, 6); // Increased vertical spacing

            // Add custom paint handler for better checkbox visibility
            checkBox.Paint += (sender, e) =>
            {
                if (sender is CheckBox cb)
                {
                    // Enable text anti-aliasing for softer fonts
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                    // Clear the default checkbox rendering
                    e.Graphics.Clear(cb.BackColor);

                    // Calculate checkbox rectangle
                    var checkBoxSize = 18; // Increased from 13 to 18 (5 pixels bigger)
                    var checkBoxRect = new Rectangle(0, (cb.Height - checkBoxSize) / 2, checkBoxSize, checkBoxSize);

                    // Draw checkbox background
                    using var backgroundBrush = new SolidBrush(CurrentTheme.CheckboxBackground);
                    e.Graphics.FillRectangle(backgroundBrush, checkBoxRect);

                    // Draw checkbox border
                    using var borderPen = new Pen(CurrentTheme.CheckboxBorder, 1);
                    e.Graphics.DrawRectangle(borderPen, checkBoxRect);

                    // Draw checkmark if checked
                    if (cb.Checked)
                    {
                        using var checkBrush = new SolidBrush(CurrentTheme.Accent);
                        e.Graphics.FillRectangle(checkBrush, new Rectangle(checkBoxRect.X + 2, checkBoxRect.Y + 2, checkBoxRect.Width - 4, checkBoxRect.Height - 4));

                        // Draw checkmark symbol - adjusted for larger checkbox
                        using var checkPen = new Pen(CurrentTheme.Background, 2);
                        var points = new Point[]
                  {
              new Point(checkBoxRect.X + 4, checkBoxRect.Y + 9),  // Adjusted for larger size
              new Point(checkBoxRect.X + 7, checkBoxRect.Y + 12), // Adjusted for larger size
              new Point(checkBoxRect.X + 14, checkBoxRect.Y + 5)  // Adjusted for larger size
                  };
                        e.Graphics.DrawLines(checkPen, points);
                    }

                    // Draw text with better spacing
                    var textRect = new Rectangle(checkBoxSize + 8, 0, cb.Width - checkBoxSize - 8, cb.Height); // Spacing automatically adjusts with larger checkbox
                    TextRenderer.DrawText(e.Graphics, cb.Text, cb.Font, textRect, cb.ForeColor, TextFormatFlags.VerticalCenter);
                }
            };
        }

        private static void ApplyTrackBarTheme(TrackBar trackBar)
        {
            trackBar.BackColor = CurrentTheme.Background;

            // Remove all existing paint handlers first
            trackBar.Paint -= TrackBarPaintHandler;

            // Add our custom paint handler
            trackBar.Paint += TrackBarPaintHandler;

            // Remove existing mouse handlers to avoid duplicates
            trackBar.MouseDown -= TrackBarMouseDownHandler;
            trackBar.MouseMove -= TrackBarMouseMoveHandler;
            trackBar.MouseUp -= TrackBarMouseUpHandler;

            // Add enhanced mouse event handlers that override Windows' native hit-testing
            trackBar.MouseDown += TrackBarMouseDownHandler;
            trackBar.MouseMove += TrackBarMouseMoveHandler;
            trackBar.MouseUp += TrackBarMouseUpHandler;

            // Essential event handlers for value changes
            trackBar.ValueChanged += (sender, e) => trackBar.Invalidate();

            // Try to enable custom drawing using reflection
            try
            {
                var method = typeof(Control).GetMethod("SetStyle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(trackBar, new object[] { ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true });
                }
            }
            catch
            {
                // If reflection fails, continue with normal approach
            }

            // Force a complete repaint to ensure the new colors are applied
            trackBar.Invalidate();
        }

        public static void TrackBarPaintHandler(object? sender, PaintEventArgs e)
        {
            if (sender is TrackBar tb)
            {
                // Always get the current theme colors (not cached) to ensure accent color updates
                var currentTheme = CatppuccinTheme.GetThemeColors(CurrentFlavor, CurrentAccentColor);

                // Enable anti-aliasing for smoother graphics
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Clear the entire background to ensure no default rendering shows through
                e.Graphics.Clear(currentTheme.Background);

                // Fill the entire control area to override any default rendering
                using var backgroundBrush = new SolidBrush(currentTheme.Background);
                e.Graphics.FillRectangle(backgroundBrush, 0, 0, tb.Width, tb.Height);

                // Calculate track dimensions with better positioning
                var trackHeight = 8;
                var trackRect = new Rectangle(20, tb.Height / 2 - trackHeight / 2, tb.Width - 40, trackHeight);

                // Draw track background with rounded corners
                using var trackBrush = new SolidBrush(currentTheme.TrackBarTrack);
                e.Graphics.FillRoundedRectangle(trackBrush, trackRect, trackHeight / 2);

                // Calculate thumb position
                var thumbPos = (int)(trackRect.Left + (double)(tb.Value - tb.Minimum) / (tb.Maximum - tb.Minimum) * trackRect.Width);
                var thumbSize = 24; // Increased from 20 to 24 for better visibility and clicking
                var thumbRect = new Rectangle(thumbPos - thumbSize / 2, tb.Height / 2 - thumbSize / 2, thumbSize, thumbSize);

                // Draw filled portion of track (from start to thumb) with accent color
                var filledRect = new Rectangle(trackRect.Left, trackRect.Top, Math.Max(0, thumbPos - trackRect.Left), trackRect.Height);
                using var accentBrush = new SolidBrush(currentTheme.Accent);
                if (filledRect.Width > 0)
                {
                    e.Graphics.FillRoundedRectangle(accentBrush, filledRect, trackHeight / 2);
                }

                // Draw thumb with accent color and better styling
                e.Graphics.FillEllipse(accentBrush, thumbRect);

                // Draw thumb border for definition - make it more prominent
                using var thumbBorderPen = new Pen(currentTheme.Background, 4); // Increased from 3 to 4
                e.Graphics.DrawEllipse(thumbBorderPen, thumbRect);

                // Add inner border for better definition
                using var innerBorderPen = new Pen(currentTheme.SubtleBorder, 2); // Increased from 1 to 2
                var innerRect = new Rectangle(thumbRect.X + 3, thumbRect.Y + 3, thumbRect.Width - 6, thumbRect.Height - 6);
                e.Graphics.DrawEllipse(innerBorderPen, innerRect);

                // Add a highlight to make the thumb more visible
                using var highlightBrush = new SolidBrush(Color.FromArgb(60, Color.White));
                var highlightSize = thumbRect.Width / 4;
                var highlightRect = new Rectangle(thumbRect.X + thumbRect.Width / 4, thumbRect.Y + thumbRect.Height / 4, highlightSize, highlightSize);
                e.Graphics.FillEllipse(highlightBrush, highlightRect);
            }
        }

        private static void TrackBarMouseDownHandler(object? sender, MouseEventArgs e)
        {
            if (sender is TrackBar tb && e.Button == MouseButtons.Left)
            {
                // Calculate track dimensions (same as in paint handler)
                var trackHeight = 8;
                var trackRect = new Rectangle(20, tb.Height / 2 - trackHeight / 2, tb.Width - 40, trackHeight);

                // Allow clicking anywhere along the track horizontally
                if (e.X >= trackRect.Left && e.X <= trackRect.Right)
                {
                    // Calculate new value based on click position
                    var percentage = Math.Max(0, Math.Min(1, (double)(e.X - trackRect.Left) / trackRect.Width));
                    var newValue = (int)(tb.Minimum + percentage * (tb.Maximum - tb.Minimum));

                    // Set the new value (this will trigger ValueChanged event)
                    tb.Value = Math.Max(tb.Minimum, Math.Min(tb.Maximum, newValue));

                    // Force immediate repaint
                    tb.Invalidate();

                    // Capture mouse for dragging
                    tb.Capture = true;
                }
            }
        }

        private static void TrackBarMouseMoveHandler(object? sender, MouseEventArgs e)
        {
            if (sender is TrackBar tb && tb.Capture && e.Button == MouseButtons.Left)
            {
                // Calculate track dimensions (same as in paint handler)
                var trackHeight = 8;
                var trackRect = new Rectangle(20, tb.Height / 2 - trackHeight / 2, tb.Width - 40, trackHeight);

                // Update value based on mouse position during drag
                if (e.X >= trackRect.Left && e.X <= trackRect.Right)
                {
                    var percentage = Math.Max(0, Math.Min(1, (double)(e.X - trackRect.Left) / trackRect.Width));
                    var newValue = (int)(tb.Minimum + percentage * (tb.Maximum - tb.Minimum));

                    // Set the new value (this will trigger ValueChanged event)
                    tb.Value = Math.Max(tb.Minimum, Math.Min(tb.Maximum, newValue));

                    // Force immediate repaint
                    tb.Invalidate();
                }
            }
        }

        private static void TrackBarMouseUpHandler(object? sender, MouseEventArgs e)
        {
            if (sender is TrackBar tb)
            {
                // Release mouse capture
                tb.Capture = false;
                tb.Invalidate();
            }
        }

        // Extension method to draw rounded rectangles
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle rect, int radius)
        {
            using var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            graphics.FillPath(brush, path);
        }

        private static void ApplyComboBoxTheme(ComboBox comboBox)
        {
            comboBox.BackColor = CurrentTheme.Surface;
            comboBox.ForeColor = CurrentTheme.Text;
            comboBox.FlatStyle = FlatStyle.Flat;

            // Custom draw for better theming
            comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox.DrawItem += (sender, e) =>
            {
                if (e.Index < 0) return;

                var cb = sender as ComboBox;
                if (cb == null) return;

                // Determine colors based on selection state
                var backgroundColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected
            ? CurrentTheme.Accent
            : CurrentTheme.Surface;
                var textColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected
            ? CurrentTheme.Background  // Use background color for contrast against accent
            : CurrentTheme.Text;

                // Draw background
                using var backgroundBrush = new SolidBrush(backgroundColor);
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

                // Draw text
                var text = cb.GetItemText(cb.Items[e.Index]);
                TextRenderer.DrawText(e.Graphics, text, cb.Font, e.Bounds, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                // Draw focus rectangle if needed
                if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                {
                    using var focusPen = new Pen(CurrentTheme.SubtleBorder, 1);
                    e.Graphics.DrawRectangle(focusPen, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));
                }
            };
        }

        private static void ApplyGroupBoxTheme(GroupBox groupBox)
        {
            groupBox.BackColor = CurrentTheme.Background;
            groupBox.ForeColor = CurrentTheme.TextSecondary; // Use more subtle text color
            groupBox.FlatStyle = FlatStyle.Flat;

            // Increase font size more significantly for group box headers to create hierarchy
            if (groupBox.Font.Size <= 10) // Increased threshold to catch more cases
            {
                groupBox.Font = new Font(groupBox.Font.FontFamily, groupBox.Font.Size + 2, groupBox.Font.Style); // +2 instead of +1 for more prominence
            }

            // Increase breathing room with more generous padding - extra left padding for rounded corners
            groupBox.Padding = new Padding(20, 12, 16, 16); // Increased from (16, 8, 12, 12)

            // Use subtle border for group boxes with rounded corners and text gap
            groupBox.Paint += (sender, e) =>
            {
                if (sender is GroupBox gb)
                {
                    // Clear the entire area first to prevent artifacts
                    e.Graphics.Clear(gb.BackColor);

                    // Enable anti-aliasing for smooth rounded corners
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // Use an even more subtle border color
                    using var pen = new Pen(CurrentTheme.PanelBorder, 1);

                    // Calculate text dimensions for the gap more precisely
                    var textSize = TextRenderer.MeasureText(gb.Text, gb.Font);
                    var textHeight = gb.Font.Height / 2;
                    var textStart = 16; // Increased from 12 for more space for rounded corner
                    var textEnd = textStart + textSize.Width + 6; // Increased padding

                    // Border coordinates with rounded corners
                    var left = 0;
                    var top = textHeight;
                    var right = gb.Width - 1;
                    var bottom = gb.Height - 2;
                    var cornerRadius = 8; // Rounded corner radius

                    // Create rounded rectangle path
                    using var path = new System.Drawing.Drawing2D.GraphicsPath();

                    // Top-left corner (with gap consideration)
                    if (textStart > cornerRadius)
                    {
                        path.AddArc(left, top, cornerRadius * 2, cornerRadius * 2, 180, 90);
                        path.AddLine(left + cornerRadius, top, textStart, top);
                    }
                    else
                    {
                        path.AddLine(left + cornerRadius, top, textStart, top);
                    }

                    // Gap for text
                    path.StartFigure();

                    // Top line after text gap to top-right corner
                    if (textEnd < right - cornerRadius)
                    {
                        path.AddLine(textEnd, top, right - cornerRadius, top);
                        path.AddArc(right - cornerRadius * 2, top, cornerRadius * 2, cornerRadius * 2, 270, 90);
                    }
                    else
                    {
                        path.AddLine(textEnd, top, right, top);
                        path.StartFigure();
                        path.AddLine(right, top, right, top + cornerRadius);
                    }

                    // Right side
                    path.AddLine(right, top + cornerRadius, right, bottom - cornerRadius);

                    // Bottom-right corner
                    path.AddArc(right - cornerRadius * 2, bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);

                    // Bottom side
                    path.AddLine(right - cornerRadius, bottom, left + cornerRadius, bottom);

                    // Bottom-left corner
                    path.AddArc(left, bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);

                    // Left side
                    path.AddLine(left, bottom - cornerRadius, left, top + cornerRadius);

                    // Draw the rounded border
                    e.Graphics.DrawPath(pen, path);

                    // Draw the text on top to ensure it's visible with subtle color
                    var textRect = new Rectangle(textStart, 0, textSize.Width, gb.Font.Height);
                    TextRenderer.DrawText(e.Graphics, gb.Text, gb.Font, textRect, CurrentTheme.TextSecondary, gb.BackColor);
                }
            };
        }

        private static void ApplyPanelTheme(Panel panel)
        {
            // Check if this is a status bar panel (docked to bottom)
            if (panel.Dock == DockStyle.Bottom)
            {
                ApplyStatusBarTheme(panel);
                return;
            }

            panel.BackColor = CurrentTheme.Surface;

            // Increase breathing room with more generous padding
            panel.Padding = new Padding(12); // Increased from 8

            // Add subtle border to panels with precise pixel control
            panel.Paint += (sender, e) =>
            {
                if (sender is Panel p)
                {
                    // Only draw border if the panel doesn't have a parent panel (avoid nested borders)
                    if (!(p.Parent is Panel))
                    {
                        using var pen = new Pen(CurrentTheme.PanelBorder, 1);

                        // Use more precise coordinates to ensure even borders
                        var left = 0;
                        var top = 0;
                        var right = p.Width - 1;
                        var bottom = p.Height - 2; // Adjust bottom to prevent thickness

                        // Draw border lines with precise positioning
                        e.Graphics.DrawLine(pen, left, top, right, top);        // Top
                        e.Graphics.DrawLine(pen, left, top, left, bottom);      // Left
                        e.Graphics.DrawLine(pen, right, top, right, bottom);    // Right
                        e.Graphics.DrawLine(pen, left, bottom, right, bottom);  // Bottom
                    }
                }
            };
        }

        private static void ApplyStatusBarTheme(Panel statusPanel)
        {
            // Status bar gets a darker background for visual separation
            var palette = CatppuccinTheme.GetPalette(CurrentFlavor);
            statusPanel.BackColor = palette.Mantle; // Use Mantle instead of Background for darker appearance

            // Increase padding for status bar as well
            if (statusPanel.Padding == Padding.Empty)
            {
                statusPanel.Padding = new Padding(12); // Increased from 8
            }

            // Add a subtle top border only
            statusPanel.Paint += (sender, e) =>
            {
                if (sender is Panel p)
                {
                    using var pen = new Pen(CurrentTheme.SubtleBorder, 1);
                    e.Graphics.DrawLine(pen, 0, 0, p.Width, 0); // Top border only
                }
            };

            // Apply theme to child controls with status bar specific styling
            foreach (Control child in statusPanel.Controls)
            {
                if (child is LinkLabel linkLabel)
                {
                    // Make status bar links more subtle with transparent background
                    linkLabel.BackColor = Color.Transparent; // Transparent to blend with status bar
                    linkLabel.LinkColor = CurrentTheme.TextSecondary;
                    linkLabel.VisitedLinkColor = CurrentTheme.TextSecondary;
                    linkLabel.ActiveLinkColor = CurrentTheme.Accent;
                }
                else if (child is Label label)
                {
                    // Make status bar text more subtle with transparent background
                    label.BackColor = Color.Transparent; // Transparent to blend with status bar
                    label.ForeColor = CurrentTheme.TextSecondary;
                }
            }
        }

        private static void ApplyLinkLabelTheme(LinkLabel linkLabel)
        {
            linkLabel.BackColor = CurrentTheme.Background;
            linkLabel.LinkColor = CurrentTheme.LinkColor;
            linkLabel.VisitedLinkColor = CurrentTheme.LinkHover;
            linkLabel.ActiveLinkColor = CurrentTheme.Primary;
        }

        private static void ApplyLabelTheme(Label label)
        {
            label.BackColor = CurrentTheme.Background;
            label.ForeColor = CurrentTheme.Text;

            // Add more spacing around labels
            label.Margin = new Padding(4, 8, 4, 4); // Increased spacing
        }

        private static void ApplyPictureBoxTheme(PictureBox pictureBox)
        {
            pictureBox.BackColor = CurrentTheme.Background;
        }

        private static void ApplyContextMenuTheme(ContextMenuStrip contextMenu)
        {
            contextMenu.BackColor = CurrentTheme.Surface;
            contextMenu.ForeColor = CurrentTheme.Text;
            contextMenu.Renderer = new CatppuccinToolStripRenderer();

            // Apply theme to menu items
            foreach (ToolStripItem item in contextMenu.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    ApplyMenuItemTheme(menuItem);
                }
            }
        }

        private static void ApplyMenuItemTheme(ToolStripMenuItem menuItem)
        {
            menuItem.BackColor = CurrentTheme.Surface;
            menuItem.ForeColor = CurrentTheme.Text;
        }

        /// <summary>
        /// Apply the current theme globally
        /// </summary>
        private static void ApplyCurrentTheme()
        {
            // This could be extended to apply theme to all open forms
            // For now, forms will need to call ApplyTheme individually
        }

        /// <summary>
        /// Load theme preference from registry
        /// </summary>
        private static void LoadThemeFromRegistry()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(REGROOT);
                if (key != null)
                {
                    // Load theme flavor
                    if (key.GetValue(THEME_REGISTRY_KEY) is string themeValue)
                    {
                        if (Enum.TryParse<CatppuccinTheme.Flavor>(themeValue, out var flavor))
                        {
                            CurrentFlavor = flavor;
                        }
                    }

                    // Load accent color
                    if (key.GetValue(ACCENT_REGISTRY_KEY) is string accentValue)
                    {
                        if (Enum.TryParse<CatppuccinTheme.AccentColor>(accentValue, out var accentColor))
                        {
                            CurrentAccentColor = accentColor;
                        }
                    }

                    CurrentTheme = CatppuccinTheme.GetThemeColors(CurrentFlavor, CurrentAccentColor);
                }
            }
            catch
            {
                // If loading fails, use default theme (Mocha with Lavender accent)
                CurrentFlavor = CatppuccinTheme.Flavor.Latte;
                CurrentAccentColor = CatppuccinTheme.AccentColor.Sky;
                CurrentTheme = CatppuccinTheme.GetThemeColors(CurrentFlavor, CurrentAccentColor);
            }
        }

        /// <summary>
        /// Save theme preference to registry
        /// </summary>
        private static void SaveThemeToRegistry()
        {
            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(REGROOT);
                key.SetValue(THEME_REGISTRY_KEY, CurrentFlavor.ToString());
                key.SetValue(ACCENT_REGISTRY_KEY, CurrentAccentColor.ToString());
            }
            catch
            {
                // Silently fail if we can't save to registry
            }
        }
    }

    /// <summary>
    /// Custom renderer for ToolStrip controls to match Catppuccin theme
    /// </summary>
    public class CatppuccinToolStripRenderer : ToolStripProfessionalRenderer
    {
        public CatppuccinToolStripRenderer() : base(new CatppuccinColorTable())
        {
        }
    }

    /// <summary>
    /// Color table for ToolStrip controls
    /// </summary>
    public class CatppuccinColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => ThemeManager.CurrentTheme.ButtonHover;
        public override Color MenuItemBorder => ThemeManager.CurrentTheme.SubtleBorder;
        public override Color MenuBorder => ThemeManager.CurrentTheme.SubtleBorder;
        public override Color MenuItemSelectedGradientBegin => ThemeManager.CurrentTheme.ButtonHover;
        public override Color MenuItemSelectedGradientEnd => ThemeManager.CurrentTheme.ButtonHover;
        public override Color MenuItemPressedGradientBegin => ThemeManager.CurrentTheme.Accent;
        public override Color MenuItemPressedGradientEnd => ThemeManager.CurrentTheme.Accent;
        public override Color ToolStripDropDownBackground => ThemeManager.CurrentTheme.Surface;
        public override Color ImageMarginGradientBegin => ThemeManager.CurrentTheme.Surface;
        public override Color ImageMarginGradientMiddle => ThemeManager.CurrentTheme.Surface;
        public override Color ImageMarginGradientEnd => ThemeManager.CurrentTheme.Surface;
        public override Color SeparatorDark => ThemeManager.CurrentTheme.DividerColor;
        public override Color SeparatorLight => ThemeManager.CurrentTheme.Surface;
    }

    /// <summary>
    /// Custom TrackBar that supports full theming
    /// </summary>
    public class ThemedTrackBar : TrackBar
    {
        public ThemedTrackBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Use our custom paint handler
            ThemeManager.TrackBarPaintHandler(this, e);
        }
    }

    /// <summary>
    /// Completely custom slider control with full theming support
    /// </summary>
    public class CustomSlider : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private bool _isDragging = false;
        private int _tickFrequency = 10;
        private Rectangle _trackRect;
        private Rectangle _thumbRect;

        public event EventHandler? ValueChanged;

        [Browsable(true)]
        [DefaultValue(0)]
        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                if (_value < _minimum) Value = _minimum;
                Invalidate();
            }
        }

        [Browsable(true)]
        [DefaultValue(100)]
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                if (_value > _maximum) Value = _maximum;
                Invalidate();
            }
        }

        [Browsable(true)]
        [DefaultValue(0)]
        public int Value
        {
            get => _value;
            set
            {
                var newValue = Math.Max(_minimum, Math.Min(_maximum, value));
                if (_value != newValue)
                {
                    _value = newValue;
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(10)]
        public int TickFrequency
        {
            get => _tickFrequency;
            set
            {
                _tickFrequency = value;
                Invalidate();
            }
        }

        public CustomSlider()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            Size = new Size(200, 45);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var currentTheme = ThemeManager.CurrentTheme;

            // Enable anti-aliasing
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Clear background
            e.Graphics.Clear(currentTheme.Background);

            // Calculate dimensions
            var trackHeight = 8;
            _trackRect = new Rectangle(20, Height / 2 - trackHeight / 2, Width - 40, trackHeight);

            // Draw track background
            using var trackBrush = new SolidBrush(currentTheme.TrackBarTrack);
            e.Graphics.FillRoundedRectangle(trackBrush, _trackRect, trackHeight / 2);

            // Calculate thumb position
            var thumbPos = (int)(_trackRect.Left + (double)(_value - _minimum) / (_maximum - _minimum) * _trackRect.Width);
            var thumbSize = 24; // Increased from 20 to 24 for better visibility and clicking
            _thumbRect = new Rectangle(thumbPos - thumbSize / 2, Height / 2 - thumbSize / 2, thumbSize, thumbSize);

            // Draw filled portion
            var filledRect = new Rectangle(_trackRect.Left, _trackRect.Top, Math.Max(0, thumbPos - _trackRect.Left), _trackRect.Height);
            using var accentBrush = new SolidBrush(currentTheme.Accent);
            if (filledRect.Width > 0)
            {
                e.Graphics.FillRoundedRectangle(accentBrush, filledRect, trackHeight / 2);
            }

            // Draw thumb
            e.Graphics.FillEllipse(accentBrush, _thumbRect);

            // Draw thumb border - make it more prominent
            using var thumbBorderPen = new Pen(currentTheme.Background, 4);
            e.Graphics.DrawEllipse(thumbBorderPen, _thumbRect);

            // Draw inner border
            using var innerBorderPen = new Pen(currentTheme.SubtleBorder, 2);
            var innerRect = new Rectangle(_thumbRect.X + 3, _thumbRect.Y + 3, _thumbRect.Width - 6, _thumbRect.Height - 6);
            e.Graphics.DrawEllipse(innerBorderPen, innerRect);

            // Add a subtle highlight to make the thumb more visible (fix positioning and color)
            using var highlightBrush = new SolidBrush(Color.FromArgb(60, Color.White));
            var highlightSize = _thumbRect.Width / 4;
            var highlightRect = new Rectangle(_thumbRect.X + _thumbRect.Width / 4, _thumbRect.Y + _thumbRect.Height / 4, highlightSize, highlightSize);
            e.Graphics.FillEllipse(highlightBrush, highlightRect);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // First, ensure we have the current thumb position calculated
                var trackHeight = 8;
                _trackRect = new Rectangle(20, Height / 2 - trackHeight / 2, Width - 40, trackHeight);
                var thumbPos = (int)(_trackRect.Left + (double)(_value - _minimum) / (_maximum - _minimum) * _trackRect.Width);
                var thumbSize = 24;
                _thumbRect = new Rectangle(thumbPos - thumbSize / 2, Height / 2 - thumbSize / 2, thumbSize, thumbSize);

                // Simplified click detection - allow clicking anywhere on the entire track area or control
                // This makes it much easier to interact with the slider
                if (e.X >= _trackRect.Left && e.X <= _trackRect.Right)
                {
                    _isDragging = true;
                    UpdateValueFromMouse(e.X);
                    Capture = true;
                    Invalidate(); // Force immediate repaint
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                UpdateValueFromMouse(e.X);

                // Recalculate thumb position after value update for accurate hit detection
                var trackHeight = 8;
                _trackRect = new Rectangle(20, Height / 2 - trackHeight / 2, Width - 40, trackHeight);
                var thumbPos = (int)(_trackRect.Left + (double)(_value - _minimum) / (_maximum - _minimum) * _trackRect.Width);
                var thumbSize = 24;
                _thumbRect = new Rectangle(thumbPos - thumbSize / 2, Height / 2 - thumbSize / 2, thumbSize, thumbSize);

                Invalidate(); // Force immediate repaint during dragging
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                Capture = false;
                Invalidate(); // Force final repaint
            }
            base.OnMouseUp(e);
        }

        private void UpdateValueFromMouse(int mouseX)
        {
            var percentage = Math.Max(0, Math.Min(1, (double)(mouseX - _trackRect.Left) / _trackRect.Width));
            var newValue = (int)(_minimum + percentage * (_maximum - _minimum));

            // Only update if the value actually changed to reduce flickering
            if (newValue != _value)
            {
                Value = newValue;
            }
        }
    }
}
