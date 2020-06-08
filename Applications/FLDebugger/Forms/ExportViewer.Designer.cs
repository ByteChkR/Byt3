namespace FLDebugger.Forms
{
    partial class ExportViewer
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
            this.pbExportView = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbExportView)).BeginInit();
            this.SuspendLayout();
            // 
            // pbExportView
            // 
            this.pbExportView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbExportView.Location = new System.Drawing.Point(0, 0);
            this.pbExportView.Name = "pbExportView";
            this.pbExportView.Size = new System.Drawing.Size(800, 450);
            this.pbExportView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbExportView.TabIndex = 0;
            this.pbExportView.TabStop = false;
            // 
            // ExportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbExportView);
            this.Name = "ExportViewer";
            this.Text = "ExportViewer";
            ((System.ComponentModel.ISupportInitialize)(this.pbExportView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbExportView;
    }
}