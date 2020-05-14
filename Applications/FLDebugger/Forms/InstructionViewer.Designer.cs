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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInstructionName = new System.Windows.Forms.Label();
            this.panelInstructions.SuspendLayout();
            this.gbInstructions.SuspendLayout();
            this.panelMainInstructionView.SuspendLayout();
            this.panelOverloads.SuspendLayout();
            this.gbOverloads.SuspendLayout();
            this.panelHeaderInfo.SuspendLayout();
            this.panelLegend.SuspendLayout();
            this.gbLegend.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInstructions
            // 
            this.panelInstructions.BackColor = System.Drawing.Color.DimGray;
            this.panelInstructions.Controls.Add(this.gbInstructions);
            this.panelInstructions.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelInstructions.Location = new System.Drawing.Point(0, 0);
            this.panelInstructions.Name = "panelInstructions";
            this.panelInstructions.Size = new System.Drawing.Size(200, 356);
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
            this.gbInstructions.Size = new System.Drawing.Size(200, 356);
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
            this.lbInstructions.Size = new System.Drawing.Size(194, 337);
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
            this.panelMainInstructionView.Size = new System.Drawing.Size(442, 356);
            this.panelMainInstructionView.TabIndex = 1;
            // 
            // panelOverloads
            // 
            this.panelOverloads.BackColor = System.Drawing.Color.DimGray;
            this.panelOverloads.Controls.Add(this.gbOverloads);
            this.panelOverloads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOverloads.Location = new System.Drawing.Point(0, 203);
            this.panelOverloads.Name = "panelOverloads";
            this.panelOverloads.Size = new System.Drawing.Size(442, 153);
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
            this.gbOverloads.Size = new System.Drawing.Size(442, 153);
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
            this.lbOverloads.Size = new System.Drawing.Size(436, 134);
            this.lbOverloads.TabIndex = 0;
            // 
            // panelHeaderInfo
            // 
            this.panelHeaderInfo.BackColor = System.Drawing.Color.DimGray;
            this.panelHeaderInfo.Controls.Add(this.panelLegend);
            this.panelHeaderInfo.Controls.Add(this.panel1);
            this.panelHeaderInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeaderInfo.Location = new System.Drawing.Point(0, 0);
            this.panelHeaderInfo.Name = "panelHeaderInfo";
            this.panelHeaderInfo.Size = new System.Drawing.Size(442, 203);
            this.panelHeaderInfo.TabIndex = 1;
            // 
            // panelLegend
            // 
            this.panelLegend.Controls.Add(this.gbLegend);
            this.panelLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLegend.Location = new System.Drawing.Point(0, 31);
            this.panelLegend.Name = "panelLegend";
            this.panelLegend.Size = new System.Drawing.Size(442, 172);
            this.panelLegend.TabIndex = 4;
            // 
            // gbLegend
            // 
            this.gbLegend.Controls.Add(this.lbLegend);
            this.gbLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLegend.Location = new System.Drawing.Point(0, 0);
            this.gbLegend.Name = "gbLegend";
            this.gbLegend.Size = new System.Drawing.Size(442, 172);
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
            this.lbLegend.Size = new System.Drawing.Size(436, 153);
            this.lbLegend.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblInstructionName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(442, 31);
            this.panel1.TabIndex = 3;
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
            this.ClientSize = new System.Drawing.Size(642, 356);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.Panel panel1;
    }
}