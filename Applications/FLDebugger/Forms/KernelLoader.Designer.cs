namespace FLDebugger.Forms
{
    partial class KernelLoader
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
            this.pbSplash = new System.Windows.Forms.PictureBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.panelLog = new System.Windows.Forms.Panel();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLoad = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkFinishTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbSplash)).BeginInit();
            this.panelLog.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbSplash
            // 
            this.pbSplash.BackColor = System.Drawing.Color.Transparent;
            this.pbSplash.Image = global::FLDebugger.Properties.Resources.OpenFL;
            this.pbSplash.Location = new System.Drawing.Point(12, 12);
            this.pbSplash.Name = "pbSplash";
            this.pbSplash.Size = new System.Drawing.Size(190, 190);
            this.pbSplash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSplash.TabIndex = 0;
            this.pbSplash.TabStop = false;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.DimGray;
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.ForeColor = System.Drawing.Color.White;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(394, 129);
            this.rtbLog.TabIndex = 1;
            this.rtbLog.Text = "";
            // 
            // panelLog
            // 
            this.panelLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLog.Controls.Add(this.rtbLog);
            this.panelLog.Location = new System.Drawing.Point(208, 72);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(396, 131);
            this.panelLog.TabIndex = 2;
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(209, 56);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(396, 10);
            this.pbProgress.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.DimGray;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(394, 20);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "AAAAAAAAA";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLoad
            // 
            this.lblLoad.AutoSize = true;
            this.lblLoad.BackColor = System.Drawing.Color.DimGray;
            this.lblLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoad.ForeColor = System.Drawing.Color.White;
            this.lblLoad.Location = new System.Drawing.Point(310, 9);
            this.lblLoad.Name = "lblLoad";
            this.lblLoad.Size = new System.Drawing.Size(195, 18);
            this.lblLoad.TabIndex = 5;
            this.lblLoad.Text = "Loading OpenCL Kernels";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Location = new System.Drawing.Point(209, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 20);
            this.panel1.TabIndex = 6;
            // 
            // checkFinishTimer
            // 
            this.checkFinishTimer.Interval = 1000;
            this.checkFinishTimer.Tick += new System.EventHandler(this.checkFinishTimer_Tick);
            // 
            // KernelLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(616, 215);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblLoad);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.pbSplash);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "KernelLoader";
            this.Text = "KernelLoader";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.Load += new System.EventHandler(this.KernelLoader_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbSplash)).EndInit();
            this.panelLog.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbSplash;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLoad;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer checkFinishTimer;
    }
}