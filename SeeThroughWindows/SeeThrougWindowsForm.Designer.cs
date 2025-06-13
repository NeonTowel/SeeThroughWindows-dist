namespace SeeThroughWindows
{
  partial class SeeThrougWindowsForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      Label label3;
      Label label4;
      Label label1;
      Label label2;
      Label transparencyValueLabel;
      Label themeLabel;
      Label accentColorLabel;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeeThrougWindowsForm));
      toolStripSeparator1 = new ToolStripSeparator();
      pictureBox1 = new PictureBox();
      notifyIcon = new NotifyIcon(components);
      contextMenuStrip = new ContextMenuStrip(components);
      optionsToolStripMenuItem = new ToolStripMenuItem();
      exitToolStripMenuItem = new ToolStripMenuItem();
      shiftCheckBox = new CheckBox();
      hotKeyComboBox = new ComboBox();
      controlCheckBox = new CheckBox();
      altCheckBox = new CheckBox();
      windowsCheckBox = new CheckBox();
      transparencyTrackBar = new TrackBar();
      previewCheckBox = new CheckBox();
      helpLink = new LinkLabel();
      groupBox1 = new GroupBox();
      restoreAllButton = new Button();
      enableChangeTransparencyCheckbox = new CheckBox();
      topMostCheckBox = new CheckBox();
      clickThroughCheckBox = new CheckBox();
      autoApplyOnStartupCheckBox = new CheckBox();
      groupBox2 = new GroupBox();
      sendToMonitorEnabledCheckBox = new CheckBox();
      minMaxEnabledCheckBox = new CheckBox();
      groupBox3 = new GroupBox();
      themeComboBox = new ComboBox();
      updateAvailableLink = new LinkLabel();
      panel1 = new Panel();
      label3 = new Label();
      label4 = new Label();
      label1 = new Label();
      label2 = new Label();
      this.transparencyValueLabel = new Label();
      themeLabel = new Label();
      accentColorLabel = new Label();
      accentColorComboBox = new ComboBox();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      contextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)transparencyTrackBar).BeginInit();
      groupBox1.SuspendLayout();
      groupBox2.SuspendLayout();
      groupBox3.SuspendLayout();
      panel1.SuspendLayout();
      SuspendLayout();
      //
      // label3
      //
      label3.AutoSize = false;
      label3.Location = new Point(148, 143);
      label3.Margin = new Padding(4, 0, 4, 0);
      label3.Name = "label3";
      label3.Size = new Size(85, 20);
      label3.TabIndex = 8;
      label3.Text = "Transparent";
      label3.TextAlign = ContentAlignment.MiddleLeft;
      label3.Font = new Font(label3.Font.FontFamily, label3.Font.Size + 1, label3.Font.Style);
      //
      // label4
      //
      label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      label4.AutoSize = false;
      label4.Location = new Point(482, 135);
      label4.Margin = new Padding(4, 0, 4, 0);
      label4.Name = "label4";
      label4.Size = new Size(65, 20);
      label4.TabIndex = 9;
      label4.Text = "Opaque";
      label4.TextAlign = ContentAlignment.MiddleRight;
      label4.Font = new Font(label4.Font.FontFamily, label4.Font.Size + 1, label4.Font.Style);
      //
      // label1
      //
      label1.AutoSize = false;
      label1.Location = new Point(55, 37);
      label1.Margin = new Padding(4, 0, 4, 0);
      label1.Name = "label1";
      label1.Size = new Size(60, 20);
      label1.TabIndex = 0;
      label1.Text = "&Hotkey:";
      label1.TextAlign = ContentAlignment.MiddleRight;
      //
      // label2
      //
      label2.AutoSize = false;
      label2.Location = new Point(24, 127);
      label2.Margin = new Padding(4, 0, 4, 0);
      label2.Name = "label2";
      label2.Size = new Size(120, 20);
      label2.TabIndex = 6;
      label2.Text = "&Transparency:";
      label2.TextAlign = ContentAlignment.MiddleRight;
      //
      // transparencyValueLabel
      //
      this.transparencyValueLabel.AutoSize = false;
      this.transparencyValueLabel.Location = new Point(310, 143);
      this.transparencyValueLabel.Margin = new Padding(4, 0, 4, 0);
      this.transparencyValueLabel.Name = "transparencyValueLabel";
      this.transparencyValueLabel.Size = new Size(60, 20);
      this.transparencyValueLabel.TabIndex = 15;
      this.transparencyValueLabel.Text = "96%";
      this.transparencyValueLabel.TextAlign = ContentAlignment.MiddleCenter;
      this.transparencyValueLabel.Font = new Font(this.transparencyValueLabel.Font.FontFamily, this.transparencyValueLabel.Font.Size + 1, this.transparencyValueLabel.Font.Style);
      //
      // themeLabel
      //
      themeLabel.AutoSize = true;
      themeLabel.Location = new Point(4, 22);
      themeLabel.Margin = new Padding(4, 0, 4, 0);
      themeLabel.Name = "themeLabel";
      themeLabel.Size = new Size(43, 15);
      themeLabel.TabIndex = 0;
      themeLabel.Text = "&Theme:";
      themeLabel.TextAlign = ContentAlignment.MiddleRight;
      //
      // accentColorLabel
      //
      accentColorLabel.AutoSize = true;
      accentColorLabel.Location = new Point(4, 57);
      accentColorLabel.Margin = new Padding(4, 0, 4, 0);
      accentColorLabel.Name = "accentColorLabel";
      accentColorLabel.Size = new Size(78, 15);
      accentColorLabel.TabIndex = 2;
      accentColorLabel.Text = "&Accent Color:";
      accentColorLabel.TextAlign = ContentAlignment.MiddleRight;
      //
      // toolStripSeparator1
      //
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new Size(122, 6);
      //
      // pictureBox1
      //
      pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
      pictureBox1.Location = new Point(0, 0);
      pictureBox1.Margin = new Padding(4);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(32, 28);
      pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
      pictureBox1.TabIndex = 12;
      pictureBox1.TabStop = false;
      //
      // notifyIcon
      //
      notifyIcon.ContextMenuStrip = contextMenuStrip;
      notifyIcon.Text = "See Through Windows";
      notifyIcon.Visible = true;
      notifyIcon.MouseUp += notifyIcon_MouseUp;
      //
      // contextMenuStrip
      //
      contextMenuStrip.ImageScalingSize = new Size(24, 24);
      contextMenuStrip.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
      contextMenuStrip.Name = "contextMenuStrip";
      contextMenuStrip.Size = new Size(126, 54);
      //
      // optionsToolStripMenuItem
      //
      optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      optionsToolStripMenuItem.Size = new Size(125, 22);
      optionsToolStripMenuItem.Text = "Options...";
      optionsToolStripMenuItem.MouseUp += notifyIcon_MouseUp;
      //
      // exitToolStripMenuItem
      //
      exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      exitToolStripMenuItem.Size = new Size(125, 22);
      exitToolStripMenuItem.Text = "E&xit";
      exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
      //
      // shiftCheckBox
      //
      shiftCheckBox.AutoSize = false;
      shiftCheckBox.Location = new Point(135, 75);
      shiftCheckBox.Margin = new Padding(6, 8, 6, 8);
      shiftCheckBox.Name = "shiftCheckBox";
      shiftCheckBox.Size = new Size(70, 19);
      shiftCheckBox.TabIndex = 2;
      shiftCheckBox.Text = "&Shift";
      shiftCheckBox.UseVisualStyleBackColor = true;
      shiftCheckBox.CheckedChanged += UpdateHotKey;
      //
      // hotKeyComboBox
      //
      hotKeyComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      hotKeyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      hotKeyComboBox.FlatStyle = FlatStyle.System;
      hotKeyComboBox.FormattingEnabled = true;
      hotKeyComboBox.Location = new Point(120, 30);
      hotKeyComboBox.Margin = new Padding(6, 8, 6, 8);
      hotKeyComboBox.Name = "hotKeyComboBox";
      hotKeyComboBox.Size = new Size(440, 23);
      hotKeyComboBox.TabIndex = 1;
      hotKeyComboBox.SelectedIndexChanged += UpdateHotKey;
      //
      // controlCheckBox
      //
      controlCheckBox.AutoSize = false;
      controlCheckBox.Location = new Point(205, 75);
      controlCheckBox.Margin = new Padding(6, 8, 6, 8);
      controlCheckBox.Name = "controlCheckBox";
      controlCheckBox.Size = new Size(80, 19);
      controlCheckBox.TabIndex = 3;
      controlCheckBox.Text = "&Control";
      controlCheckBox.UseVisualStyleBackColor = true;
      controlCheckBox.CheckedChanged += UpdateHotKey;
      //
      // altCheckBox
      //
      altCheckBox.AutoSize = false;
      altCheckBox.Location = new Point(291, 75);
      altCheckBox.Margin = new Padding(6, 8, 6, 8);
      altCheckBox.Name = "altCheckBox";
      altCheckBox.Size = new Size(55, 19);
      altCheckBox.TabIndex = 4;
      altCheckBox.Text = "&Alt";
      altCheckBox.UseVisualStyleBackColor = true;
      altCheckBox.CheckedChanged += UpdateHotKey;
      //
      // windowsCheckBox
      //
      windowsCheckBox.AutoSize = false;
      windowsCheckBox.Location = new Point(352, 75);
      windowsCheckBox.Margin = new Padding(6, 8, 6, 8);
      windowsCheckBox.Name = "windowsCheckBox";
      windowsCheckBox.Size = new Size(90, 19);
      windowsCheckBox.TabIndex = 5;
      windowsCheckBox.Text = "&Windows";
      windowsCheckBox.UseVisualStyleBackColor = true;
      windowsCheckBox.CheckedChanged += UpdateHotKey;
      //
      // transparencyTrackBar
      //
      transparencyTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      transparencyTrackBar.LargeChange = 32;
      transparencyTrackBar.Location = new Point(110, 100);
      transparencyTrackBar.Margin = new Padding(6, 8, 6, 8);
      transparencyTrackBar.Maximum = 250;
      transparencyTrackBar.Minimum = 10;
      transparencyTrackBar.Name = "transparencyTrackBar";
      transparencyTrackBar.Size = new Size(450, 45);
      transparencyTrackBar.SmallChange = 8;
      transparencyTrackBar.TabIndex = 7;
      transparencyTrackBar.TickFrequency = 16;
      transparencyTrackBar.Value = 10;
      transparencyTrackBar.ValueChanged += transparencyTrackBar_ValueChanged;
      //
      // previewCheckBox
      //
      previewCheckBox.AutoSize = false;
      previewCheckBox.Location = new Point(20, 260);
      previewCheckBox.Margin = new Padding(6, 8, 6, 8);
      previewCheckBox.Name = "previewCheckBox";
      previewCheckBox.Size = new Size(180, 19);
      previewCheckBox.TabIndex = 13;
      previewCheckBox.Text = "&Preview transparency";
      previewCheckBox.UseVisualStyleBackColor = true;
      previewCheckBox.CheckedChanged += previewCheckBox_CheckedChanged;
      //
      // helpLink
      //
      helpLink.AutoSize = true;
      helpLink.Location = new Point(0, 0);
      helpLink.Margin = new Padding(4, 0, 4, 0);
      helpLink.Name = "helpLink";
      helpLink.Size = new Size(313, 15);
      helpLink.TabIndex = 0;
      helpLink.TabStop = true;
      helpLink.Text = "";
      helpLink.TextAlign = ContentAlignment.MiddleCenter;
      helpLink.VisitedLinkColor = Color.Blue;
      helpLink.LinkClicked += helpLink_LinkClicked;
      //
      // groupBox1
      //
      groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      groupBox1.Controls.Add(restoreAllButton);
      groupBox1.Controls.Add(enableChangeTransparencyCheckbox);
      groupBox1.Controls.Add(topMostCheckBox);
      groupBox1.Controls.Add(autoApplyOnStartupCheckBox);
      groupBox1.Controls.Add(clickThroughCheckBox);
      groupBox1.Controls.Add(hotKeyComboBox);
      groupBox1.Controls.Add(shiftCheckBox);
      groupBox1.Controls.Add(previewCheckBox);
      groupBox1.Controls.Add(label4);
      groupBox1.Controls.Add(label1);
      groupBox1.Controls.Add(controlCheckBox);
      groupBox1.Controls.Add(label3);
      groupBox1.Controls.Add(this.transparencyValueLabel);
      groupBox1.Controls.Add(altCheckBox);
      groupBox1.Controls.Add(windowsCheckBox);
      groupBox1.Controls.Add(label2);
      groupBox1.Controls.Add(transparencyTrackBar);
      groupBox1.Location = new Point(20, 20);
      groupBox1.Margin = new Padding(10);
      groupBox1.Name = "groupBox1";
      groupBox1.Padding = new Padding(2);
      groupBox1.Size = new Size(580, 405);
      groupBox1.TabIndex = 0;
      groupBox1.TabStop = false;
      groupBox1.Text = "Make window transparent";
      //
      // restoreAllButton
      //
      restoreAllButton.Location = new Point(190, 195);
      restoreAllButton.Margin = new Padding(6, 12, 6, 12);
      restoreAllButton.Name = "restoreAllButton";
      restoreAllButton.Size = new Size(200, 40);
      restoreAllButton.TabIndex = 12;
      restoreAllButton.Text = "Restore all windows";
      restoreAllButton.UseVisualStyleBackColor = true;
      restoreAllButton.Click += restoreAllButton_Click;
      //
      // enableChangeTransparencyCheckbox
      //
      enableChangeTransparencyCheckbox.AutoSize = false;
      enableChangeTransparencyCheckbox.Location = new Point(20, 345);
      enableChangeTransparencyCheckbox.Margin = new Padding(6, 8, 6, 8);
      enableChangeTransparencyCheckbox.Name = "enableChangeTransparencyCheckbox";
      enableChangeTransparencyCheckbox.Size = new Size(550, 19);
      enableChangeTransparencyCheckbox.TabIndex = 14;
      enableChangeTransparencyCheckbox.Text = "Enable Control+Windows+PageUp/PageDown to change &transparency";
      enableChangeTransparencyCheckbox.UseVisualStyleBackColor = true;
      enableChangeTransparencyCheckbox.CheckedChanged += enableChangeTransparencyCheckbox_CheckedChanged;
      //
      // topMostCheckBox
      //
      topMostCheckBox.AutoSize = false;
      topMostCheckBox.Enabled = false;
      topMostCheckBox.Location = new Point(260, 295);
      topMostCheckBox.Margin = new Padding(6, 8, 6, 8);
      topMostCheckBox.Name = "topMostCheckBox";
      topMostCheckBox.Size = new Size(280, 19);
      topMostCheckBox.TabIndex = 11;
      topMostCheckBox.Text = "... and keep &on top of other windows";
      topMostCheckBox.UseVisualStyleBackColor = true;
      topMostCheckBox.CheckedChanged += topMostCheckBox_CheckedChanged;
      //
      // clickThroughCheckBox
      //
      clickThroughCheckBox.AutoSize = false;
      clickThroughCheckBox.Location = new Point(20, 295);
      clickThroughCheckBox.Margin = new Padding(6, 8, 6, 8);
      clickThroughCheckBox.Name = "clickThroughCheckBox";
      clickThroughCheckBox.Size = new Size(220, 19);
      clickThroughCheckBox.TabIndex = 10;
      clickThroughCheckBox.Text = "Make window 'C&lick-through'";
      clickThroughCheckBox.UseVisualStyleBackColor = true;
      clickThroughCheckBox.CheckedChanged += clickThroughCheckBox_CheckedChanged;
      //
      // autoApplyOnStartupCheckBox
      //
      autoApplyOnStartupCheckBox.AutoSize = false;
      autoApplyOnStartupCheckBox.Location = new Point(20, 320);
      autoApplyOnStartupCheckBox.Margin = new Padding(6, 8, 6, 8);
      autoApplyOnStartupCheckBox.Name = "autoApplyOnStartupCheckBox";
      autoApplyOnStartupCheckBox.Size = new Size(550, 19);
      autoApplyOnStartupCheckBox.TabIndex = 13;
      autoApplyOnStartupCheckBox.Text = "&Auto-apply transparency to visible windows on startup";
      autoApplyOnStartupCheckBox.UseVisualStyleBackColor = true;
      autoApplyOnStartupCheckBox.CheckedChanged += autoApplyOnStartupCheckBox_CheckedChanged;
      //
      // groupBox2
      //
      groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      groupBox2.Controls.Add(sendToMonitorEnabledCheckBox);
      groupBox2.Controls.Add(minMaxEnabledCheckBox);
      groupBox2.Location = new Point(20, 445);
      groupBox2.Margin = new Padding(10);
      groupBox2.Name = "groupBox2";
      groupBox2.Padding = new Padding(2);
      groupBox2.Size = new Size(580, 100);
      groupBox2.TabIndex = 1;
      groupBox2.TabStop = false;
      groupBox2.Text = "Move window";
      //
      // sendToMonitorEnabledCheckBox
      //
      sendToMonitorEnabledCheckBox.AutoSize = false;
      sendToMonitorEnabledCheckBox.Location = new Point(20, 60);
      sendToMonitorEnabledCheckBox.Margin = new Padding(6, 8, 6, 12);
      sendToMonitorEnabledCheckBox.Name = "sendToMonitorEnabledCheckBox";
      sendToMonitorEnabledCheckBox.Size = new Size(550, 19);
      sendToMonitorEnabledCheckBox.TabIndex = 1;
      sendToMonitorEnabledCheckBox.Text = "Enable Control+Windows+Left/Right to &send windows to other monitors";
      sendToMonitorEnabledCheckBox.UseVisualStyleBackColor = true;
      sendToMonitorEnabledCheckBox.CheckedChanged += sendToMonitorEnabledCheckBox_CheckedChanged;
      //
      // minMaxEnabledCheckBox
      //
      minMaxEnabledCheckBox.AutoSize = false;
      minMaxEnabledCheckBox.Location = new Point(20, 30);
      minMaxEnabledCheckBox.Margin = new Padding(6, 8, 6, 8);
      minMaxEnabledCheckBox.Name = "minMaxEnabledCheckBox";
      minMaxEnabledCheckBox.Size = new Size(550, 19);
      minMaxEnabledCheckBox.TabIndex = 0;
      minMaxEnabledCheckBox.Text = "Enable Control+Windows+Up/Down to &maximize/mimimize windows";
      minMaxEnabledCheckBox.UseVisualStyleBackColor = true;
      minMaxEnabledCheckBox.CheckedChanged += minMaxEnabledCheckBox_CheckedChanged;
      //
      // groupBox3
      //
      groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      groupBox3.Controls.Add(themeComboBox);
      groupBox3.Controls.Add(themeLabel);
      groupBox3.Controls.Add(accentColorLabel);
      groupBox3.Controls.Add(accentColorComboBox);
      groupBox3.Location = new Point(20, 540);
      groupBox3.Margin = new Padding(10);
      groupBox3.Name = "groupBox3";
      groupBox3.Padding = new Padding(2);
      groupBox3.Size = new Size(580, 110);
      groupBox3.TabIndex = 2;
      groupBox3.TabStop = false;
      groupBox3.Text = "Appearance";
      //
      // themeComboBox
      //
      themeComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      themeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      themeComboBox.FlatStyle = FlatStyle.System;
      themeComboBox.FormattingEnabled = true;
      themeComboBox.Location = new Point(120, 20);
      themeComboBox.Margin = new Padding(6, 8, 6, 8);
      themeComboBox.Name = "themeComboBox";
      themeComboBox.Size = new Size(440, 23);
      themeComboBox.TabIndex = 1;
      themeComboBox.SelectedIndexChanged += themeComboBox_SelectedIndexChanged;
      //
      // accentColorComboBox
      //
      accentColorComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      accentColorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      accentColorComboBox.FlatStyle = FlatStyle.System;
      accentColorComboBox.FormattingEnabled = true;
      accentColorComboBox.Location = new Point(120, 55);
      accentColorComboBox.Margin = new Padding(6, 8, 6, 8);
      accentColorComboBox.Name = "accentColorComboBox";
      accentColorComboBox.Size = new Size(440, 23);
      accentColorComboBox.TabIndex = 3;
      accentColorComboBox.SelectedIndexChanged += accentColorComboBox_SelectedIndexChanged;
      //
      // updateAvailableLink
      //
      updateAvailableLink.AutoSize = true;
      updateAvailableLink.Location = new Point(0, 0);
      updateAvailableLink.Name = "updateAvailableLink";
      updateAvailableLink.Size = new Size(39, 15);
      updateAvailableLink.TabIndex = 1;
      updateAvailableLink.TabStop = true;
      updateAvailableLink.Text = "(auto)";
      updateAvailableLink.TextAlign = ContentAlignment.MiddleCenter;
      updateAvailableLink.Visible = false;
      updateAvailableLink.VisitedLinkColor = Color.Blue;
      updateAvailableLink.LinkClicked += updateAvailableLink_LinkClicked;
      //
      // panel1
      //
      panel1.Controls.Add(pictureBox1);
      panel1.Controls.Add(helpLink);
      panel1.Controls.Add(updateAvailableLink);
      panel1.Dock = DockStyle.Bottom;
      panel1.Location = new Point(0, 680);
      panel1.Name = "panel1";
      panel1.Padding = new Padding(16);
      panel1.Size = new Size(620, 60);
      panel1.TabIndex = 3;
      panel1.BackColor = Color.FromArgb(18, 18, 28);
      //
      // SeeThrougWindowsForm
      //
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(620, 740);
      Controls.Add(panel1);
      Controls.Add(groupBox3);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      Icon = (Icon)resources.GetObject("$this.Icon");
      Margin = new Padding(4);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "SeeThrougWindowsForm";
      Text = "See Through Windows Options";
      Shown += SeeThrougWindowsForm_Shown;
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      contextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)transparencyTrackBar).EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      groupBox3.ResumeLayout(false);
      groupBox3.PerformLayout();
      panel1.ResumeLayout(false);
      panel1.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.NotifyIcon notifyIcon;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.CheckBox shiftCheckBox;
    private System.Windows.Forms.ComboBox hotKeyComboBox;
    private System.Windows.Forms.CheckBox controlCheckBox;
    private System.Windows.Forms.CheckBox altCheckBox;
    private System.Windows.Forms.CheckBox windowsCheckBox;
    private System.Windows.Forms.TrackBar transparencyTrackBar;
    private System.Windows.Forms.CheckBox previewCheckBox;
    private System.Windows.Forms.LinkLabel helpLink;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.ComboBox themeComboBox;
    private System.Windows.Forms.CheckBox sendToMonitorEnabledCheckBox;
    private System.Windows.Forms.CheckBox minMaxEnabledCheckBox;
    private System.Windows.Forms.CheckBox clickThroughCheckBox;
    private System.Windows.Forms.CheckBox topMostCheckBox;
    private System.Windows.Forms.CheckBox enableChangeTransparencyCheckbox;
    private ToolStripSeparator toolStripSeparator1;
    private PictureBox pictureBox1;
    private Button restoreAllButton;
    private LinkLabel updateAvailableLink;
    private Panel panel1;
    private Label themeLabel;
    private Label accentColorLabel;
    private ComboBox accentColorComboBox;
    private Label transparencyValueLabel;
    private System.Windows.Forms.CheckBox autoApplyOnStartupCheckBox;
  }
}

