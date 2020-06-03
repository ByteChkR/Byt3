namespace FLDebugger.Forms
{
    partial class BufferView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BufferView));
            this.panelSide = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.pbIdle = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.panelImage = new System.Windows.Forms.Panel();
            this.sfdExport = new System.Windows.Forms.SaveFileDialog();
            this.nudFrame = new System.Windows.Forms.NumericUpDown();
            this.panelSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIdle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSide
            // 
            this.panelSide.BackColor = System.Drawing.Color.DimGray;
            this.panelSide.Controls.Add(this.nudFrame);
            this.panelSide.Controls.Add(this.btnExport);
            this.panelSide.Controls.Add(this.pbIdle);
            this.panelSide.Controls.Add(this.comboBox1);
            this.panelSide.Controls.Add(this.btnReload);
            this.panelSide.Controls.Add(this.pbLoading);
            this.panelSide.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSide.Location = new System.Drawing.Point(758, 0);
            this.panelSide.Name = "panelSide";
            this.panelSide.Size = new System.Drawing.Size(194, 497);
            this.panelSide.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Location = new System.Drawing.Point(6, 59);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(185, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // pbIdle
            // 
            this.pbIdle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbIdle.BackColor = System.Drawing.Color.DimGray;
            this.pbIdle.Image = global::FLDebugger.Properties.Resources.OpenFL;
            this.pbIdle.Location = new System.Drawing.Point(116, 419);
            this.pbIdle.Name = "pbIdle";
            this.pbIdle.Size = new System.Drawing.Size(75, 75);
            this.pbIdle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbIdle.TabIndex = 1;
            this.pbIdle.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 32);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(185, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnReload
            // 
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.Location = new System.Drawing.Point(6, 3);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(185, 23);
            this.btnReload.TabIndex = 0;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // pbLoading
            // 
            this.pbLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLoading.Image = global::FLDebugger.Properties.Resources.loading_pac;
            this.pbLoading.Location = new System.Drawing.Point(116, 419);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(75, 75);
            this.pbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLoading.TabIndex = 0;
            this.pbLoading.TabStop = false;
            // 
            // panelImage
            // 
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 0);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(758, 497);
            this.panelImage.TabIndex = 1;
            // 
            // sfdExport
            // 
            this.sfdExport.DefaultExt = "png";
            this.sfdExport.FileName = "export.png";
            this.sfdExport.Filter = "Portable Network Graphic|*.png|Bitmap|*.bmp";
            this.sfdExport.Title = "Export Buffer Content";
            // 
            // nudFrame
            // 
            this.nudFrame.Location = new System.Drawing.Point(45, 98);
            this.nudFrame.Name = "nudFrame";
            this.nudFrame.Size = new System.Drawing.Size(120, 20);
            this.nudFrame.TabIndex = 5;
            // 
            // BufferView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 497);
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.panelSide);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BufferView";
            this.Text = "BufferView";
            this.Load += new System.EventHandler(this.BufferView_Load);
            this.panelSide.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIdle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSide;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.PictureBox pbIdle;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog sfdExport;
        private System.Windows.Forms.NumericUpDown nudFrame;
    }
}