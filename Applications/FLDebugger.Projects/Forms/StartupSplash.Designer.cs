namespace FLDebugger.Projects
{
    partial class StartupSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupSplash));
            this.lblProcessText = new System.Windows.Forms.Label();
            this.startup = new System.Windows.Forms.Timer(this.components);
            this.fbdProjectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProcessText
            // 
            this.lblProcessText.BackColor = System.Drawing.Color.Gray;
            this.lblProcessText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessText.ForeColor = System.Drawing.Color.White;
            this.lblProcessText.Location = new System.Drawing.Point(0, 200);
            this.lblProcessText.Name = "lblProcessText";
            this.lblProcessText.Size = new System.Drawing.Size(200, 25);
            this.lblProcessText.TabIndex = 1;
            this.lblProcessText.Text = "Loading.";
            this.lblProcessText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startup
            // 
            this.startup.Interval = 1000;
            this.startup.Tick += new System.EventHandler(this.startup_Tick);
            // 
            // fbdProjectFolder
            // 
            this.fbdProjectFolder.Description = "Select Project Directory";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(0, 228);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(200, 10);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 2;
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Gray;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(0, 183);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(200, 17);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Version: 9.9.99.9999";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::FLDebugger.Projects.Properties.Resources.OpenFL;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 200);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // StartupSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(200, 242);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblProcessText);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartupSplash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartupSplash";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.Load += new System.EventHandler(this.StartupSplash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblProcessText;
        private System.Windows.Forms.Timer startup;
        private System.Windows.Forms.FolderBrowserDialog fbdProjectFolder;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblVersion;
    }
}