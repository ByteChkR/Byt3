namespace FLDebugger.Projects
{
    partial class FLProjectExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLProjectExplorer));
            this.tmrTreeViewRefresh = new System.Windows.Forms.Timer();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnCollapse = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.lblShortcuts = new System.Windows.Forms.Label();
            this.btnAddEditor = new System.Windows.Forms.Button();
            this.lblDefaultProgram = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tvWorkingDir = new System.Windows.Forms.TreeView();
            this.panelTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrTreeViewRefresh
            // 
            this.tmrTreeViewRefresh.Tick += new System.EventHandler(this.tmrTreeViewRefresh_Tick);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblVersion);
            this.panelTop.Controls.Add(this.btnCollapse);
            this.panelTop.Controls.Add(this.btnExpand);
            this.panelTop.Controls.Add(this.lblShortcuts);
            this.panelTop.Controls.Add(this.btnAddEditor);
            this.panelTop.Controls.Add(this.lblDefaultProgram);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(347, 103);
            this.panelTop.TabIndex = 2;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(12, 87);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(45, 13);
            this.lblVersion.TabIndex = 5;
            this.lblVersion.Text = "Version:";
            // 
            // btnCollapse
            // 
            this.btnCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCollapse.Location = new System.Drawing.Point(267, 57);
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(75, 23);
            this.btnCollapse.TabIndex = 4;
            this.btnCollapse.Text = "Collapse All";
            this.btnCollapse.UseVisualStyleBackColor = true;
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpand.Location = new System.Drawing.Point(267, 31);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(75, 23);
            this.btnExpand.TabIndex = 3;
            this.btnExpand.Text = "Expand All";
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // lblShortcuts
            // 
            this.lblShortcuts.AutoSize = true;
            this.lblShortcuts.Location = new System.Drawing.Point(12, 31);
            this.lblShortcuts.Name = "lblShortcuts";
            this.lblShortcuts.Size = new System.Drawing.Size(174, 39);
            this.lblShortcuts.TabIndex = 2;
            this.lblShortcuts.Text = "Shift+Click(File): Open in Explorer\r\nDoubleClick(File): Open with Editor\r\nShift+C" +
    "lick(Folder): Open as Project";
            // 
            // btnAddEditor
            // 
            this.btnAddEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddEditor.Location = new System.Drawing.Point(267, 5);
            this.btnAddEditor.Name = "btnAddEditor";
            this.btnAddEditor.Size = new System.Drawing.Size(75, 23);
            this.btnAddEditor.TabIndex = 1;
            this.btnAddEditor.Text = "Add Editor";
            this.btnAddEditor.UseVisualStyleBackColor = true;
            this.btnAddEditor.Click += new System.EventHandler(this.btnAddEditor_Click);
            // 
            // lblDefaultProgram
            // 
            this.lblDefaultProgram.AutoSize = true;
            this.lblDefaultProgram.Location = new System.Drawing.Point(12, 9);
            this.lblDefaultProgram.Name = "lblDefaultProgram";
            this.lblDefaultProgram.Size = new System.Drawing.Size(86, 13);
            this.lblDefaultProgram.TabIndex = 0;
            this.lblDefaultProgram.Text = "Default Program:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tvWorkingDir);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 103);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 408);
            this.panel1.TabIndex = 3;
            // 
            // tvWorkingDir
            // 
            this.tvWorkingDir.BackColor = System.Drawing.Color.DimGray;
            this.tvWorkingDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvWorkingDir.Location = new System.Drawing.Point(0, 0);
            this.tvWorkingDir.Name = "tvWorkingDir";
            this.tvWorkingDir.Size = new System.Drawing.Size(347, 408);
            this.tvWorkingDir.TabIndex = 0;
            this.tvWorkingDir.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvWorkingDir_AfterSelect);
            // 
            // FLProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(347, 511);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(363, 141);
            this.Name = "FLProjectExplorer";
            this.Text = "Project Explorer";
            this.Load += new System.EventHandler(this.FLProjectExplorer_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrTreeViewRefresh;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblDefaultProgram;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAddEditor;
        private System.Windows.Forms.Label lblShortcuts;
        private System.Windows.Forms.TreeView tvWorkingDir;
        private System.Windows.Forms.Button btnCollapse;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Label lblVersion;
    }
}

