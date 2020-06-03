namespace FLDebugger.Forms
{
    partial class AboutInfo
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.llblFLProjects = new System.Windows.Forms.LinkLabel();
            this.llblVersionArchive = new System.Windows.Forms.LinkLabel();
            this.lblVersionInfo = new System.Windows.Forms.Label();
            this.gbLoadedAssemblies = new System.Windows.Forms.GroupBox();
            this.lbLoadedAssemblies = new System.Windows.Forms.ListBox();
            this.lblGithub = new System.Windows.Forms.LinkLabel();
            this.panelTop.SuspendLayout();
            this.gbLoadedAssemblies.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblGithub);
            this.panelTop.Controls.Add(this.llblFLProjects);
            this.panelTop.Controls.Add(this.llblVersionArchive);
            this.panelTop.Controls.Add(this.lblVersionInfo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(336, 81);
            this.panelTop.TabIndex = 13;
            // 
            // llblFLProjects
            // 
            this.llblFLProjects.AutoSize = true;
            this.llblFLProjects.Location = new System.Drawing.Point(3, 43);
            this.llblFLProjects.Name = "llblFLProjects";
            this.llblFLProjects.Size = new System.Drawing.Size(159, 13);
            this.llblFLProjects.TabIndex = 2;
            this.llblFLProjects.TabStop = true;
            this.llblFLProjects.Text = "Check out FLDebugger.Projects";
            this.llblFLProjects.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblFLProjects_LinkClicked);
            // 
            // llblVersionArchive
            // 
            this.llblVersionArchive.AutoSize = true;
            this.llblVersionArchive.Location = new System.Drawing.Point(3, 26);
            this.llblVersionArchive.Name = "llblVersionArchive";
            this.llblVersionArchive.Size = new System.Drawing.Size(81, 13);
            this.llblVersionArchive.TabIndex = 1;
            this.llblVersionArchive.TabStop = true;
            this.llblVersionArchive.Text = "Version Archive";
            this.llblVersionArchive.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblVersionArchive_LinkClicked);
            // 
            // lblVersionInfo
            // 
            this.lblVersionInfo.AutoSize = true;
            this.lblVersionInfo.Location = new System.Drawing.Point(3, 9);
            this.lblVersionInfo.Name = "lblVersionInfo";
            this.lblVersionInfo.Size = new System.Drawing.Size(95, 13);
            this.lblVersionInfo.TabIndex = 0;
            this.lblVersionInfo.Text = "Debugger Version:";
            // 
            // gbLoadedAssemblies
            // 
            this.gbLoadedAssemblies.BackColor = System.Drawing.Color.DimGray;
            this.gbLoadedAssemblies.Controls.Add(this.lbLoadedAssemblies);
            this.gbLoadedAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLoadedAssemblies.Location = new System.Drawing.Point(0, 81);
            this.gbLoadedAssemblies.Name = "gbLoadedAssemblies";
            this.gbLoadedAssemblies.Size = new System.Drawing.Size(336, 197);
            this.gbLoadedAssemblies.TabIndex = 14;
            this.gbLoadedAssemblies.TabStop = false;
            this.gbLoadedAssemblies.Text = "Loaded Assemblies";
            // 
            // lbLoadedAssemblies
            // 
            this.lbLoadedAssemblies.BackColor = System.Drawing.Color.DimGray;
            this.lbLoadedAssemblies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbLoadedAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLoadedAssemblies.FormattingEnabled = true;
            this.lbLoadedAssemblies.Location = new System.Drawing.Point(3, 16);
            this.lbLoadedAssemblies.Name = "lbLoadedAssemblies";
            this.lbLoadedAssemblies.Size = new System.Drawing.Size(330, 178);
            this.lbLoadedAssemblies.TabIndex = 0;
            // 
            // lblGithub
            // 
            this.lblGithub.AutoSize = true;
            this.lblGithub.Location = new System.Drawing.Point(3, 61);
            this.lblGithub.Name = "lblGithub";
            this.lblGithub.Size = new System.Drawing.Size(90, 13);
            this.lblGithub.TabIndex = 3;
            this.lblGithub.TabStop = true;
            this.lblGithub.Text = "Source on Github";
            this.lblGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblGithub_LinkClicked);
            // 
            // AboutInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(336, 278);
            this.Controls.Add(this.gbLoadedAssemblies);
            this.Controls.Add(this.panelTop);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutInfo";
            this.Text = "Version Info";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.gbLoadedAssemblies.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblVersionInfo;
        private System.Windows.Forms.GroupBox gbLoadedAssemblies;
        private System.Windows.Forms.ListBox lbLoadedAssemblies;
        private System.Windows.Forms.LinkLabel llblFLProjects;
        private System.Windows.Forms.LinkLabel llblVersionArchive;
        private System.Windows.Forms.LinkLabel lblGithub;
    }
}