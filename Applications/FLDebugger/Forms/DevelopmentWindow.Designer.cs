namespace FLDebugger.Forms
{
    partial class DevelopmentWindow
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
            this.btnRestart = new System.Windows.Forms.Button();
            this.cbExperimentalKernelLoading = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(12, 12);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 23);
            this.btnRestart.TabIndex = 0;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // cbExperimentalKernelLoading
            // 
            this.cbExperimentalKernelLoading.AutoSize = true;
            this.cbExperimentalKernelLoading.Location = new System.Drawing.Point(12, 41);
            this.cbExperimentalKernelLoading.Name = "cbExperimentalKernelLoading";
            this.cbExperimentalKernelLoading.Size = new System.Drawing.Size(160, 17);
            this.cbExperimentalKernelLoading.TabIndex = 1;
            this.cbExperimentalKernelLoading.Text = "Experimental Kernel Loading";
            this.cbExperimentalKernelLoading.UseVisualStyleBackColor = true;
            this.cbExperimentalKernelLoading.CheckedChanged += new System.EventHandler(this.cbExperimentalKernelLoading_CheckedChanged);
            // 
            // DevelopmentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(179, 71);
            this.Controls.Add(this.cbExperimentalKernelLoading);
            this.Controls.Add(this.btnRestart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevelopmentWindow";
            this.Text = "Dev Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.CheckBox cbExperimentalKernelLoading;
    }
}