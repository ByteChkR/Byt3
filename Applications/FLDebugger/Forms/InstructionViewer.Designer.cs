namespace FLDebugger.Forms
{
    partial class InstructionViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstructionViewer));
            this.panelInstructions = new System.Windows.Forms.Panel();
            this.gbInstructions = new System.Windows.Forms.GroupBox();
            this.lbInstructions = new System.Windows.Forms.ListBox();
            this.panelMainInstructionView = new System.Windows.Forms.Panel();
            this.panelOverloads = new System.Windows.Forms.Panel();
            this.gbOverloads = new System.Windows.Forms.GroupBox();
            this.lbOverloads = new System.Windows.Forms.ListBox();
            this.panelHeaderInfo = new System.Windows.Forms.Panel();
            this.panelLegend = new System.Windows.Forms.Panel();
            this.gbLegend = new System.Windows.Forms.GroupBox();
            this.lbLegend = new System.Windows.Forms.ListBox();
            this.panelInstructionInfo = new System.Windows.Forms.Panel();
            this.panelInstructionDescription = new System.Windows.Forms.Panel();
            this.gbInstructionDescription = new System.Windows.Forms.GroupBox();
            this.rtbInstructionDescription = new System.Windows.Forms.RichTextBox();
            this.panelInstructionName = new System.Windows.Forms.Panel();
            this.lblInstructionName = new System.Windows.Forms.Label();
            this.panelInstructions.SuspendLayout();
            this.gbInstructions.SuspendLayout();
            this.panelMainInstructionView.SuspendLayout();
            this.panelOverloads.SuspendLayout();
            this.gbOverloads.SuspendLayout();
            this.panelHeaderInfo.SuspendLayout();
            this.panelLegend.SuspendLayout();
            this.gbLegend.SuspendLayout();
            this.panelInstructionInfo.SuspendLayout();
            this.panelInstructionDescription.SuspendLayout();
            this.gbInstructionDescription.SuspendLayout();
            this.panelInstructionName.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInstructions
            // 
            this.panelInstructions.BackColor = System.Drawing.Color.DimGray;
            this.panelInstructions.Controls.Add(this.gbInstructions);
            this.panelInstructions.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelInstructions.Location = new System.Drawing.Point(0, 0);
            this.panelInstructions.Name = "panelInstructions";
            this.panelInstructions.Size = new System.Drawing.Size(200, 563);
            this.panelInstructions.TabIndex = 0;
            // 
            // gbInstructions
            // 
            this.gbInstructions.BackColor = System.Drawing.Color.DimGray;
            this.gbInstructions.Controls.Add(this.lbInstructions);
            this.gbInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbInstructions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbInstructions.Location = new System.Drawing.Point(0, 0);
            this.gbInstructions.Name = "gbInstructions";
            this.gbInstructions.Size = new System.Drawing.Size(200, 563);
            this.gbInstructions.TabIndex = 0;
            this.gbInstructions.TabStop = false;
            this.gbInstructions.Text = "Instructions";
            // 
            // lbInstructions
            // 
            this.lbInstructions.BackColor = System.Drawing.Color.DimGray;
            this.lbInstructions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbInstructions.FormattingEnabled = true;
            this.lbInstructions.Location = new System.Drawing.Point(3, 16);
            this.lbInstructions.Name = "lbInstructions";
            this.lbInstructions.Size = new System.Drawing.Size(194, 544);
            this.lbInstructions.TabIndex = 0;
            this.lbInstructions.SelectedIndexChanged += new System.EventHandler(this.lbInstructions_SelectedIndexChanged);
            // 
            // panelMainInstructionView
            // 
            this.panelMainInstructionView.BackColor = System.Drawing.Color.DimGray;
            this.panelMainInstructionView.Controls.Add(this.panelOverloads);
            this.panelMainInstructionView.Controls.Add(this.panelHeaderInfo);
            this.panelMainInstructionView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainInstructionView.Location = new System.Drawing.Point(200, 0);
            this.panelMainInstructionView.Name = "panelMainInstructionView";
            this.panelMainInstructionView.Size = new System.Drawing.Size(392, 563);
            this.panelMainInstructionView.TabIndex = 1;
            // 
            // panelOverloads
            // 
            this.panelOverloads.BackColor = System.Drawing.Color.DimGray;
            this.panelOverloads.Controls.Add(this.gbOverloads);
            this.panelOverloads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOverloads.Location = new System.Drawing.Point(0, 429);
            this.panelOverloads.Name = "panelOverloads";
            this.panelOverloads.Size = new System.Drawing.Size(392, 134);
            this.panelOverloads.TabIndex = 2;
            // 
            // gbOverloads
            // 
            this.gbOverloads.BackColor = System.Drawing.Color.DimGray;
            this.gbOverloads.Controls.Add(this.lbOverloads);
            this.gbOverloads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOverloads.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbOverloads.Location = new System.Drawing.Point(0, 0);
            this.gbOverloads.Name = "gbOverloads";
            this.gbOverloads.Size = new System.Drawing.Size(392, 134);
            this.gbOverloads.TabIndex = 0;
            this.gbOverloads.TabStop = false;
            this.gbOverloads.Text = "Instruction Overloads:";
            // 
            // lbOverloads
            // 
            this.lbOverloads.BackColor = System.Drawing.Color.DimGray;
            this.lbOverloads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbOverloads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbOverloads.FormattingEnabled = true;
            this.lbOverloads.Location = new System.Drawing.Point(3, 16);
            this.lbOverloads.Name = "lbOverloads";
            this.lbOverloads.Size = new System.Drawing.Size(386, 115);
            this.lbOverloads.TabIndex = 0;
            // 
            // panelHeaderInfo
            // 
            this.panelHeaderInfo.BackColor = System.Drawing.Color.DimGray;
            this.panelHeaderInfo.Controls.Add(this.panelLegend);
            this.panelHeaderInfo.Controls.Add(this.panelInstructionInfo);
            this.panelHeaderInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeaderInfo.Location = new System.Drawing.Point(0, 0);
            this.panelHeaderInfo.Name = "panelHeaderInfo";
            this.panelHeaderInfo.Size = new System.Drawing.Size(392, 429);
            this.panelHeaderInfo.TabIndex = 1;
            // 
            // panelLegend
            // 
            this.panelLegend.Controls.Add(this.gbLegend);
            this.panelLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLegend.Location = new System.Drawing.Point(0, 258);
            this.panelLegend.Name = "panelLegend";
            this.panelLegend.Size = new System.Drawing.Size(392, 171);
            this.panelLegend.TabIndex = 4;
            // 
            // gbLegend
            // 
            this.gbLegend.Controls.Add(this.lbLegend);
            this.gbLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLegend.Location = new System.Drawing.Point(0, 0);
            this.gbLegend.Name = "gbLegend";
            this.gbLegend.Size = new System.Drawing.Size(392, 171);
            this.gbLegend.TabIndex = 1;
            this.gbLegend.TabStop = false;
            this.gbLegend.Text = "Instruction Argument Legend:";
            // 
            // lbLegend
            // 
            this.lbLegend.BackColor = System.Drawing.Color.DimGray;
            this.lbLegend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLegend.FormattingEnabled = true;
            this.lbLegend.Items.AddRange(new object[] {
            "Value = decimal",
            "Function = Function in the same Script",
            "Script = Defined External Script Name",
            "Buffer = Defined Buffer",
            "DefinedElement = Script|Function|Buffer",
            "DefinedFunction = Script|Function",
            "InternalDefinedElement = Function|Buffer",
            "Name = String literal(not checked at compile time)",
            "NumberResolvable = Anything that resolves to a Value",
            "AllElements = All of the Above",
            "Invalid = Can not parse Argument Signature"});
            this.lbLegend.Location = new System.Drawing.Point(3, 16);
            this.lbLegend.Name = "lbLegend";
            this.lbLegend.Size = new System.Drawing.Size(386, 152);
            this.lbLegend.TabIndex = 0;
            // 
            // panelInstructionInfo
            // 
            this.panelInstructionInfo.Controls.Add(this.panelInstructionDescription);
            this.panelInstructionInfo.Controls.Add(this.panelInstructionName);
            this.panelInstructionInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInstructionInfo.Location = new System.Drawing.Point(0, 0);
            this.panelInstructionInfo.Name = "panelInstructionInfo";
            this.panelInstructionInfo.Size = new System.Drawing.Size(392, 258);
            this.panelInstructionInfo.TabIndex = 3;
            // 
            // panelInstructionDescription
            // 
            this.panelInstructionDescription.Controls.Add(this.gbInstructionDescription);
            this.panelInstructionDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInstructionDescription.Location = new System.Drawing.Point(0, 29);
            this.panelInstructionDescription.Name = "panelInstructionDescription";
            this.panelInstructionDescription.Size = new System.Drawing.Size(392, 229);
            this.panelInstructionDescription.TabIndex = 2;
            // 
            // gbInstructionDescription
            // 
            this.gbInstructionDescription.Controls.Add(this.rtbInstructionDescription);
            this.gbInstructionDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbInstructionDescription.Location = new System.Drawing.Point(0, 0);
            this.gbInstructionDescription.Name = "gbInstructionDescription";
            this.gbInstructionDescription.Size = new System.Drawing.Size(392, 229);
            this.gbInstructionDescription.TabIndex = 0;
            this.gbInstructionDescription.TabStop = false;
            this.gbInstructionDescription.Text = "Instruction Description:";
            // 
            // rtbInstructionDescription
            // 
            this.rtbInstructionDescription.BackColor = System.Drawing.Color.DimGray;
            this.rtbInstructionDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbInstructionDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInstructionDescription.Location = new System.Drawing.Point(3, 16);
            this.rtbInstructionDescription.Name = "rtbInstructionDescription";
            this.rtbInstructionDescription.ReadOnly = true;
            this.rtbInstructionDescription.Size = new System.Drawing.Size(386, 210);
            this.rtbInstructionDescription.TabIndex = 0;
            this.rtbInstructionDescription.Text = "";
            // 
            // panelInstructionName
            // 
            this.panelInstructionName.Controls.Add(this.lblInstructionName);
            this.panelInstructionName.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInstructionName.Location = new System.Drawing.Point(0, 0);
            this.panelInstructionName.Name = "panelInstructionName";
            this.panelInstructionName.Size = new System.Drawing.Size(392, 29);
            this.panelInstructionName.TabIndex = 1;
            // 
            // lblInstructionName
            // 
            this.lblInstructionName.AutoSize = true;
            this.lblInstructionName.BackColor = System.Drawing.Color.DimGray;
            this.lblInstructionName.Location = new System.Drawing.Point(6, 9);
            this.lblInstructionName.Name = "lblInstructionName";
            this.lblInstructionName.Size = new System.Drawing.Size(90, 13);
            this.lblInstructionName.TabIndex = 0;
            this.lblInstructionName.Text = "Instruction Name:";
            // 
            // InstructionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 563);
            this.Controls.Add(this.panelMainInstructionView);
            this.Controls.Add(this.panelInstructions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(492, 283);
            this.Name = "InstructionViewer";
            this.Text = "InstructionViewer";
            this.Load += new System.EventHandler(this.InstructionViewer_Load);
            this.panelInstructions.ResumeLayout(false);
            this.gbInstructions.ResumeLayout(false);
            this.panelMainInstructionView.ResumeLayout(false);
            this.panelOverloads.ResumeLayout(false);
            this.gbOverloads.ResumeLayout(false);
            this.panelHeaderInfo.ResumeLayout(false);
            this.panelLegend.ResumeLayout(false);
            this.gbLegend.ResumeLayout(false);
            this.panelInstructionInfo.ResumeLayout(false);
            this.panelInstructionDescription.ResumeLayout(false);
            this.gbInstructionDescription.ResumeLayout(false);
            this.panelInstructionName.ResumeLayout(false);
            this.panelInstructionName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelInstructions;
        private System.Windows.Forms.GroupBox gbInstructions;
        private System.Windows.Forms.ListBox lbInstructions;
        private System.Windows.Forms.Panel panelMainInstructionView;
        private System.Windows.Forms.Panel panelOverloads;
        private System.Windows.Forms.GroupBox gbOverloads;
        private System.Windows.Forms.ListBox lbOverloads;
        private System.Windows.Forms.Panel panelHeaderInfo;
        private System.Windows.Forms.Label lblInstructionName;
        private System.Windows.Forms.Panel panelLegend;
        private System.Windows.Forms.GroupBox gbLegend;
        private System.Windows.Forms.ListBox lbLegend;
        private System.Windows.Forms.Panel panelInstructionInfo;
        private System.Windows.Forms.Panel panelInstructionDescription;
        private System.Windows.Forms.GroupBox gbInstructionDescription;
        private System.Windows.Forms.RichTextBox rtbInstructionDescription;
        private System.Windows.Forms.Panel panelInstructionName;
    }
}