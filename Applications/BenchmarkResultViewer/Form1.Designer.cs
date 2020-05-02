namespace BenchmarkResultViewer
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.StripLine stripLine2 = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.fbdSelectDir = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.allTests = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.allList = new System.Windows.Forms.CheckedListBox();
            this.btnScreen = new System.Windows.Forms.Button();
            this.sfdScreen = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.allTests.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fbdSelectDir
            // 
            this.fbdSelectDir.Description = "Select Performance Folder";
            this.fbdSelectDir.SelectedPath = "D:\\Users\\Tim\\Documents\\Byt3\\Consoles\\TestingProject\\bin\\Debug\\netcoreapp2.1\\perfo" +
    "rmance";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.allTests);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(983, 458);
            this.tabControl1.TabIndex = 0;
            // 
            // allTests
            // 
            this.allTests.Controls.Add(this.panel2);
            this.allTests.Controls.Add(this.panel1);
            this.allTests.Location = new System.Drawing.Point(4, 22);
            this.allTests.Name = "allTests";
            this.allTests.Size = new System.Drawing.Size(975, 432);
            this.allTests.TabIndex = 0;
            this.allTests.Text = "All";
            this.allTests.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnScreen);
            this.panel2.Controls.Add(this.chart1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(651, 432);
            this.panel2.TabIndex = 3;
            // 
            // chart1
            // 
            stripLine2.Text = "AAA";
            chartArea2.AxisX.StripLines.Add(stripLine2);
            chartArea2.AxisX.Title = "Versions";
            chartArea2.Name = "MainArea";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(651, 432);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "All";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.allList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(651, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 432);
            this.panel1.TabIndex = 2;
            // 
            // allList
            // 
            this.allList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allList.FormattingEnabled = true;
            this.allList.Location = new System.Drawing.Point(0, 0);
            this.allList.Name = "allList";
            this.allList.Size = new System.Drawing.Size(324, 432);
            this.allList.TabIndex = 1;
            // 
            // btnScreen
            // 
            this.btnScreen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnScreen.Location = new System.Drawing.Point(0, 409);
            this.btnScreen.Name = "btnScreen";
            this.btnScreen.Size = new System.Drawing.Size(651, 23);
            this.btnScreen.TabIndex = 1;
            this.btnScreen.Text = "Make Screenshot";
            this.btnScreen.UseVisualStyleBackColor = true;
            this.btnScreen.Click += new System.EventHandler(this.btnScreen_Click);
            // 
            // sfdScreen
            // 
            this.sfdScreen.Filter = "\"PNG|*.png\"";
            this.sfdScreen.Title = "Save Screenshot";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 458);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Benchmark Result Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.allTests.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog fbdSelectDir;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage allTests;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckedListBox allList;
        private System.Windows.Forms.Button btnScreen;
        private System.Windows.Forms.SaveFileDialog sfdScreen;
    }
}

