namespace FLDebugger.Projects.Forms
{
    partial class StartupDialog
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
            this.gbLastProjects = new System.Windows.Forms.GroupBox();
            this.flpLastProjects = new System.Windows.Forms.FlowLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnOpen = new System.Windows.Forms.Button();
            this.fbdProjectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOtherVersions = new System.Windows.Forms.Button();
            this.gbLastProjects.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLastProjects
            // 
            this.gbLastProjects.Controls.Add(this.flpLastProjects);
            this.gbLastProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLastProjects.Location = new System.Drawing.Point(0, 31);
            this.gbLastProjects.Name = "gbLastProjects";
            this.gbLastProjects.Size = new System.Drawing.Size(482, 444);
            this.gbLastProjects.TabIndex = 0;
            this.gbLastProjects.TabStop = false;
            this.gbLastProjects.Text = "Last Projects:";
            // 
            // flpLastProjects
            // 
            this.flpLastProjects.AutoScroll = true;
            this.flpLastProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpLastProjects.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpLastProjects.Location = new System.Drawing.Point(3, 16);
            this.flpLastProjects.Name = "flpLastProjects";
            this.flpLastProjects.Size = new System.Drawing.Size(476, 425);
            this.flpLastProjects.TabIndex = 0;
            this.flpLastProjects.WrapContents = false;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnOtherVersions);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(482, 23);
            this.panelHeader.TabIndex = 1;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.gbLastProjects);
            this.panelMain.Controls.Add(this.panelButtons);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 23);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(482, 475);
            this.panelMain.TabIndex = 2;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnOpen);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(482, 31);
            this.panelButtons.TabIndex = 1;
            // 
            // btnOpen
            // 
            this.btnOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Location = new System.Drawing.Point(0, 0);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(482, 31);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // fbdProjectFolder
            // 
            this.fbdProjectFolder.Description = "Select Project Directory";
            // 
            // btnOtherVersions
            // 
            this.btnOtherVersions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOtherVersions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOtherVersions.Location = new System.Drawing.Point(0, 0);
            this.btnOtherVersions.Name = "btnOtherVersions";
            this.btnOtherVersions.Size = new System.Drawing.Size(482, 23);
            this.btnOtherVersions.TabIndex = 1;
            this.btnOtherVersions.Text = "Check out other Versions";
            this.btnOtherVersions.UseVisualStyleBackColor = true;
            this.btnOtherVersions.Click += new System.EventHandler(this.btnOtherVersions_Click);
            // 
            // StartupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(482, 498);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "StartupDialog";
            this.Text = "StartupDialog";
            this.gbLastProjects.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLastProjects;
        private System.Windows.Forms.FlowLayoutPanel flpLastProjects;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.FolderBrowserDialog fbdProjectFolder;
        private System.Windows.Forms.Button btnOtherVersions;
    }
}