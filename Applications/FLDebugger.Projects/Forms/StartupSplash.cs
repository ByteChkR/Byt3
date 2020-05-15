using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using FLDebugger.Projects.Forms;
using FLDebugger.Projects.ProjectObjects;

namespace FLDebugger.Projects
{
    public partial class StartupSplash : Form
    {
        private object ProcessTextLock = new object();

        public static readonly string EditorConfigPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "configs", "editors");

        public bool Cancel { get; private set; }
        private List<ExternalProgramEditor> Editors;
        private string[] Args;
        private Project project;
        private bool onlyEditor;

        private TreeView _tv;

        private Task LoadTask;

        public Project GetProject()
        {
            return project;
        }

        public List<ExternalProgramEditor> GetEditors()
        {
            return Editors;
        }

        public StartupSplash(string[] args)
        {
            Args = args;
            InitializeComponent();
        }


        StartupDialog dialog = new StartupDialog();

        private void StartupSplash_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.OpenFL_Icon;
            lblVersion.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version;
            Point pos = Screen.PrimaryScreen.Bounds.Location + new Size(Screen.PrimaryScreen.Bounds.Width / 2,
                            Screen.PrimaryScreen.Bounds.Height / 2) - new Size(Bounds.Width / 2, Bounds.Height / 2);
            Location = pos;
            CheckForIllegalCrossThreadCalls = false;
            if (Args.Length == 0)
            {
                while (dialog.ShowDialog() != DialogResult.OK)
                {
                    DialogResult res = MessageBox.Show("Invalid Folder.", "Error", MessageBoxButtons.RetryCancel);
                    if (res == DialogResult.Cancel)
                    {
                        Cancel = true;
                        Close();
                        return;
                    }
                }

                Args = new[] { dialog.SelectedPath };
            }
            LoadTask = new Task(Initialize);
            LoadTask.Start();
            startup.Start(); //Temporary timer for debugging purposes.
        }

        public void Reload(bool onlyEditor, TreeView tv)
        {
            _tv = tv;
            this.onlyEditor = onlyEditor;
            ShowDialog();
        }


        private void LoadEditors()
        {


            string path = EditorConfigPath;
            XmlSerializer xs = new XmlSerializer(typeof(ExternalProgramEditor));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Stream newS = File.Create(Path.Combine(path, "fleditor.xml"));
                xs.Serialize(newS, new ExternalProgramEditor(".fl") { Format = "{0} {1}", Target = @"D:\Users\Tim\Documents\Byt3\Applications\FLDebugger\bin\Debug\FLDebugger.exe", SetWorkingDir = false });
                newS.Close();
            }

            string[] configs = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);

            Editors = new List<ExternalProgramEditor>();
            for (int i = 0; i < configs.Length; i++)
            {
                string config = configs[i];
                SetText($"Loading Editor {i + 1}/{configs.Length}");

                Stream s = File.OpenRead(config);
                Editors.Add((ExternalProgramEditor)xs.Deserialize(s));
                s.Close();
            }

            //if (!File.Exists("./editors.xml"))
            //{
            //    Stream sNew = File.Create("./editors.xml");
            //    xs.Serialize(sNew, new List<ExternalProgramEditor>{ new ExternalProgramEditor(".fl") { Format = "{0} {1}", Target = @"D:\Users\Tim\Documents\Byt3\Applications\FLDebugger\bin\Debug\FLDebugger.exe" } });
            //    sNew.Close();
            //}
            //Stream s = File.OpenRead("./editors.xml");
            //Editors = (List<ExternalProgramEditor>) xs.Deserialize(s);
            //s.Close();
        }

        private void SetText(string text)
        {
            lock (ProcessTextLock)
            {
                lblProcessText.Text = text;
            }
        }

        private void Initialize()
        {
            LoadEditors();
            if (onlyEditor)
            {
                Close();
                return;
            }

            SetText("Initializing Working Directory");

            if (Args.Length == 0)
            {
                throw new InvalidOperationException("Argument for Project Folder is required.");
            }

            project = new Project() { ProjectDirectory = new WorkingDirectory(Path.GetFullPath(Args[0])) };




            SetText("Loading Finished");
        }


        private void startup_Tick(object sender, EventArgs e)
        {
            startup.Interval = 10;
            if (FSEntry.Entries != 0 && WorkingDirectory.FilesMax != 0)
            {
                SetText($"Files Loaded: {FSEntry.Entries}/{WorkingDirectory.FilesMax}");
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Maximum = WorkingDirectory.FilesMax;
                progressBar.Value = Math.Min(WorkingDirectory.FilesMax, FSEntry.Entries);
            }
            if (LoadTask.IsCompleted)
            {
                FSEntry.Entries = WorkingDirectory.FilesMax = 0;

                SetText("Creating Tree View");
                Application.DoEvents();
                if (_tv != null)
                {
                    project?.ProjectDirectory.UpdateTreeView(_tv);
                    _tv = null;
                }

                Close();
            }
        }
    }
}
