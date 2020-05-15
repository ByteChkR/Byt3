using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FLDebugger.Projects.Instancing;
using FLDebugger.Projects.ProjectObjects;

namespace FLDebugger.Projects
{
    public partial class FLProjectExplorer : Form
    {
        private List<ExternalProgramEditor> Editors;
        private Project Project;
        private string[] Args;
        public FLProjectExplorer(string[] args)
        {
            Args = args;
            
            InitializeComponent();


            
        }


        private bool RefreshWorkingDirectory(string[] args)
        {
            Project = null;
            Editors = null;
            StartupSplash splash = new StartupSplash(args);
            Application.DoEvents();
            splash.Reload(false, tvWorkingDir);
            if (!splash.Cancel)
            {
                Editors = splash.GetEditors();
                Project = splash.GetProject();
                return true;
            }
            else
            {
                Close();
                return false;
            }
            
        }

        private void FLProjectExplorer_Load(object sender, EventArgs e)
        {
            
            Icon = Properties.Resources.OpenFL_Icon;
            tmrTreeViewRefresh.Start();
            lblVersion.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version;

            tvWorkingDir.NodeMouseDoubleClick += OnDoubleClickEntry;
            tvWorkingDir.NodeMouseClick += OnClickEntry;
            tvWorkingDir.KeyDown += TvWorkingDir_KeyDown;
            tvWorkingDir.KeyUp += TvWorkingDir_KeyUp;

            Closing += (s, eventArgs) => tmrTreeViewRefresh.Stop();

            if (RefreshWorkingDirectory(Args))
            {
                Text = Project.ProjectDirectory.Directory;
            }
        }

        private void OnClickEntry(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is FSDir dir)
            {
                //Check if Shift Click
                //Open in explorer
                if (ShiftClick)
                {
                    DomainHelper.CreateSubInstance(dir.EntryPath);
                    ShiftClick = false;
                }
            }
            else if (e.Node is FSFile file)
            {
                if (ShiftClick)
                {
                    Process.Start("explorer.exe", $"/select,{file.EntryPath}");
                    ShiftClick = false;
                }
            }
        }

        private bool ShiftClick;
        private void TvWorkingDir_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
                ShiftClick = false;
        }

        private void TvWorkingDir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
                ShiftClick = true;
        }

        private void OnDoubleClickEntry(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Node is FSFile file)
            {
                ExternalProgramEditor editor = GetEditor(file.GetExtension());
                if (editor == null)
                {
                    //TODO, ask for default program
                    MessageBox.Show("No Program defined for Extension: " + file.GetExtension(), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    editor.StartProgram(Project.ProjectDirectory, file);
                }
            }


        }

        private ExternalProgramEditor GetEditor(string extension) => Editors.FirstOrDefault(x => x.Extension == extension);

        private void tmrTreeViewRefresh_Tick(object sender, EventArgs e)
        {
            if (Project != null && Project.ProjectDirectory.IsDirty)
            {
                RefreshWorkingDirectory(new[] { Project.ProjectDirectory.Directory });
            }
        }

        
        private void btnAddEditor_Click(object sender, EventArgs e)
        {
            AddEditorDialog dialog = new AddEditorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StartupSplash splash = new StartupSplash(Args);
                splash.Reload(true, tvWorkingDir);
                if (!splash.Cancel)
                {
                    Editors = splash.GetEditors();
                }
            }
        }

        private void tvWorkingDir_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvWorkingDir.SelectedNode is FSFile file)
            {
                ExternalProgramEditor editor = GetEditor(file.GetExtension());
                if (editor == null)
                {
                    lblDefaultProgram.Text = $"Default Program: No Editor Defined";
                }
                else
                {
                    lblDefaultProgram.Text = $"Default Program: {editor}";
                }
            }
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            TreeNode node = tvWorkingDir.SelectedNode ?? tvWorkingDir.TopNode;
            node?.ExpandAll();
        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            TreeNode node = tvWorkingDir.SelectedNode ?? tvWorkingDir.TopNode;
            node?.Collapse(false);
        }
    }
}
