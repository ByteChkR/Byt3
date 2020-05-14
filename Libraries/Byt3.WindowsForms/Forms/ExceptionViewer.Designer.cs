namespace Byt3.WindowsForms.Forms
{
    partial class ExceptionViewer
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
            this.tvExceptionView = new System.Windows.Forms.TreeView();
            this.panelTv = new System.Windows.Forms.Panel();
            this.panelEx = new System.Windows.Forms.Panel();
            this.panelExCallstack = new System.Windows.Forms.Panel();
            this.rtbCallStack = new System.Windows.Forms.RichTextBox();
            this.panelExMessage = new System.Windows.Forms.Panel();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.panelExTop = new System.Windows.Forms.Panel();
            this.lblExeptionType = new System.Windows.Forms.Label();
            this.lblHResult = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblTargetSite = new System.Windows.Forms.Label();
            this.lblHelpLink = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.gbExceptionView = new System.Windows.Forms.GroupBox();
            this.gbMessage = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelTv.SuspendLayout();
            this.panelEx.SuspendLayout();
            this.panelExCallstack.SuspendLayout();
            this.panelExMessage.SuspendLayout();
            this.panelExTop.SuspendLayout();
            this.gbExceptionView.SuspendLayout();
            this.gbMessage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvExceptionView
            // 
            this.tvExceptionView.BackColor = System.Drawing.Color.DimGray;
            this.tvExceptionView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvExceptionView.Location = new System.Drawing.Point(3, 16);
            this.tvExceptionView.Name = "tvExceptionView";
            this.tvExceptionView.Size = new System.Drawing.Size(301, 431);
            this.tvExceptionView.TabIndex = 0;
            // 
            // panelTv
            // 
            this.panelTv.BackColor = System.Drawing.Color.DimGray;
            this.panelTv.Controls.Add(this.gbExceptionView);
            this.panelTv.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTv.Location = new System.Drawing.Point(0, 0);
            this.panelTv.Name = "panelTv";
            this.panelTv.Size = new System.Drawing.Size(307, 450);
            this.panelTv.TabIndex = 0;
            this.panelTv.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panelEx
            // 
            this.panelEx.BackColor = System.Drawing.Color.DimGray;
            this.panelEx.Controls.Add(this.panelExCallstack);
            this.panelEx.Controls.Add(this.panelExTop);
            this.panelEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx.Location = new System.Drawing.Point(307, 0);
            this.panelEx.Name = "panelEx";
            this.panelEx.Size = new System.Drawing.Size(493, 450);
            this.panelEx.TabIndex = 1;
            // 
            // panelExCallstack
            // 
            this.panelExCallstack.BackColor = System.Drawing.Color.DimGray;
            this.panelExCallstack.Controls.Add(this.groupBox1);
            this.panelExCallstack.Controls.Add(this.panelExMessage);
            this.panelExCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExCallstack.Location = new System.Drawing.Point(0, 84);
            this.panelExCallstack.Name = "panelExCallstack";
            this.panelExCallstack.Size = new System.Drawing.Size(493, 366);
            this.panelExCallstack.TabIndex = 3;
            // 
            // rtbCallStack
            // 
            this.rtbCallStack.BackColor = System.Drawing.Color.DimGray;
            this.rtbCallStack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbCallStack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCallStack.Location = new System.Drawing.Point(3, 16);
            this.rtbCallStack.Name = "rtbCallStack";
            this.rtbCallStack.Size = new System.Drawing.Size(487, 192);
            this.rtbCallStack.TabIndex = 3;
            this.rtbCallStack.Text = "";
            // 
            // panelExMessage
            // 
            this.panelExMessage.BackColor = System.Drawing.Color.DimGray;
            this.panelExMessage.Controls.Add(this.gbMessage);
            this.panelExMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExMessage.Location = new System.Drawing.Point(0, 0);
            this.panelExMessage.Name = "panelExMessage";
            this.panelExMessage.Size = new System.Drawing.Size(493, 155);
            this.panelExMessage.TabIndex = 2;
            // 
            // rtbMessage
            // 
            this.rtbMessage.BackColor = System.Drawing.Color.DimGray;
            this.rtbMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMessage.Location = new System.Drawing.Point(3, 16);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(487, 136);
            this.rtbMessage.TabIndex = 1;
            this.rtbMessage.Text = "";
            // 
            // panelExTop
            // 
            this.panelExTop.BackColor = System.Drawing.Color.DimGray;
            this.panelExTop.Controls.Add(this.btnRestart);
            this.panelExTop.Controls.Add(this.lblHelpLink);
            this.panelExTop.Controls.Add(this.lblTargetSite);
            this.panelExTop.Controls.Add(this.lblSource);
            this.panelExTop.Controls.Add(this.lblHResult);
            this.panelExTop.Controls.Add(this.lblExeptionType);
            this.panelExTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExTop.Location = new System.Drawing.Point(0, 0);
            this.panelExTop.Name = "panelExTop";
            this.panelExTop.Size = new System.Drawing.Size(493, 84);
            this.panelExTop.TabIndex = 2;
            // 
            // lblExeptionType
            // 
            this.lblExeptionType.AutoSize = true;
            this.lblExeptionType.BackColor = System.Drawing.Color.DimGray;
            this.lblExeptionType.Location = new System.Drawing.Point(6, 9);
            this.lblExeptionType.Name = "lblExeptionType";
            this.lblExeptionType.Size = new System.Drawing.Size(31, 13);
            this.lblExeptionType.TabIndex = 0;
            this.lblExeptionType.Text = "Type";
            // 
            // lblHResult
            // 
            this.lblHResult.AutoSize = true;
            this.lblHResult.BackColor = System.Drawing.Color.DimGray;
            this.lblHResult.Location = new System.Drawing.Point(6, 22);
            this.lblHResult.Name = "lblHResult";
            this.lblHResult.Size = new System.Drawing.Size(45, 13);
            this.lblHResult.TabIndex = 1;
            this.lblHResult.Text = "HResult";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.BackColor = System.Drawing.Color.DimGray;
            this.lblSource.Location = new System.Drawing.Point(6, 35);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(41, 13);
            this.lblSource.TabIndex = 2;
            this.lblSource.Text = "Source";
            // 
            // lblTargetSite
            // 
            this.lblTargetSite.AutoSize = true;
            this.lblTargetSite.BackColor = System.Drawing.Color.DimGray;
            this.lblTargetSite.Location = new System.Drawing.Point(6, 48);
            this.lblTargetSite.Name = "lblTargetSite";
            this.lblTargetSite.Size = new System.Drawing.Size(59, 13);
            this.lblTargetSite.TabIndex = 3;
            this.lblTargetSite.Text = "Target Site";
            // 
            // lblHelpLink
            // 
            this.lblHelpLink.AutoSize = true;
            this.lblHelpLink.BackColor = System.Drawing.Color.DimGray;
            this.lblHelpLink.Location = new System.Drawing.Point(6, 61);
            this.lblHelpLink.Name = "lblHelpLink";
            this.lblHelpLink.Size = new System.Drawing.Size(49, 13);
            this.lblHelpLink.TabIndex = 4;
            this.lblHelpLink.Text = "HelpLink";
            // 
            // btnRestart
            // 
            this.btnRestart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestart.BackColor = System.Drawing.Color.DimGray;
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Location = new System.Drawing.Point(374, 9);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(107, 23);
            this.btnRestart.TabIndex = 5;
            this.btnRestart.Text = "Restart Application";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.button1_Click);
            // 
            // gbExceptionView
            // 
            this.gbExceptionView.BackColor = System.Drawing.Color.DimGray;
            this.gbExceptionView.Controls.Add(this.tvExceptionView);
            this.gbExceptionView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbExceptionView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbExceptionView.Location = new System.Drawing.Point(0, 0);
            this.gbExceptionView.Name = "gbExceptionView";
            this.gbExceptionView.Size = new System.Drawing.Size(307, 450);
            this.gbExceptionView.TabIndex = 1;
            this.gbExceptionView.TabStop = false;
            this.gbExceptionView.Text = "Exception Tree:";
            // 
            // gbMessage
            // 
            this.gbMessage.BackColor = System.Drawing.Color.DimGray;
            this.gbMessage.Controls.Add(this.rtbMessage);
            this.gbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbMessage.Location = new System.Drawing.Point(0, 0);
            this.gbMessage.Name = "gbMessage";
            this.gbMessage.Size = new System.Drawing.Size(493, 155);
            this.gbMessage.TabIndex = 2;
            this.gbMessage.TabStop = false;
            this.gbMessage.Text = "Message:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DimGray;
            this.groupBox1.Controls.Add(this.rtbCallStack);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(0, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 211);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stack Trace:";
            // 
            // ExceptionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelEx);
            this.Controls.Add(this.panelTv);
            this.Name = "ExceptionViewer";
            this.Text = "ExceptionViewer";
            this.panelTv.ResumeLayout(false);
            this.panelEx.ResumeLayout(false);
            this.panelExCallstack.ResumeLayout(false);
            this.panelExMessage.ResumeLayout(false);
            this.panelExTop.ResumeLayout(false);
            this.panelExTop.PerformLayout();
            this.gbExceptionView.ResumeLayout(false);
            this.gbMessage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvExceptionView;
        private System.Windows.Forms.Panel panelTv;
        private System.Windows.Forms.Panel panelEx;
        private System.Windows.Forms.Panel panelExCallstack;
        private System.Windows.Forms.RichTextBox rtbCallStack;
        private System.Windows.Forms.Panel panelExMessage;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Panel panelExTop;
        private System.Windows.Forms.Label lblExeptionType;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblHResult;
        private System.Windows.Forms.Label lblHelpLink;
        private System.Windows.Forms.Label lblTargetSite;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.GroupBox gbExceptionView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbMessage;
    }
}