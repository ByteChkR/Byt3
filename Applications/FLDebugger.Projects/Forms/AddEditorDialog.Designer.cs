namespace FLDebugger.Projects
{
    partial class AddEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEditorDialog));
            this.tbEditorPath = new System.Windows.Forms.TextBox();
            this.lblEditorPath = new System.Windows.Forms.Label();
            this.btnBrowseEditor = new System.Windows.Forms.Button();
            this.lblFormat = new System.Windows.Forms.Label();
            this.tbFormat = new System.Windows.Forms.TextBox();
            this.tbExtensions = new System.Windows.Forms.TextBox();
            this.lblExtensions = new System.Windows.Forms.Label();
            this.lblExtensionsDesc = new System.Windows.Forms.Label();
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.lbOutput = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.ofdEditorPath = new System.Windows.Forms.OpenFileDialog();
            this.cbSetWorkingDir = new System.Windows.Forms.CheckBox();
            this.gbOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbEditorPath
            // 
            this.tbEditorPath.BackColor = System.Drawing.Color.DimGray;
            this.tbEditorPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbEditorPath.Location = new System.Drawing.Point(77, 13);
            this.tbEditorPath.Name = "tbEditorPath";
            this.tbEditorPath.Size = new System.Drawing.Size(424, 20);
            this.tbEditorPath.TabIndex = 0;
            this.tbEditorPath.TextChanged += new System.EventHandler(this.tbEditorPath_TextChanged);
            // 
            // lblEditorPath
            // 
            this.lblEditorPath.AutoSize = true;
            this.lblEditorPath.Location = new System.Drawing.Point(12, 16);
            this.lblEditorPath.Name = "lblEditorPath";
            this.lblEditorPath.Size = new System.Drawing.Size(59, 13);
            this.lblEditorPath.TabIndex = 1;
            this.lblEditorPath.Text = "Editor Path";
            // 
            // btnBrowseEditor
            // 
            this.btnBrowseEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseEditor.Location = new System.Drawing.Point(508, 12);
            this.btnBrowseEditor.Name = "btnBrowseEditor";
            this.btnBrowseEditor.Size = new System.Drawing.Size(32, 23);
            this.btnBrowseEditor.TabIndex = 2;
            this.btnBrowseEditor.Text = "...";
            this.btnBrowseEditor.UseVisualStyleBackColor = true;
            this.btnBrowseEditor.Click += new System.EventHandler(this.btnBrowseEditor_Click);
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(12, 42);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(39, 13);
            this.lblFormat.TabIndex = 4;
            this.lblFormat.Text = "Format";
            // 
            // tbFormat
            // 
            this.tbFormat.BackColor = System.Drawing.Color.DimGray;
            this.tbFormat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFormat.Location = new System.Drawing.Point(77, 39);
            this.tbFormat.Name = "tbFormat";
            this.tbFormat.Size = new System.Drawing.Size(424, 20);
            this.tbFormat.TabIndex = 3;
            this.tbFormat.Text = "{0}";
            this.tbFormat.TextChanged += new System.EventHandler(this.tbFormat_TextChanged);
            // 
            // tbExtensions
            // 
            this.tbExtensions.BackColor = System.Drawing.Color.DimGray;
            this.tbExtensions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbExtensions.Location = new System.Drawing.Point(77, 66);
            this.tbExtensions.Name = "tbExtensions";
            this.tbExtensions.Size = new System.Drawing.Size(100, 20);
            this.tbExtensions.TabIndex = 5;
            this.tbExtensions.TextChanged += new System.EventHandler(this.tbExtensions_TextChanged);
            // 
            // lblExtensions
            // 
            this.lblExtensions.AutoSize = true;
            this.lblExtensions.Location = new System.Drawing.Point(12, 69);
            this.lblExtensions.Name = "lblExtensions";
            this.lblExtensions.Size = new System.Drawing.Size(61, 13);
            this.lblExtensions.TabIndex = 6;
            this.lblExtensions.Text = "Extensions:";
            // 
            // lblExtensionsDesc
            // 
            this.lblExtensionsDesc.AutoSize = true;
            this.lblExtensionsDesc.Location = new System.Drawing.Point(183, 69);
            this.lblExtensionsDesc.Name = "lblExtensionsDesc";
            this.lblExtensionsDesc.Size = new System.Drawing.Size(100, 13);
            this.lblExtensionsDesc.TabIndex = 7;
            this.lblExtensionsDesc.Text = "(Comma Seperated)";
            // 
            // gbOutput
            // 
            this.gbOutput.Controls.Add(this.lbOutput);
            this.gbOutput.Location = new System.Drawing.Point(15, 100);
            this.gbOutput.Name = "gbOutput";
            this.gbOutput.Size = new System.Drawing.Size(525, 134);
            this.gbOutput.TabIndex = 8;
            this.gbOutput.TabStop = false;
            this.gbOutput.Text = "Output";
            // 
            // lbOutput
            // 
            this.lbOutput.BackColor = System.Drawing.Color.DimGray;
            this.lbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbOutput.FormattingEnabled = true;
            this.lbOutput.Location = new System.Drawing.Point(3, 16);
            this.lbOutput.Name = "lbOutput";
            this.lbOutput.Size = new System.Drawing.Size(519, 115);
            this.lbOutput.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(15, 237);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(525, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ofdEditorPath
            // 
            this.ofdEditorPath.FileName = "openFileDialog1";
            // 
            // cbSetWorkingDir
            // 
            this.cbSetWorkingDir.AutoSize = true;
            this.cbSetWorkingDir.Location = new System.Drawing.Point(371, 67);
            this.cbSetWorkingDir.Name = "cbSetWorkingDir";
            this.cbSetWorkingDir.Size = new System.Drawing.Size(130, 17);
            this.cbSetWorkingDir.TabIndex = 10;
            this.cbSetWorkingDir.Text = "Set Working Directory";
            this.cbSetWorkingDir.UseVisualStyleBackColor = true;
            // 
            // AddEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(552, 262);
            this.Controls.Add(this.cbSetWorkingDir);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbOutput);
            this.Controls.Add(this.lblExtensionsDesc);
            this.Controls.Add(this.lblExtensions);
            this.Controls.Add(this.tbExtensions);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.tbFormat);
            this.Controls.Add(this.btnBrowseEditor);
            this.Controls.Add(this.lblEditorPath);
            this.Controls.Add(this.tbEditorPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddEditorDialog";
            this.Text = "Add a new Editor";
            this.Load += new System.EventHandler(this.AddEditorDialog_Load);
            this.gbOutput.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbEditorPath;
        private System.Windows.Forms.Label lblEditorPath;
        private System.Windows.Forms.Button btnBrowseEditor;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.TextBox tbFormat;
        private System.Windows.Forms.TextBox tbExtensions;
        private System.Windows.Forms.Label lblExtensions;
        private System.Windows.Forms.Label lblExtensionsDesc;
        private System.Windows.Forms.GroupBox gbOutput;
        private System.Windows.Forms.ListBox lbOutput;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog ofdEditorPath;
        private System.Windows.Forms.CheckBox cbSetWorkingDir;
    }
}