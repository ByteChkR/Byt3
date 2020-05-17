namespace FLDebugger.Forms
{
    partial class FLScriptEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLScriptEditor));
            this.panelInput = new System.Windows.Forms.Panel();
            this.btnPopOutInput = new System.Windows.Forms.Button();
            this.rtbIn = new System.Windows.Forms.RichTextBox();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnShowInstructions = new System.Windows.Forms.Button();
            this.cbAutoBuild = new System.Windows.Forms.CheckBox();
            this.cbLiveView = new System.Windows.Forms.CheckBox();
            this.btnRestartDebugger = new System.Windows.Forms.Button();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSwitchDockSide = new System.Windows.Forms.Button();
            this.btnSetHomeDir = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lbOptimizations = new System.Windows.Forms.CheckedListBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.openFLScript = new System.Windows.Forms.OpenFileDialog();
            this.rtbOut = new System.Windows.Forms.RichTextBox();
            this.panelConsoleOut = new System.Windows.Forms.Panel();
            this.btnPopOutBuildLog = new System.Windows.Forms.Button();
            this.rtbParserOutput = new System.Windows.Forms.RichTextBox();
            this.panelCodeArea = new System.Windows.Forms.Panel();
            this.panelOutput = new System.Windows.Forms.Panel();
            this.btnPopOutOutput = new System.Windows.Forms.Button();
            this.tmrConsoleRefresh = new System.Windows.Forms.Timer(this.components);
            this.tmrConsoleColors = new System.Windows.Forms.Timer(this.components);
            this.fbdSelectHomeDir = new System.Windows.Forms.FolderBrowserDialog();
            this.sfdScript = new System.Windows.Forms.SaveFileDialog();
            this.cbBuildMode = new System.Windows.Forms.ComboBox();
            this.lblBuildMode = new System.Windows.Forms.Label();
            this.panelInput.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.panelConsoleOut.SuspendLayout();
            this.panelCodeArea.SuspendLayout();
            this.panelOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInput
            // 
            this.panelInput.BackColor = System.Drawing.Color.DimGray;
            this.panelInput.Controls.Add(this.btnPopOutInput);
            this.panelInput.Controls.Add(this.rtbIn);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelInput.Location = new System.Drawing.Point(0, 0);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(334, 315);
            this.panelInput.TabIndex = 0;
            // 
            // btnPopOutInput
            // 
            this.btnPopOutInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPopOutInput.BackColor = System.Drawing.Color.DimGray;
            this.btnPopOutInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopOutInput.Location = new System.Drawing.Point(247, 289);
            this.btnPopOutInput.Name = "btnPopOutInput";
            this.btnPopOutInput.Size = new System.Drawing.Size(84, 23);
            this.btnPopOutInput.TabIndex = 14;
            this.btnPopOutInput.Text = "Pop Out";
            this.btnPopOutInput.UseVisualStyleBackColor = false;
            this.btnPopOutInput.Click += new System.EventHandler(this.btnPopOutInput_Click);
            // 
            // rtbIn
            // 
            this.rtbIn.AcceptsTab = true;
            this.rtbIn.BackColor = System.Drawing.Color.DimGray;
            this.rtbIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbIn.ForeColor = System.Drawing.Color.Black;
            this.rtbIn.Location = new System.Drawing.Point(0, 0);
            this.rtbIn.Name = "rtbIn";
            this.rtbIn.Size = new System.Drawing.Size(334, 315);
            this.rtbIn.TabIndex = 0;
            this.rtbIn.Text = "";
            this.rtbIn.TextChanged += new System.EventHandler(this.rtbIn_TextChanged);
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.DimGray;
            this.panelToolbar.Controls.Add(this.lblBuildMode);
            this.panelToolbar.Controls.Add(this.cbBuildMode);
            this.panelToolbar.Controls.Add(this.btnAbout);
            this.panelToolbar.Controls.Add(this.btnShowInstructions);
            this.panelToolbar.Controls.Add(this.cbAutoBuild);
            this.panelToolbar.Controls.Add(this.cbLiveView);
            this.panelToolbar.Controls.Add(this.btnRestartDebugger);
            this.panelToolbar.Controls.Add(this.btnShowLog);
            this.panelToolbar.Controls.Add(this.btnSave);
            this.panelToolbar.Controls.Add(this.btnSwitchDockSide);
            this.panelToolbar.Controls.Add(this.btnSetHomeDir);
            this.panelToolbar.Controls.Add(this.btnDebug);
            this.panelToolbar.Controls.Add(this.btnUpdate);
            this.panelToolbar.Controls.Add(this.lbOptimizations);
            this.panelToolbar.Controls.Add(this.btnOpen);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(236, 491);
            this.panelToolbar.TabIndex = 0;
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.DimGray;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Location = new System.Drawing.Point(0, 432);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(236, 23);
            this.btnAbout.TabIndex = 16;
            this.btnAbout.Text = "Version Info";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnShowInstructions
            // 
            this.btnShowInstructions.BackColor = System.Drawing.Color.DimGray;
            this.btnShowInstructions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowInstructions.Location = new System.Drawing.Point(0, 287);
            this.btnShowInstructions.Name = "btnShowInstructions";
            this.btnShowInstructions.Size = new System.Drawing.Size(236, 23);
            this.btnShowInstructions.TabIndex = 15;
            this.btnShowInstructions.Text = "Show Instructions";
            this.btnShowInstructions.UseVisualStyleBackColor = false;
            this.btnShowInstructions.Click += new System.EventHandler(this.btnShowInstructions_Click);
            // 
            // cbAutoBuild
            // 
            this.cbAutoBuild.AutoSize = true;
            this.cbAutoBuild.Location = new System.Drawing.Point(42, 465);
            this.cbAutoBuild.Name = "cbAutoBuild";
            this.cbAutoBuild.Size = new System.Drawing.Size(74, 17);
            this.cbAutoBuild.TabIndex = 14;
            this.cbAutoBuild.Text = "Auto Build";
            this.cbAutoBuild.UseVisualStyleBackColor = true;
            // 
            // cbLiveView
            // 
            this.cbLiveView.AutoSize = true;
            this.cbLiveView.Location = new System.Drawing.Point(122, 465);
            this.cbLiveView.Name = "cbLiveView";
            this.cbLiveView.Size = new System.Drawing.Size(64, 17);
            this.cbLiveView.TabIndex = 13;
            this.cbLiveView.Text = "Preview";
            this.cbLiveView.UseVisualStyleBackColor = true;
            this.cbLiveView.CheckedChanged += new System.EventHandler(this.cbLiveView_CheckedChanged);
            // 
            // btnRestartDebugger
            // 
            this.btnRestartDebugger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestartDebugger.BackColor = System.Drawing.Color.DimGray;
            this.btnRestartDebugger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestartDebugger.ForeColor = System.Drawing.Color.Maroon;
            this.btnRestartDebugger.Location = new System.Drawing.Point(0, 403);
            this.btnRestartDebugger.Name = "btnRestartDebugger";
            this.btnRestartDebugger.Size = new System.Drawing.Size(236, 23);
            this.btnRestartDebugger.TabIndex = 12;
            this.btnRestartDebugger.Text = "[EXPERIMENTAL]Restart Debugger";
            this.btnRestartDebugger.UseVisualStyleBackColor = false;
            this.btnRestartDebugger.Click += new System.EventHandler(this.btnRestartDebugger_Click);
            // 
            // btnShowLog
            // 
            this.btnShowLog.BackColor = System.Drawing.Color.DimGray;
            this.btnShowLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowLog.Location = new System.Drawing.Point(0, 374);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(236, 23);
            this.btnShowLog.TabIndex = 11;
            this.btnShowLog.Text = "Show Log";
            this.btnShowLog.UseVisualStyleBackColor = false;
            this.btnShowLog.Click += new System.EventHandler(this.btnShowLog_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.DimGray;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(0, 41);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(236, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSwitchDockSide
            // 
            this.btnSwitchDockSide.BackColor = System.Drawing.Color.DimGray;
            this.btnSwitchDockSide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchDockSide.Location = new System.Drawing.Point(0, 345);
            this.btnSwitchDockSide.Name = "btnSwitchDockSide";
            this.btnSwitchDockSide.Size = new System.Drawing.Size(236, 23);
            this.btnSwitchDockSide.TabIndex = 7;
            this.btnSwitchDockSide.Text = "Move to Right";
            this.btnSwitchDockSide.UseVisualStyleBackColor = false;
            this.btnSwitchDockSide.Click += new System.EventHandler(this.btnSwitchDockSide_Click);
            // 
            // btnSetHomeDir
            // 
            this.btnSetHomeDir.BackColor = System.Drawing.Color.DimGray;
            this.btnSetHomeDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetHomeDir.Location = new System.Drawing.Point(0, 316);
            this.btnSetHomeDir.Name = "btnSetHomeDir";
            this.btnSetHomeDir.Size = new System.Drawing.Size(236, 23);
            this.btnSetHomeDir.TabIndex = 4;
            this.btnSetHomeDir.Text = "Set Home Directory";
            this.btnSetHomeDir.UseVisualStyleBackColor = false;
            this.btnSetHomeDir.Click += new System.EventHandler(this.btnSetHomeDir_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.BackColor = System.Drawing.Color.DimGray;
            this.btnDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebug.Location = new System.Drawing.Point(120, 258);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(116, 23);
            this.btnDebug.TabIndex = 3;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = false;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.DimGray;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Location = new System.Drawing.Point(0, 258);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(116, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Parse";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lbOptimizations
            // 
            this.lbOptimizations.BackColor = System.Drawing.Color.DimGray;
            this.lbOptimizations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbOptimizations.CheckOnClick = true;
            this.lbOptimizations.FormattingEnabled = true;
            this.lbOptimizations.Location = new System.Drawing.Point(0, 100);
            this.lbOptimizations.Name = "lbOptimizations";
            this.lbOptimizations.Size = new System.Drawing.Size(236, 152);
            this.lbOptimizations.TabIndex = 1;
            this.lbOptimizations.SelectedIndexChanged += new System.EventHandler(this.lbOptimizations_SelectedIndexChanged);
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.DimGray;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Location = new System.Drawing.Point(0, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(236, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // openFLScript
            // 
            this.openFLScript.FileName = "openFileDialog1";
            // 
            // rtbOut
            // 
            this.rtbOut.BackColor = System.Drawing.Color.DimGray;
            this.rtbOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOut.ForeColor = System.Drawing.Color.Black;
            this.rtbOut.Location = new System.Drawing.Point(0, 0);
            this.rtbOut.Name = "rtbOut";
            this.rtbOut.ReadOnly = true;
            this.rtbOut.Size = new System.Drawing.Size(292, 315);
            this.rtbOut.TabIndex = 0;
            this.rtbOut.Text = "";
            // 
            // panelConsoleOut
            // 
            this.panelConsoleOut.BackColor = System.Drawing.Color.DimGray;
            this.panelConsoleOut.Controls.Add(this.btnPopOutBuildLog);
            this.panelConsoleOut.Controls.Add(this.rtbParserOutput);
            this.panelConsoleOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConsoleOut.Location = new System.Drawing.Point(236, 315);
            this.panelConsoleOut.Name = "panelConsoleOut";
            this.panelConsoleOut.Size = new System.Drawing.Size(626, 176);
            this.panelConsoleOut.TabIndex = 1;
            // 
            // btnPopOutBuildLog
            // 
            this.btnPopOutBuildLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPopOutBuildLog.BackColor = System.Drawing.Color.DimGray;
            this.btnPopOutBuildLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopOutBuildLog.Location = new System.Drawing.Point(539, 150);
            this.btnPopOutBuildLog.Name = "btnPopOutBuildLog";
            this.btnPopOutBuildLog.Size = new System.Drawing.Size(84, 23);
            this.btnPopOutBuildLog.TabIndex = 12;
            this.btnPopOutBuildLog.Text = "Pop Out";
            this.btnPopOutBuildLog.UseVisualStyleBackColor = false;
            this.btnPopOutBuildLog.Click += new System.EventHandler(this.btnPopOutBuildLog_Click);
            // 
            // rtbParserOutput
            // 
            this.rtbParserOutput.BackColor = System.Drawing.Color.DimGray;
            this.rtbParserOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbParserOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbParserOutput.ForeColor = System.Drawing.Color.Maroon;
            this.rtbParserOutput.Location = new System.Drawing.Point(0, 0);
            this.rtbParserOutput.Name = "rtbParserOutput";
            this.rtbParserOutput.ReadOnly = true;
            this.rtbParserOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbParserOutput.Size = new System.Drawing.Size(626, 176);
            this.rtbParserOutput.TabIndex = 0;
            this.rtbParserOutput.Text = "";
            // 
            // panelCodeArea
            // 
            this.panelCodeArea.BackColor = System.Drawing.Color.DimGray;
            this.panelCodeArea.Controls.Add(this.panelOutput);
            this.panelCodeArea.Controls.Add(this.panelInput);
            this.panelCodeArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCodeArea.Location = new System.Drawing.Point(236, 0);
            this.panelCodeArea.Name = "panelCodeArea";
            this.panelCodeArea.Size = new System.Drawing.Size(626, 315);
            this.panelCodeArea.TabIndex = 2;
            // 
            // panelOutput
            // 
            this.panelOutput.BackColor = System.Drawing.Color.DimGray;
            this.panelOutput.Controls.Add(this.btnPopOutOutput);
            this.panelOutput.Controls.Add(this.rtbOut);
            this.panelOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOutput.Location = new System.Drawing.Point(334, 0);
            this.panelOutput.Name = "panelOutput";
            this.panelOutput.Size = new System.Drawing.Size(292, 315);
            this.panelOutput.TabIndex = 1;
            // 
            // btnPopOutOutput
            // 
            this.btnPopOutOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPopOutOutput.BackColor = System.Drawing.Color.DimGray;
            this.btnPopOutOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopOutOutput.Location = new System.Drawing.Point(205, 289);
            this.btnPopOutOutput.Name = "btnPopOutOutput";
            this.btnPopOutOutput.Size = new System.Drawing.Size(84, 23);
            this.btnPopOutOutput.TabIndex = 13;
            this.btnPopOutOutput.Text = "Pop Out";
            this.btnPopOutOutput.UseVisualStyleBackColor = false;
            this.btnPopOutOutput.Click += new System.EventHandler(this.btnPopOutOutput_Click);
            // 
            // tmrConsoleRefresh
            // 
            this.tmrConsoleRefresh.Tick += new System.EventHandler(this.tmrConsoleRefresh_Tick);
            // 
            // tmrConsoleColors
            // 
            this.tmrConsoleColors.Interval = 1000;
            this.tmrConsoleColors.Tick += new System.EventHandler(this.tmrConsoleColors_Tick);
            // 
            // fbdSelectHomeDir
            // 
            this.fbdSelectHomeDir.Description = "Select the Root directory from where the FL scripts will be called(this will fix " +
    "filenames)";
            // 
            // sfdScript
            // 
            this.sfdScript.DefaultExt = "fl";
            this.sfdScript.FileName = "script";
            this.sfdScript.Filter = "FLScript|*.fl";
            this.sfdScript.Title = "Save FL Script";
            // 
            // cbBuildMode
            // 
            this.cbBuildMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuildMode.FormattingEnabled = true;
            this.cbBuildMode.Items.AddRange(new object[] {
            "Debug",
            "Release"});
            this.cbBuildMode.Location = new System.Drawing.Point(81, 70);
            this.cbBuildMode.Name = "cbBuildMode";
            this.cbBuildMode.Size = new System.Drawing.Size(149, 21);
            this.cbBuildMode.TabIndex = 17;
            this.cbBuildMode.SelectedIndexChanged += new System.EventHandler(this.cbBuildMode_SelectedIndexChanged);
            // 
            // lblBuildMode
            // 
            this.lblBuildMode.AutoSize = true;
            this.lblBuildMode.Location = new System.Drawing.Point(12, 73);
            this.lblBuildMode.Name = "lblBuildMode";
            this.lblBuildMode.Size = new System.Drawing.Size(63, 13);
            this.lblBuildMode.TabIndex = 18;
            this.lblBuildMode.Text = "Build Mode:";
            // 
            // FLScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 491);
            this.Controls.Add(this.panelCodeArea);
            this.Controls.Add(this.panelConsoleOut);
            this.Controls.Add(this.panelToolbar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(252, 530);
            this.Name = "FLScriptEditor";
            this.Text = "FL Parse Output Viewer";
            this.Load += new System.EventHandler(this.frmOptimizationView_Load);
            this.panelInput.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.panelToolbar.PerformLayout();
            this.panelConsoleOut.ResumeLayout(false);
            this.panelCodeArea.ResumeLayout(false);
            this.panelOutput.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog openFLScript;
        private System.Windows.Forms.RichTextBox rtbIn;
        private System.Windows.Forms.RichTextBox rtbOut;
        private System.Windows.Forms.Panel panelConsoleOut;
        private System.Windows.Forms.Panel panelCodeArea;
        private System.Windows.Forms.Panel panelOutput;
        private System.Windows.Forms.RichTextBox rtbParserOutput;
        private System.Windows.Forms.Timer tmrConsoleRefresh;
        private System.Windows.Forms.CheckedListBox lbOptimizations;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Timer tmrConsoleColors;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnSetHomeDir;
        private System.Windows.Forms.FolderBrowserDialog fbdSelectHomeDir;
        private System.Windows.Forms.Button btnSwitchDockSide;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog sfdScript;
        private System.Windows.Forms.Button btnShowLog;
        private System.Windows.Forms.Button btnPopOutBuildLog;
        private System.Windows.Forms.Button btnPopOutInput;
        private System.Windows.Forms.Button btnPopOutOutput;
        private System.Windows.Forms.Button btnRestartDebugger;
        private System.Windows.Forms.CheckBox cbLiveView;
        private System.Windows.Forms.CheckBox cbAutoBuild;
        private System.Windows.Forms.Button btnShowInstructions;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Label lblBuildMode;
        private System.Windows.Forms.ComboBox cbBuildMode;
    }
}

