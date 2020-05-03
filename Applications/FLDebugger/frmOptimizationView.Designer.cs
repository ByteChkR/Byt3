namespace FLDebugger
{
    partial class frmOptimizationView
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
            this.panelInput = new System.Windows.Forms.Panel();
            this.rtbIn = new System.Windows.Forms.RichTextBox();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSwitchDockSide = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnSetHomeDir = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lbOptimizations = new System.Windows.Forms.CheckedListBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.openFLScript = new System.Windows.Forms.OpenFileDialog();
            this.rtbOut = new System.Windows.Forms.RichTextBox();
            this.panelConsoleOut = new System.Windows.Forms.Panel();
            this.rtbLogOut = new System.Windows.Forms.RichTextBox();
            this.panelCodeArea = new System.Windows.Forms.Panel();
            this.panelOutput = new System.Windows.Forms.Panel();
            this.tmrConsoleRefresh = new System.Windows.Forms.Timer(this.components);
            this.tmrConsoleColors = new System.Windows.Forms.Timer(this.components);
            this.fbdSelectHomeDir = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.sfdScript = new System.Windows.Forms.SaveFileDialog();
            this.lblFLVersion = new System.Windows.Forms.Label();
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
            this.panelInput.Controls.Add(this.rtbIn);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelInput.Location = new System.Drawing.Point(0, 0);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(334, 340);
            this.panelInput.TabIndex = 0;
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
            this.rtbIn.Size = new System.Drawing.Size(334, 340);
            this.rtbIn.TabIndex = 0;
            this.rtbIn.Text = "";
            this.rtbIn.TextChanged += new System.EventHandler(this.rtbIn_TextChanged);
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.DimGray;
            this.panelToolbar.Controls.Add(this.lblFLVersion);
            this.panelToolbar.Controls.Add(this.btnSave);
            this.panelToolbar.Controls.Add(this.label1);
            this.panelToolbar.Controls.Add(this.btnSwitchDockSide);
            this.panelToolbar.Controls.Add(this.btnMinimize);
            this.panelToolbar.Controls.Add(this.btnSetHomeDir);
            this.panelToolbar.Controls.Add(this.btnDebug);
            this.panelToolbar.Controls.Add(this.btnUpdate);
            this.panelToolbar.Controls.Add(this.lbOptimizations);
            this.panelToolbar.Controls.Add(this.btnOpen);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(236, 516);
            this.panelToolbar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "[CTRL] + Click to change optimization order";
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
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.DimGray;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(0, 374);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(236, 23);
            this.btnMinimize.TabIndex = 6;
            this.btnMinimize.Text = "Minimize";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
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
            this.btnDebug.Location = new System.Drawing.Point(0, 287);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(236, 23);
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
            this.btnUpdate.Size = new System.Drawing.Size(236, 23);
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
            this.lbOptimizations.Location = new System.Drawing.Point(0, 70);
            this.lbOptimizations.Name = "lbOptimizations";
            this.lbOptimizations.Size = new System.Drawing.Size(236, 167);
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
            this.rtbOut.Size = new System.Drawing.Size(298, 340);
            this.rtbOut.TabIndex = 0;
            this.rtbOut.Text = "";
            // 
            // panelConsoleOut
            // 
            this.panelConsoleOut.BackColor = System.Drawing.Color.DimGray;
            this.panelConsoleOut.Controls.Add(this.rtbLogOut);
            this.panelConsoleOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConsoleOut.Location = new System.Drawing.Point(236, 340);
            this.panelConsoleOut.Name = "panelConsoleOut";
            this.panelConsoleOut.Size = new System.Drawing.Size(632, 176);
            this.panelConsoleOut.TabIndex = 1;
            // 
            // rtbLogOut
            // 
            this.rtbLogOut.BackColor = System.Drawing.Color.DimGray;
            this.rtbLogOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLogOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLogOut.ForeColor = System.Drawing.Color.Maroon;
            this.rtbLogOut.Location = new System.Drawing.Point(0, 0);
            this.rtbLogOut.Name = "rtbLogOut";
            this.rtbLogOut.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbLogOut.Size = new System.Drawing.Size(632, 176);
            this.rtbLogOut.TabIndex = 0;
            this.rtbLogOut.Text = "";
            // 
            // panelCodeArea
            // 
            this.panelCodeArea.BackColor = System.Drawing.Color.DimGray;
            this.panelCodeArea.Controls.Add(this.panelOutput);
            this.panelCodeArea.Controls.Add(this.panelInput);
            this.panelCodeArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCodeArea.Location = new System.Drawing.Point(236, 0);
            this.panelCodeArea.Name = "panelCodeArea";
            this.panelCodeArea.Size = new System.Drawing.Size(632, 340);
            this.panelCodeArea.TabIndex = 2;
            // 
            // panelOutput
            // 
            this.panelOutput.BackColor = System.Drawing.Color.DimGray;
            this.panelOutput.Controls.Add(this.rtbOut);
            this.panelOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOutput.Location = new System.Drawing.Point(334, 0);
            this.panelOutput.Name = "panelOutput";
            this.panelOutput.Size = new System.Drawing.Size(298, 340);
            this.panelOutput.TabIndex = 1;
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
            // sfdScript
            // 
            this.sfdScript.DefaultExt = "fl";
            this.sfdScript.FileName = "script";
            this.sfdScript.Filter = "FLScript|*.fl";
            this.sfdScript.Title = "Save FL Script";
            // 
            // lblFLVersion
            // 
            this.lblFLVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFLVersion.AutoSize = true;
            this.lblFLVersion.Location = new System.Drawing.Point(12, 468);
            this.lblFLVersion.Name = "lblFLVersion";
            this.lblFLVersion.Size = new System.Drawing.Size(91, 39);
            this.lblFLVersion.TabIndex = 10;
            this.lblFLVersion.Text = "OpenFL Versions:\r\nParser:\r\nCommon:";
            // 
            // frmOptimizationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 516);
            this.Controls.Add(this.panelCodeArea);
            this.Controls.Add(this.panelConsoleOut);
            this.Controls.Add(this.panelToolbar);
            this.MinimumSize = new System.Drawing.Size(252, 490);
            this.Name = "frmOptimizationView";
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
        private System.Windows.Forms.RichTextBox rtbLogOut;
        private System.Windows.Forms.Timer tmrConsoleRefresh;
        private System.Windows.Forms.CheckedListBox lbOptimizations;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Timer tmrConsoleColors;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnSetHomeDir;
        private System.Windows.Forms.FolderBrowserDialog fbdSelectHomeDir;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnSwitchDockSide;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog sfdScript;
        private System.Windows.Forms.Label lblFLVersion;
    }
}

