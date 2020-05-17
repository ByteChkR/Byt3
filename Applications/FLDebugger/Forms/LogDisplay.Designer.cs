namespace FLDebugger.Forms
{
    partial class LogDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogDisplay));
            this.rtbLogOut = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.panelSide = new System.Windows.Forms.Panel();
            this.panelSideMain = new System.Windows.Forms.Panel();
            this.lbLogger = new System.Windows.Forms.ListBox();
            this.panelSideTop = new System.Windows.Forms.Panel();
            this.cbMinSeverity = new System.Windows.Forms.ComboBox();
            this.cbProjects = new System.Windows.Forms.ComboBox();
            this.lblLogSeverity = new System.Windows.Forms.Label();
            this.btnRefreshProjects = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelSide.SuspendLayout();
            this.panelSideMain.SuspendLayout();
            this.panelSideTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbLogOut
            // 
            this.rtbLogOut.BackColor = System.Drawing.Color.DimGray;
            this.rtbLogOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLogOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLogOut.ForeColor = System.Drawing.Color.Maroon;
            this.rtbLogOut.Location = new System.Drawing.Point(0, 0);
            this.rtbLogOut.Name = "rtbLogOut";
            this.rtbLogOut.ReadOnly = true;
            this.rtbLogOut.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtbLogOut.Size = new System.Drawing.Size(597, 475);
            this.rtbLogOut.TabIndex = 1;
            this.rtbLogOut.Text = "";
            this.rtbLogOut.TextChanged += new System.EventHandler(this.rtbLogOut_TextChanged);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.DimGray;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.Maroon;
            this.btnClear.Location = new System.Drawing.Point(9, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(200, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panelSide
            // 
            this.panelSide.BackColor = System.Drawing.Color.DimGray;
            this.panelSide.Controls.Add(this.panelSideMain);
            this.panelSide.Controls.Add(this.panelSideTop);
            this.panelSide.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSide.Location = new System.Drawing.Point(597, 0);
            this.panelSide.Name = "panelSide";
            this.panelSide.Size = new System.Drawing.Size(220, 475);
            this.panelSide.TabIndex = 3;
            // 
            // panelSideMain
            // 
            this.panelSideMain.Controls.Add(this.lbLogger);
            this.panelSideMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSideMain.Location = new System.Drawing.Point(0, 119);
            this.panelSideMain.Name = "panelSideMain";
            this.panelSideMain.Size = new System.Drawing.Size(220, 356);
            this.panelSideMain.TabIndex = 9;
            // 
            // lbLogger
            // 
            this.lbLogger.BackColor = System.Drawing.Color.DimGray;
            this.lbLogger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLogger.FormattingEnabled = true;
            this.lbLogger.Location = new System.Drawing.Point(0, 0);
            this.lbLogger.Name = "lbLogger";
            this.lbLogger.Size = new System.Drawing.Size(220, 356);
            this.lbLogger.TabIndex = 4;
            // 
            // panelSideTop
            // 
            this.panelSideTop.Controls.Add(this.btnClear);
            this.panelSideTop.Controls.Add(this.cbMinSeverity);
            this.panelSideTop.Controls.Add(this.cbProjects);
            this.panelSideTop.Controls.Add(this.lblLogSeverity);
            this.panelSideTop.Controls.Add(this.btnRefreshProjects);
            this.panelSideTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSideTop.Location = new System.Drawing.Point(0, 0);
            this.panelSideTop.Name = "panelSideTop";
            this.panelSideTop.Size = new System.Drawing.Size(220, 119);
            this.panelSideTop.TabIndex = 8;
            // 
            // cbMinSeverity
            // 
            this.cbMinSeverity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMinSeverity.FormattingEnabled = true;
            this.cbMinSeverity.Location = new System.Drawing.Point(82, 88);
            this.cbMinSeverity.Name = "cbMinSeverity";
            this.cbMinSeverity.Size = new System.Drawing.Size(128, 21);
            this.cbMinSeverity.TabIndex = 7;
            this.cbMinSeverity.SelectedIndexChanged += new System.EventHandler(this.cbMinSeverity_SelectedIndexChanged);
            // 
            // cbProjects
            // 
            this.cbProjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProjects.BackColor = System.Drawing.Color.DimGray;
            this.cbProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProjects.FormattingEnabled = true;
            this.cbProjects.Location = new System.Drawing.Point(9, 61);
            this.cbProjects.Name = "cbProjects";
            this.cbProjects.Size = new System.Drawing.Size(201, 21);
            this.cbProjects.TabIndex = 3;
            this.cbProjects.SelectedIndexChanged += new System.EventHandler(this.cbProjects_SelectedIndexChanged);
            // 
            // lblLogSeverity
            // 
            this.lblLogSeverity.AutoSize = true;
            this.lblLogSeverity.Location = new System.Drawing.Point(8, 91);
            this.lblLogSeverity.Name = "lblLogSeverity";
            this.lblLogSeverity.Size = new System.Drawing.Size(68, 13);
            this.lblLogSeverity.TabIndex = 6;
            this.lblLogSeverity.Text = "Min Severity:";
            // 
            // btnRefreshProjects
            // 
            this.btnRefreshProjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshProjects.BackColor = System.Drawing.Color.DimGray;
            this.btnRefreshProjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshProjects.ForeColor = System.Drawing.Color.Maroon;
            this.btnRefreshProjects.Location = new System.Drawing.Point(9, 32);
            this.btnRefreshProjects.Name = "btnRefreshProjects";
            this.btnRefreshProjects.Size = new System.Drawing.Size(200, 23);
            this.btnRefreshProjects.TabIndex = 5;
            this.btnRefreshProjects.Text = "Refresh Projects";
            this.btnRefreshProjects.UseVisualStyleBackColor = false;
            this.btnRefreshProjects.Click += new System.EventHandler(this.btnRefreshProjects_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.rtbLogOut);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(597, 475);
            this.panelMain.TabIndex = 4;
            // 
            // LogDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 475);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelSide);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogDisplay";
            this.Text = "LogDisplay";
            this.Load += new System.EventHandler(this.LogDisplay_Load);
            this.panelSide.ResumeLayout(false);
            this.panelSideMain.ResumeLayout(false);
            this.panelSideTop.ResumeLayout(false);
            this.panelSideTop.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLogOut;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel panelSide;
        private System.Windows.Forms.ListBox lbLogger;
        private System.Windows.Forms.ComboBox cbProjects;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button btnRefreshProjects;
        private System.Windows.Forms.ComboBox cbMinSeverity;
        private System.Windows.Forms.Label lblLogSeverity;
        private System.Windows.Forms.Panel panelSideMain;
        private System.Windows.Forms.Panel panelSideTop;
    }
}