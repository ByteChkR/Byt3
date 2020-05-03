using System.Windows.Forms;

namespace FLDebugger
{
    partial class CodeView
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
            this.btnContinue = new System.Windows.Forms.Button();
            this.panelSidePanel = new System.Windows.Forms.Panel();
            this.cbFollowScripts = new System.Windows.Forms.CheckBox();
            this.btnRunToEnd = new System.Windows.Forms.Button();
            this.lblActiveBuffer = new System.Windows.Forms.Label();
            this.lblActiveChannels = new System.Windows.Forms.Label();
            this.lbBuffers = new System.Windows.Forms.ListBox();
            this.btnHideSidePanel = new System.Windows.Forms.Button();
            this.btnShowSidePanel = new System.Windows.Forms.Button();
            this.lbInternalBuffers = new System.Windows.Forms.ListBox();
            this.lblInternalBuffers = new System.Windows.Forms.Label();
            this.panelSidePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.DimGray;
            this.btnContinue.Enabled = false;
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Location = new System.Drawing.Point(6, 12);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(156, 23);
            this.btnContinue.TabIndex = 1;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // panelSidePanel
            // 
            this.panelSidePanel.BackColor = System.Drawing.Color.DimGray;
            this.panelSidePanel.Controls.Add(this.lblInternalBuffers);
            this.panelSidePanel.Controls.Add(this.lbInternalBuffers);
            this.panelSidePanel.Controls.Add(this.cbFollowScripts);
            this.panelSidePanel.Controls.Add(this.btnRunToEnd);
            this.panelSidePanel.Controls.Add(this.lblActiveBuffer);
            this.panelSidePanel.Controls.Add(this.btnContinue);
            this.panelSidePanel.Controls.Add(this.lblActiveChannels);
            this.panelSidePanel.Controls.Add(this.lbBuffers);
            this.panelSidePanel.Controls.Add(this.btnHideSidePanel);
            this.panelSidePanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSidePanel.Location = new System.Drawing.Point(648, 0);
            this.panelSidePanel.Name = "panelSidePanel";
            this.panelSidePanel.Size = new System.Drawing.Size(166, 472);
            this.panelSidePanel.TabIndex = 2;
            this.panelSidePanel.Visible = false;
            // 
            // cbFollowScripts
            // 
            this.cbFollowScripts.AutoSize = true;
            this.cbFollowScripts.Location = new System.Drawing.Point(6, 71);
            this.cbFollowScripts.Name = "cbFollowScripts";
            this.cbFollowScripts.Size = new System.Drawing.Size(91, 17);
            this.cbFollowScripts.TabIndex = 10;
            this.cbFollowScripts.Text = "Follow Scripts";
            this.cbFollowScripts.UseVisualStyleBackColor = true;
            this.cbFollowScripts.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnRunToEnd
            // 
            this.btnRunToEnd.BackColor = System.Drawing.Color.DimGray;
            this.btnRunToEnd.Enabled = false;
            this.btnRunToEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunToEnd.Location = new System.Drawing.Point(6, 41);
            this.btnRunToEnd.Name = "btnRunToEnd";
            this.btnRunToEnd.Size = new System.Drawing.Size(156, 23);
            this.btnRunToEnd.TabIndex = 9;
            this.btnRunToEnd.Text = "Run to End";
            this.btnRunToEnd.UseVisualStyleBackColor = false;
            this.btnRunToEnd.Click += new System.EventHandler(this.btnRunToEnd_Click);
            // 
            // lblActiveBuffer
            // 
            this.lblActiveBuffer.AutoSize = true;
            this.lblActiveBuffer.Location = new System.Drawing.Point(3, 113);
            this.lblActiveBuffer.Name = "lblActiveBuffer";
            this.lblActiveBuffer.Size = new System.Drawing.Size(71, 13);
            this.lblActiveBuffer.TabIndex = 8;
            this.lblActiveBuffer.Text = "Active Buffer:";
            // 
            // lblActiveChannels
            // 
            this.lblActiveChannels.AutoSize = true;
            this.lblActiveChannels.Location = new System.Drawing.Point(3, 91);
            this.lblActiveChannels.Name = "lblActiveChannels";
            this.lblActiveChannels.Size = new System.Drawing.Size(87, 13);
            this.lblActiveChannels.TabIndex = 7;
            this.lblActiveChannels.Text = "Active Channels:";
            // 
            // lbBuffers
            // 
            this.lbBuffers.BackColor = System.Drawing.Color.DimGray;
            this.lbBuffers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBuffers.FormattingEnabled = true;
            this.lbBuffers.Location = new System.Drawing.Point(3, 150);
            this.lbBuffers.Name = "lbBuffers";
            this.lbBuffers.Size = new System.Drawing.Size(159, 132);
            this.lbBuffers.TabIndex = 5;
            // 
            // btnHideSidePanel
            // 
            this.btnHideSidePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHideSidePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHideSidePanel.Location = new System.Drawing.Point(4, 446);
            this.btnHideSidePanel.Name = "btnHideSidePanel";
            this.btnHideSidePanel.Size = new System.Drawing.Size(159, 23);
            this.btnHideSidePanel.TabIndex = 3;
            this.btnHideSidePanel.Text = "Hide";
            this.btnHideSidePanel.UseVisualStyleBackColor = true;
            this.btnHideSidePanel.Click += new System.EventHandler(this.btnHideSidePanel_Click);
            // 
            // btnShowSidePanel
            // 
            this.btnShowSidePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowSidePanel.BackColor = System.Drawing.Color.DimGray;
            this.btnShowSidePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowSidePanel.Location = new System.Drawing.Point(651, 446);
            this.btnShowSidePanel.Name = "btnShowSidePanel";
            this.btnShowSidePanel.Size = new System.Drawing.Size(160, 23);
            this.btnShowSidePanel.TabIndex = 4;
            this.btnShowSidePanel.Text = "Show";
            this.btnShowSidePanel.UseVisualStyleBackColor = false;
            this.btnShowSidePanel.Click += new System.EventHandler(this.btnShowSidePanel_Click);
            // 
            // lbInternalBuffers
            // 
            this.lbInternalBuffers.BackColor = System.Drawing.Color.DimGray;
            this.lbInternalBuffers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbInternalBuffers.FormattingEnabled = true;
            this.lbInternalBuffers.Location = new System.Drawing.Point(3, 308);
            this.lbInternalBuffers.Name = "lbInternalBuffers";
            this.lbInternalBuffers.Size = new System.Drawing.Size(159, 132);
            this.lbInternalBuffers.TabIndex = 11;
            // 
            // lblInternalBuffers
            // 
            this.lblInternalBuffers.AutoSize = true;
            this.lblInternalBuffers.Location = new System.Drawing.Point(3, 292);
            this.lblInternalBuffers.Name = "lblInternalBuffers";
            this.lblInternalBuffers.Size = new System.Drawing.Size(81, 13);
            this.lblInternalBuffers.TabIndex = 12;
            this.lblInternalBuffers.Text = "Internal Buffers:";
            // 
            // CodeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(814, 472);
            this.Controls.Add(this.btnShowSidePanel);
            this.Controls.Add(this.panelSidePanel);
            this.Name = "CodeView";
            this.Text = "CodeView";
            this.Load += new System.EventHandler(this.CodeView_Load);
            this.panelSidePanel.ResumeLayout(false);
            this.panelSidePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Panel panelSidePanel;
        private System.Windows.Forms.Label lblActiveBuffer;
        private System.Windows.Forms.Label lblActiveChannels;
        private System.Windows.Forms.ListBox lbBuffers;
        private System.Windows.Forms.Button btnHideSidePanel;
        private System.Windows.Forms.Button btnShowSidePanel;
        private System.Windows.Forms.Button btnRunToEnd;
        private CheckBox cbFollowScripts;
        private Label lblInternalBuffers;
        private ListBox lbInternalBuffers;
    }
}