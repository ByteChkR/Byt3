namespace FLDebugger.Forms
{
    partial class BuildExceptionViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuildExceptionViewer));
            this.lbEx = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbLoadedKernels = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbBuildOut = new System.Windows.Forms.GroupBox();
            this.rtbExText = new System.Windows.Forms.RichTextBox();
            this.btnRetry = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.gbLoadedKernels.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbBuildOut.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbEx
            // 
            this.lbEx.BackColor = System.Drawing.Color.DimGray;
            this.lbEx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbEx.FormattingEnabled = true;
            this.lbEx.Location = new System.Drawing.Point(3, 16);
            this.lbEx.Name = "lbEx";
            this.lbEx.Size = new System.Drawing.Size(324, 431);
            this.lbEx.TabIndex = 0;
            this.lbEx.SelectedIndexChanged += new System.EventHandler(this.lbEx_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbLoadedKernels);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(470, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 450);
            this.panel1.TabIndex = 1;
            // 
            // gbLoadedKernels
            // 
            this.gbLoadedKernels.BackColor = System.Drawing.Color.DimGray;
            this.gbLoadedKernels.Controls.Add(this.lbEx);
            this.gbLoadedKernels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLoadedKernels.Location = new System.Drawing.Point(0, 0);
            this.gbLoadedKernels.Name = "gbLoadedKernels";
            this.gbLoadedKernels.Size = new System.Drawing.Size(330, 450);
            this.gbLoadedKernels.TabIndex = 1;
            this.gbLoadedKernels.TabStop = false;
            this.gbLoadedKernels.Text = "Loaded Kernels:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gbBuildOut);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(470, 450);
            this.panel2.TabIndex = 2;
            // 
            // gbBuildOut
            // 
            this.gbBuildOut.BackColor = System.Drawing.Color.DimGray;
            this.gbBuildOut.Controls.Add(this.btnRetry);
            this.gbBuildOut.Controls.Add(this.rtbExText);
            this.gbBuildOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBuildOut.Location = new System.Drawing.Point(0, 0);
            this.gbBuildOut.Name = "gbBuildOut";
            this.gbBuildOut.Size = new System.Drawing.Size(470, 450);
            this.gbBuildOut.TabIndex = 1;
            this.gbBuildOut.TabStop = false;
            this.gbBuildOut.Text = "Build Output:";
            // 
            // rtbExText
            // 
            this.rtbExText.BackColor = System.Drawing.Color.DimGray;
            this.rtbExText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbExText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbExText.Location = new System.Drawing.Point(3, 16);
            this.rtbExText.Name = "rtbExText";
            this.rtbExText.ReadOnly = true;
            this.rtbExText.Size = new System.Drawing.Size(464, 431);
            this.rtbExText.TabIndex = 0;
            this.rtbExText.Text = "";
            // 
            // btnRetry
            // 
            this.btnRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRetry.Location = new System.Drawing.Point(7, 421);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(75, 23);
            this.btnRetry.TabIndex = 1;
            this.btnRetry.Text = "Retry";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // BuildExceptionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BuildExceptionViewer";
            this.Text = "BuildExceptionViewer";
            this.Load += new System.EventHandler(this.BuildExceptionViewer_Load);
            this.panel1.ResumeLayout(false);
            this.gbLoadedKernels.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.gbBuildOut.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbEx;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtbExText;
        private System.Windows.Forms.GroupBox gbLoadedKernels;
        private System.Windows.Forms.GroupBox gbBuildOut;
        private System.Windows.Forms.Button btnRetry;
    }
}