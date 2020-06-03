namespace FLDebugger.Forms
{
    partial class SettingsDialog
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
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.gbResolution = new System.Windows.Forms.GroupBox();
            this.lblDepth = new System.Windows.Forms.Label();
            this.nudDepth = new System.Windows.Forms.NumericUpDown();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.gbEditor = new System.Windows.Forms.GroupBox();
            this.cbLogPreviewStacktrace = new System.Windows.Forms.CheckBox();
            this.cbLogParserStacktrace = new System.Windows.Forms.CheckBox();
            this.btnLoadTheme = new System.Windows.Forms.Button();
            this.cbThemes = new System.Windows.Forms.ComboBox();
            this.nudLogFontSize = new System.Windows.Forms.NumericUpDown();
            this.nudCodeFontSize = new System.Windows.Forms.NumericUpDown();
            this.lblFontSizeCode = new System.Windows.Forms.Label();
            this.lblFontSizeLog = new System.Windows.Forms.Label();
            this.btnSaveThemeAsDefault = new System.Windows.Forms.Button();
            this.btnSaveTheme = new System.Windows.Forms.Button();
            this.panelColorPreview = new System.Windows.Forms.Panel();
            this.cbEditorColorSetting = new System.Windows.Forms.ComboBox();
            this.btnChangeColor = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnSetHomeDir = new System.Windows.Forms.Button();
            this.btnUnpackKernels = new System.Windows.Forms.Button();
            this.btnReloadKernels = new System.Windows.Forms.Button();
            this.cdChangeThemeColor = new System.Windows.Forms.ColorDialog();
            this.sfdTheme = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.gbResolution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.gbEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCodeFontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // nudWidth
            // 
            this.nudWidth.BackColor = System.Drawing.Color.DimGray;
            this.nudWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudWidth.Location = new System.Drawing.Point(50, 22);
            this.nudWidth.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nudWidth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(45, 16);
            this.nudWidth.TabIndex = 0;
            this.nudWidth.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.nudWidth.ValueChanged += new System.EventHandler(this.nudWidth_ValueChanged);
            // 
            // gbResolution
            // 
            this.gbResolution.BackColor = System.Drawing.Color.DimGray;
            this.gbResolution.Controls.Add(this.lblDepth);
            this.gbResolution.Controls.Add(this.nudDepth);
            this.gbResolution.Controls.Add(this.nudHeight);
            this.gbResolution.Controls.Add(this.lblHeight);
            this.gbResolution.Controls.Add(this.lblWidth);
            this.gbResolution.Controls.Add(this.nudWidth);
            this.gbResolution.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbResolution.Location = new System.Drawing.Point(0, 0);
            this.gbResolution.Name = "gbResolution";
            this.gbResolution.Size = new System.Drawing.Size(343, 86);
            this.gbResolution.TabIndex = 1;
            this.gbResolution.TabStop = false;
            this.gbResolution.Text = "Resolution";
            // 
            // lblDepth
            // 
            this.lblDepth.AutoSize = true;
            this.lblDepth.Location = new System.Drawing.Point(6, 60);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size(39, 13);
            this.lblDepth.TabIndex = 5;
            this.lblDepth.Text = "Depth:";
            // 
            // nudDepth
            // 
            this.nudDepth.BackColor = System.Drawing.Color.DimGray;
            this.nudDepth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudDepth.Location = new System.Drawing.Point(50, 61);
            this.nudDepth.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nudDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDepth.Name = "nudDepth";
            this.nudDepth.Size = new System.Drawing.Size(45, 16);
            this.nudDepth.TabIndex = 4;
            this.nudDepth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDepth.ValueChanged += new System.EventHandler(this.nudDepth_ValueChanged);
            // 
            // nudHeight
            // 
            this.nudHeight.BackColor = System.Drawing.Color.DimGray;
            this.nudHeight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudHeight.Location = new System.Drawing.Point(50, 41);
            this.nudHeight.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(45, 16);
            this.nudHeight.TabIndex = 3;
            this.nudHeight.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudHeight_ValueChanged);
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(6, 40);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 2;
            this.lblHeight.Text = "Height:";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(6, 21);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 1;
            this.lblWidth.Text = "Width:";
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.DimGray;
            this.panelButtons.Controls.Add(this.gbEditor);
            this.panelButtons.Controls.Add(this.btnAbout);
            this.panelButtons.Controls.Add(this.btnSetHomeDir);
            this.panelButtons.Controls.Add(this.btnUnpackKernels);
            this.panelButtons.Controls.Add(this.btnReloadKernels);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(0, 86);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(343, 376);
            this.panelButtons.TabIndex = 2;
            // 
            // gbEditor
            // 
            this.gbEditor.Controls.Add(this.cbLogPreviewStacktrace);
            this.gbEditor.Controls.Add(this.cbLogParserStacktrace);
            this.gbEditor.Controls.Add(this.btnLoadTheme);
            this.gbEditor.Controls.Add(this.cbThemes);
            this.gbEditor.Controls.Add(this.nudLogFontSize);
            this.gbEditor.Controls.Add(this.nudCodeFontSize);
            this.gbEditor.Controls.Add(this.lblFontSizeCode);
            this.gbEditor.Controls.Add(this.lblFontSizeLog);
            this.gbEditor.Controls.Add(this.btnSaveThemeAsDefault);
            this.gbEditor.Controls.Add(this.btnSaveTheme);
            this.gbEditor.Controls.Add(this.panelColorPreview);
            this.gbEditor.Controls.Add(this.cbEditorColorSetting);
            this.gbEditor.Controls.Add(this.btnChangeColor);
            this.gbEditor.Location = new System.Drawing.Point(0, 3);
            this.gbEditor.Name = "gbEditor";
            this.gbEditor.Size = new System.Drawing.Size(343, 254);
            this.gbEditor.TabIndex = 18;
            this.gbEditor.TabStop = false;
            this.gbEditor.Text = "Editor";
            // 
            // cbLogPreviewStacktrace
            // 
            this.cbLogPreviewStacktrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbLogPreviewStacktrace.AutoSize = true;
            this.cbLogPreviewStacktrace.Location = new System.Drawing.Point(191, 137);
            this.cbLogPreviewStacktrace.Name = "cbLogPreviewStacktrace";
            this.cbLogPreviewStacktrace.Size = new System.Drawing.Size(140, 17);
            this.cbLogPreviewStacktrace.TabIndex = 31;
            this.cbLogPreviewStacktrace.Text = "Log Preview Stacktrace";
            this.cbLogPreviewStacktrace.UseVisualStyleBackColor = true;
            this.cbLogPreviewStacktrace.CheckedChanged += new System.EventHandler(this.cbLogPreviewStacktrace_CheckedChanged);
            // 
            // cbLogParserStacktrace
            // 
            this.cbLogParserStacktrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbLogParserStacktrace.AutoSize = true;
            this.cbLogParserStacktrace.Location = new System.Drawing.Point(12, 137);
            this.cbLogParserStacktrace.Name = "cbLogParserStacktrace";
            this.cbLogParserStacktrace.Size = new System.Drawing.Size(132, 17);
            this.cbLogParserStacktrace.TabIndex = 30;
            this.cbLogParserStacktrace.Text = "Log Parser Stacktrace";
            this.cbLogParserStacktrace.UseVisualStyleBackColor = true;
            this.cbLogParserStacktrace.CheckedChanged += new System.EventHandler(this.cbLogParserStacktrace_CheckedChanged);
            // 
            // btnLoadTheme
            // 
            this.btnLoadTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadTheme.BackColor = System.Drawing.Color.DimGray;
            this.btnLoadTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadTheme.Location = new System.Drawing.Point(254, 227);
            this.btnLoadTheme.Name = "btnLoadTheme";
            this.btnLoadTheme.Size = new System.Drawing.Size(89, 23);
            this.btnLoadTheme.TabIndex = 29;
            this.btnLoadTheme.Text = "Load Theme";
            this.btnLoadTheme.UseVisualStyleBackColor = false;
            this.btnLoadTheme.Click += new System.EventHandler(this.btnLoadTheme_Click);
            // 
            // cbThemes
            // 
            this.cbThemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThemes.FormattingEnabled = true;
            this.cbThemes.Location = new System.Drawing.Point(0, 228);
            this.cbThemes.Name = "cbThemes";
            this.cbThemes.Size = new System.Drawing.Size(248, 21);
            this.cbThemes.TabIndex = 28;
            // 
            // nudLogFontSize
            // 
            this.nudLogFontSize.DecimalPlaces = 2;
            this.nudLogFontSize.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudLogFontSize.Location = new System.Drawing.Point(97, 88);
            this.nudLogFontSize.Name = "nudLogFontSize";
            this.nudLogFontSize.Size = new System.Drawing.Size(47, 20);
            this.nudLogFontSize.TabIndex = 27;
            this.nudLogFontSize.Value = new decimal(new int[] {
            825,
            0,
            0,
            131072});
            this.nudLogFontSize.ValueChanged += new System.EventHandler(this.nudLogFontSize_ValueChanged);
            // 
            // nudCodeFontSize
            // 
            this.nudCodeFontSize.DecimalPlaces = 2;
            this.nudCodeFontSize.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudCodeFontSize.Location = new System.Drawing.Point(97, 111);
            this.nudCodeFontSize.Name = "nudCodeFontSize";
            this.nudCodeFontSize.Size = new System.Drawing.Size(47, 20);
            this.nudCodeFontSize.TabIndex = 26;
            this.nudCodeFontSize.Value = new decimal(new int[] {
            825,
            0,
            0,
            131072});
            this.nudCodeFontSize.ValueChanged += new System.EventHandler(this.nudCodeFontSize_ValueChanged);
            // 
            // lblFontSizeCode
            // 
            this.lblFontSizeCode.AutoSize = true;
            this.lblFontSizeCode.Location = new System.Drawing.Point(6, 113);
            this.lblFontSizeCode.Name = "lblFontSizeCode";
            this.lblFontSizeCode.Size = new System.Drawing.Size(85, 13);
            this.lblFontSizeCode.TabIndex = 25;
            this.lblFontSizeCode.Text = "Font Size(Code):";
            // 
            // lblFontSizeLog
            // 
            this.lblFontSizeLog.AutoSize = true;
            this.lblFontSizeLog.Location = new System.Drawing.Point(6, 90);
            this.lblFontSizeLog.Name = "lblFontSizeLog";
            this.lblFontSizeLog.Size = new System.Drawing.Size(78, 13);
            this.lblFontSizeLog.TabIndex = 24;
            this.lblFontSizeLog.Text = "Font Size(Log):";
            // 
            // btnSaveThemeAsDefault
            // 
            this.btnSaveThemeAsDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveThemeAsDefault.BackColor = System.Drawing.Color.DimGray;
            this.btnSaveThemeAsDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveThemeAsDefault.Location = new System.Drawing.Point(0, 202);
            this.btnSaveThemeAsDefault.Name = "btnSaveThemeAsDefault";
            this.btnSaveThemeAsDefault.Size = new System.Drawing.Size(343, 23);
            this.btnSaveThemeAsDefault.TabIndex = 23;
            this.btnSaveThemeAsDefault.Text = "Save as Default";
            this.btnSaveThemeAsDefault.UseVisualStyleBackColor = false;
            this.btnSaveThemeAsDefault.Click += new System.EventHandler(this.btnSaveThemeAsDefault_Click);
            // 
            // btnSaveTheme
            // 
            this.btnSaveTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveTheme.BackColor = System.Drawing.Color.DimGray;
            this.btnSaveTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveTheme.Location = new System.Drawing.Point(0, 174);
            this.btnSaveTheme.Name = "btnSaveTheme";
            this.btnSaveTheme.Size = new System.Drawing.Size(343, 23);
            this.btnSaveTheme.TabIndex = 22;
            this.btnSaveTheme.Text = "Save Theme";
            this.btnSaveTheme.UseVisualStyleBackColor = false;
            this.btnSaveTheme.Click += new System.EventHandler(this.btnSaveTheme_Click);
            // 
            // panelColorPreview
            // 
            this.panelColorPreview.Location = new System.Drawing.Point(0, 41);
            this.panelColorPreview.Name = "panelColorPreview";
            this.panelColorPreview.Size = new System.Drawing.Size(343, 13);
            this.panelColorPreview.TabIndex = 21;
            // 
            // cbEditorColorSetting
            // 
            this.cbEditorColorSetting.BackColor = System.Drawing.Color.DimGray;
            this.cbEditorColorSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEditorColorSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbEditorColorSetting.FormattingEnabled = true;
            this.cbEditorColorSetting.Location = new System.Drawing.Point(0, 19);
            this.cbEditorColorSetting.Name = "cbEditorColorSetting";
            this.cbEditorColorSetting.Size = new System.Drawing.Size(343, 21);
            this.cbEditorColorSetting.TabIndex = 20;
            this.cbEditorColorSetting.SelectedIndexChanged += new System.EventHandler(this.cbEditorColorSetting_SelectedIndexChanged);
            // 
            // btnChangeColor
            // 
            this.btnChangeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChangeColor.BackColor = System.Drawing.Color.DimGray;
            this.btnChangeColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeColor.Location = new System.Drawing.Point(0, 59);
            this.btnChangeColor.Name = "btnChangeColor";
            this.btnChangeColor.Size = new System.Drawing.Size(343, 23);
            this.btnChangeColor.TabIndex = 19;
            this.btnChangeColor.Text = "Change Color";
            this.btnChangeColor.UseVisualStyleBackColor = false;
            this.btnChangeColor.Click += new System.EventHandler(this.btnChangeColor_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAbout.BackColor = System.Drawing.Color.DimGray;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Location = new System.Drawing.Point(0, 292);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(343, 23);
            this.btnAbout.TabIndex = 17;
            this.btnAbout.Text = "Version Info";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnSetHomeDir
            // 
            this.btnSetHomeDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetHomeDir.BackColor = System.Drawing.Color.DimGray;
            this.btnSetHomeDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetHomeDir.Location = new System.Drawing.Point(0, 263);
            this.btnSetHomeDir.Name = "btnSetHomeDir";
            this.btnSetHomeDir.Size = new System.Drawing.Size(343, 23);
            this.btnSetHomeDir.TabIndex = 15;
            this.btnSetHomeDir.Text = "Set Home Directory";
            this.btnSetHomeDir.UseVisualStyleBackColor = false;
            this.btnSetHomeDir.Click += new System.EventHandler(this.btnSetHomeDir_Click);
            // 
            // btnUnpackKernels
            // 
            this.btnUnpackKernels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUnpackKernels.BackColor = System.Drawing.Color.DimGray;
            this.btnUnpackKernels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnpackKernels.ForeColor = System.Drawing.Color.Maroon;
            this.btnUnpackKernels.Location = new System.Drawing.Point(0, 350);
            this.btnUnpackKernels.Name = "btnUnpackKernels";
            this.btnUnpackKernels.Size = new System.Drawing.Size(343, 23);
            this.btnUnpackKernels.TabIndex = 14;
            this.btnUnpackKernels.Text = "Unpack CL Kernels";
            this.btnUnpackKernels.UseVisualStyleBackColor = false;
            this.btnUnpackKernels.Click += new System.EventHandler(this.btnUnpackKernels_Click);
            // 
            // btnReloadKernels
            // 
            this.btnReloadKernels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReloadKernels.BackColor = System.Drawing.Color.DimGray;
            this.btnReloadKernels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReloadKernels.ForeColor = System.Drawing.Color.Maroon;
            this.btnReloadKernels.Location = new System.Drawing.Point(0, 321);
            this.btnReloadKernels.Name = "btnReloadKernels";
            this.btnReloadKernels.Size = new System.Drawing.Size(343, 23);
            this.btnReloadKernels.TabIndex = 13;
            this.btnReloadKernels.Text = "Reload CL Kernels";
            this.btnReloadKernels.UseVisualStyleBackColor = false;
            this.btnReloadKernels.Click += new System.EventHandler(this.btnReloadKernels_Click);
            // 
            // sfdTheme
            // 
            this.sfdTheme.DefaultExt = "xml";
            this.sfdTheme.Title = "Save the Theme";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 462);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.gbResolution);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsDialog";
            this.Text = "SettingsDialog";
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.gbResolution.ResumeLayout(false);
            this.gbResolution.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.gbEditor.ResumeLayout(false);
            this.gbEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCodeFontSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.GroupBox gbResolution;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnUnpackKernels;
        private System.Windows.Forms.Button btnReloadKernels;
        private System.Windows.Forms.Button btnSetHomeDir;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.GroupBox gbEditor;
        private System.Windows.Forms.Panel panelColorPreview;
        private System.Windows.Forms.ComboBox cbEditorColorSetting;
        private System.Windows.Forms.Button btnChangeColor;
        private System.Windows.Forms.ColorDialog cdChangeThemeColor;
        private System.Windows.Forms.Button btnSaveThemeAsDefault;
        private System.Windows.Forms.Button btnSaveTheme;
        private System.Windows.Forms.SaveFileDialog sfdTheme;
        private System.Windows.Forms.NumericUpDown nudLogFontSize;
        private System.Windows.Forms.NumericUpDown nudCodeFontSize;
        private System.Windows.Forms.Label lblFontSizeCode;
        private System.Windows.Forms.Label lblFontSizeLog;
        private System.Windows.Forms.Button btnLoadTheme;
        private System.Windows.Forms.ComboBox cbThemes;
        private System.Windows.Forms.CheckBox cbLogPreviewStacktrace;
        private System.Windows.Forms.CheckBox cbLogParserStacktrace;
        private System.Windows.Forms.Label lblDepth;
        private System.Windows.Forms.NumericUpDown nudDepth;
    }
}