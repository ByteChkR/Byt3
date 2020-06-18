namespace ProgramInstaller
{
    partial class SelectProductForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectProductForm));
            this.cbProduct = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cbVersion = new System.Windows.Forms.ComboBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbInstallDir = new System.Windows.Forms.TextBox();
            this.btnSelectInstallDir = new System.Windows.Forms.Button();
            this.fbdInstallDir = new System.Windows.Forms.FolderBrowserDialog();
            this.cbCreateShortcut = new System.Windows.Forms.CheckBox();
            this.cbAfterInstallAction = new System.Windows.Forms.ComboBox();
            this.lblAfterInstallAction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbProduct
            // 
            this.cbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProduct.FormattingEnabled = true;
            this.cbProduct.Location = new System.Drawing.Point(79, 12);
            this.cbProduct.Name = "cbProduct";
            this.cbProduct.Size = new System.Drawing.Size(380, 21);
            this.cbProduct.TabIndex = 0;
            this.cbProduct.SelectedIndexChanged += new System.EventHandler(this.vbProduct_SelectedIndexChanged);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(9, 15);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(44, 13);
            this.lblProduct.TabIndex = 1;
            this.lblProduct.Text = "Product";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(9, 42);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(45, 13);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Version:";
            // 
            // cbVersion
            // 
            this.cbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVersion.FormattingEnabled = true;
            this.cbVersion.Location = new System.Drawing.Point(79, 39);
            this.cbVersion.Name = "cbVersion";
            this.cbVersion.Size = new System.Drawing.Size(380, 21);
            this.cbVersion.TabIndex = 3;
            this.cbVersion.SelectedIndexChanged += new System.EventHandler(this.cbVersion_SelectedIndexChanged);
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(12, 116);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(447, 23);
            this.btnInstall.TabIndex = 4;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Install Dir:";
            // 
            // tbInstallDir
            // 
            this.tbInstallDir.Location = new System.Drawing.Point(79, 66);
            this.tbInstallDir.Name = "tbInstallDir";
            this.tbInstallDir.Size = new System.Drawing.Size(336, 20);
            this.tbInstallDir.TabIndex = 6;
            this.tbInstallDir.TextChanged += new System.EventHandler(this.tbInstallDir_TextChanged);
            // 
            // btnSelectInstallDir
            // 
            this.btnSelectInstallDir.Location = new System.Drawing.Point(421, 64);
            this.btnSelectInstallDir.Name = "btnSelectInstallDir";
            this.btnSelectInstallDir.Size = new System.Drawing.Size(38, 23);
            this.btnSelectInstallDir.TabIndex = 7;
            this.btnSelectInstallDir.Text = "...";
            this.btnSelectInstallDir.UseVisualStyleBackColor = true;
            this.btnSelectInstallDir.Click += new System.EventHandler(this.btnSelectInstallDir_Click);
            // 
            // fbdInstallDir
            // 
            this.fbdInstallDir.Description = "Select Installation Destination";
            // 
            // cbCreateShortcut
            // 
            this.cbCreateShortcut.AutoSize = true;
            this.cbCreateShortcut.Checked = true;
            this.cbCreateShortcut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCreateShortcut.Location = new System.Drawing.Point(359, 93);
            this.cbCreateShortcut.Name = "cbCreateShortcut";
            this.cbCreateShortcut.Size = new System.Drawing.Size(100, 17);
            this.cbCreateShortcut.TabIndex = 8;
            this.cbCreateShortcut.Text = "Create Shortcut";
            this.cbCreateShortcut.UseVisualStyleBackColor = true;
            // 
            // cbAfterInstallAction
            // 
            this.cbAfterInstallAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAfterInstallAction.FormattingEnabled = true;
            this.cbAfterInstallAction.Items.AddRange(new object[] {
            "None",
            "Start Program",
            "Open Folder"});
            this.cbAfterInstallAction.Location = new System.Drawing.Point(79, 91);
            this.cbAfterInstallAction.Name = "cbAfterInstallAction";
            this.cbAfterInstallAction.Size = new System.Drawing.Size(265, 21);
            this.cbAfterInstallAction.TabIndex = 10;
            // 
            // lblAfterInstallAction
            // 
            this.lblAfterInstallAction.AutoSize = true;
            this.lblAfterInstallAction.Location = new System.Drawing.Point(9, 94);
            this.lblAfterInstallAction.Name = "lblAfterInstallAction";
            this.lblAfterInstallAction.Size = new System.Drawing.Size(62, 13);
            this.lblAfterInstallAction.TabIndex = 11;
            this.lblAfterInstallAction.Text = "After Install:";
            // 
            // SelectProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 142);
            this.Controls.Add(this.lblAfterInstallAction);
            this.Controls.Add(this.cbAfterInstallAction);
            this.Controls.Add(this.cbCreateShortcut);
            this.Controls.Add(this.btnSelectInstallDir);
            this.Controls.Add(this.tbInstallDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.cbVersion);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.cbProduct);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectProductForm";
            this.Text = "Select Product";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ComboBox cbVersion;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbInstallDir;
        private System.Windows.Forms.Button btnSelectInstallDir;
        private System.Windows.Forms.FolderBrowserDialog fbdInstallDir;
        private System.Windows.Forms.CheckBox cbCreateShortcut;
        private System.Windows.Forms.ComboBox cbAfterInstallAction;
        private System.Windows.Forms.Label lblAfterInstallAction;
    }
}

