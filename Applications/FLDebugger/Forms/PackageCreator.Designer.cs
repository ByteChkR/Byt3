namespace FLDebugger.Forms
{
    partial class PackageCreator
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
            this.btnCreatePackage = new System.Windows.Forms.Button();
            this.lblPackageName = new System.Windows.Forms.Label();
            this.tbPackageName = new System.Windows.Forms.TextBox();
            this.tbUnpackConfig = new System.Windows.Forms.TextBox();
            this.lblUnpackConfig = new System.Windows.Forms.Label();
            this.sfdSavePackage = new System.Windows.Forms.SaveFileDialog();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.cbExport = new System.Windows.Forms.CheckBox();
            this.cbKeepFlScripts = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCreatePackage
            // 
            this.btnCreatePackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatePackage.BackColor = System.Drawing.Color.DimGray;
            this.btnCreatePackage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreatePackage.Location = new System.Drawing.Point(506, 61);
            this.btnCreatePackage.Name = "btnCreatePackage";
            this.btnCreatePackage.Size = new System.Drawing.Size(75, 23);
            this.btnCreatePackage.TabIndex = 0;
            this.btnCreatePackage.Text = "Create";
            this.btnCreatePackage.UseVisualStyleBackColor = false;
            this.btnCreatePackage.Click += new System.EventHandler(this.btnCreatePackage_Click);
            // 
            // lblPackageName
            // 
            this.lblPackageName.AutoSize = true;
            this.lblPackageName.Location = new System.Drawing.Point(13, 13);
            this.lblPackageName.Name = "lblPackageName";
            this.lblPackageName.Size = new System.Drawing.Size(84, 13);
            this.lblPackageName.TabIndex = 1;
            this.lblPackageName.Text = "Package Name:";
            // 
            // tbPackageName
            // 
            this.tbPackageName.BackColor = System.Drawing.Color.DimGray;
            this.tbPackageName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPackageName.Location = new System.Drawing.Point(103, 10);
            this.tbPackageName.Name = "tbPackageName";
            this.tbPackageName.Size = new System.Drawing.Size(478, 20);
            this.tbPackageName.TabIndex = 2;
            this.tbPackageName.Text = "Package";
            // 
            // tbUnpackConfig
            // 
            this.tbUnpackConfig.BackColor = System.Drawing.Color.DimGray;
            this.tbUnpackConfig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbUnpackConfig.Location = new System.Drawing.Point(103, 35);
            this.tbUnpackConfig.Name = "tbUnpackConfig";
            this.tbUnpackConfig.Size = new System.Drawing.Size(478, 20);
            this.tbUnpackConfig.TabIndex = 4;
            this.tbUnpackConfig.Text = "default;flc2tex|.flc";
            // 
            // lblUnpackConfig
            // 
            this.lblUnpackConfig.AutoSize = true;
            this.lblUnpackConfig.Location = new System.Drawing.Point(13, 38);
            this.lblUnpackConfig.Name = "lblUnpackConfig";
            this.lblUnpackConfig.Size = new System.Drawing.Size(81, 13);
            this.lblUnpackConfig.TabIndex = 3;
            this.lblUnpackConfig.Text = "Unpack Config:";
            // 
            // sfdSavePackage
            // 
            this.sfdSavePackage.DefaultExt = "flres";
            this.sfdSavePackage.FileName = "package.flres";
            this.sfdSavePackage.Filter = "FL Resource File|*.flres";
            // 
            // panelProgress
            // 
            this.panelProgress.Location = new System.Drawing.Point(13, 98);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(561, 282);
            this.panelProgress.TabIndex = 5;
            // 
            // cbExport
            // 
            this.cbExport.AutoSize = true;
            this.cbExport.Location = new System.Drawing.Point(17, 65);
            this.cbExport.Name = "cbExport";
            this.cbExport.Size = new System.Drawing.Size(106, 17);
            this.cbExport.TabIndex = 6;
            this.cbExport.Text = "Export FL Scripts";
            this.cbExport.UseVisualStyleBackColor = true;
            this.cbExport.CheckedChanged += new System.EventHandler(this.cbExport_CheckedChanged);
            // 
            // cbKeepFlScripts
            // 
            this.cbKeepFlScripts.AutoSize = true;
            this.cbKeepFlScripts.Checked = true;
            this.cbKeepFlScripts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKeepFlScripts.Enabled = false;
            this.cbKeepFlScripts.Location = new System.Drawing.Point(129, 65);
            this.cbKeepFlScripts.Name = "cbKeepFlScripts";
            this.cbKeepFlScripts.Size = new System.Drawing.Size(103, 17);
            this.cbKeepFlScripts.TabIndex = 7;
            this.cbKeepFlScripts.Text = "Keep FL Source";
            this.cbKeepFlScripts.UseVisualStyleBackColor = true;
            // 
            // PackageCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(586, 392);
            this.Controls.Add(this.cbKeepFlScripts);
            this.Controls.Add(this.cbExport);
            this.Controls.Add(this.panelProgress);
            this.Controls.Add(this.tbUnpackConfig);
            this.Controls.Add(this.lblUnpackConfig);
            this.Controls.Add(this.tbPackageName);
            this.Controls.Add(this.lblPackageName);
            this.Controls.Add(this.btnCreatePackage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PackageCreator";
            this.Text = "PackageCreator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreatePackage;
        private System.Windows.Forms.Label lblPackageName;
        private System.Windows.Forms.TextBox tbPackageName;
        private System.Windows.Forms.TextBox tbUnpackConfig;
        private System.Windows.Forms.Label lblUnpackConfig;
        private System.Windows.Forms.SaveFileDialog sfdSavePackage;
        private System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.CheckBox cbExport;
        private System.Windows.Forms.CheckBox cbKeepFlScripts;
    }
}