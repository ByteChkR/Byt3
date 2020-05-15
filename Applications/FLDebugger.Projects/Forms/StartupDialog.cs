using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FLDebugger.Projects.Properties;

namespace FLDebugger.Projects.Forms
{
    public partial class StartupDialog : Form
    {
        public string SelectedPath { get; private set; }
        public List<string> PreviousPaths = new List<string>();
        private static readonly string RECENT_PROJECT_FILE = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "configs", "projects.txt");
        public StartupDialog()
        {
            InitializeComponent();

            Icon = Resources.OpenFL_Icon;

            flpLastProjects.VerticalScroll.Enabled = flpLastProjects.VerticalScroll.Visible = true;
            flpLastProjects.HorizontalScroll.Enabled = flpLastProjects.HorizontalScroll.Visible = false;

            if (!Directory.Exists(Path.GetDirectoryName(RECENT_PROJECT_FILE)))
                Directory.CreateDirectory(Path.GetDirectoryName(RECENT_PROJECT_FILE));

            if (File.Exists(RECENT_PROJECT_FILE))
            {
                string[] projects = File.ReadAllLines(RECENT_PROJECT_FILE);
                foreach (string project in projects)
                {
                    if (Directory.Exists(project))
                    {
                        PreviousPaths.Add(project);
                        Button btn = CreateButtonForProject(project);
                        btn.Parent = flpLastProjects;
                        flpLastProjects.SizeChanged += (sender, args) => RecentSizeChanged(btn);
                        RecentSizeChanged(btn);
                    }
                }
            }
        }

        private void RecentSizeChanged(Button btn)
        {
            btn.Width = btn.Parent.ClientRectangle.Width - btn.Margin.Horizontal;
        }


        private Button CreateButtonForProject(string path)
        {
            Button ret = new Button();
            ret.MouseClick += (sender, args) => SelectPath(path);
            ret.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ret.Location = new System.Drawing.Point(3, 3);
            ret.Name = path;
            ret.Size = new System.Drawing.Size(335, 23);
            ret.TabIndex = 0;
            ret.Text = path;
            ret.UseVisualStyleBackColor = true;
            return ret;
        }

        private void SelectPath(string p)
        {
            SelectedPath = p;
            if (!PreviousPaths.Contains(p))
            {
                PreviousPaths.Add(p);
                File.WriteAllLines(RECENT_PROJECT_FILE, PreviousPaths);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (fbdProjectFolder.ShowDialog() == DialogResult.OK)
            {
                SelectedPath = fbdProjectFolder.SelectedPath;
                if (!PreviousPaths.Contains(SelectedPath))
                {
                    PreviousPaths.Add(SelectedPath);
                    File.WriteAllLines(RECENT_PROJECT_FILE, PreviousPaths);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
