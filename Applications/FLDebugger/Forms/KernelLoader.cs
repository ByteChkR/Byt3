using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.Callbacks;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.Utilities.ManifestIO;
using Byt3.Utilities.TypeFinding;
using FLDebugger.Utils;

namespace FLDebugger.Forms
{

    public partial class KernelLoader : Form
    {
        private readonly string Path;
        private readonly CLAPI Instance;
        private readonly Task LoadTask;

        public KernelDatabase Database { get; private set; }

        public KernelLoader(CLAPI instance, string path)
        {
            Instance = instance;
            Path = path;

            InitializeComponent();

            Icon = Properties.Resources.OpenFL_Icon;

            CheckForIllegalCrossThreadCalls = false;
            LoadTask = new Task(InitializeDatabase);


            FLScriptEditor.RegisterDefaultTheme(rtbLog);
            FLScriptEditor.RegisterDefaultTheme(panelLog);
            FLScriptEditor.RegisterDefaultTheme(pbProgress);
            FLScriptEditor.RegisterDefaultTheme(lblStatus);
            FLScriptEditor.RegisterDefaultTheme(lblLoad);
            FLScriptEditor.RegisterDefaultTheme(panel1);
        }

        private void KernelLoader_Load(object sender, EventArgs e)
        {
            Closing += KernelLoader_Closing;

            TypeAccumulator.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly);
            ManifestReader.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly);
            ManifestReader.PrepareManifestFiles(false);
            ManifestReader.PrepareManifestFiles(true);
            EmbeddedFileIOManager.Initialize();


            Point pos = Screen.PrimaryScreen.Bounds.Location + new Size(Screen.PrimaryScreen.Bounds.Width / 2,
                            Screen.PrimaryScreen.Bounds.Height / 2) - new Size(Bounds.Width / 2, Bounds.Height / 2);
            Location = pos;

            LoadTask.Start();
            checkFinishTimer.Start();
        }

        private void KernelLoader_Closing(object sender, CancelEventArgs e)
        {
        }

        private void InitializeDatabase()
        {
            lblStatus.Text = "Discovering Files in Path: " + Path;
            string[] files = IOManager.GetFiles(Path, "*.cl");
            pbProgress.Maximum = files.Length;
            pbProgress.Value = 0;
            if (files.Length == 0)
            {
                DialogResult res = MessageBox.Show("No Files found at path: " + Path, "Error",
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (res == DialogResult.Retry)
                {
                    InitializeDatabase();
                    return;
                }
                else if (res == DialogResult.Abort)
                {
                    DialogResult = DialogResult.Abort;
                    Close();
                    return;
                }
                else if (res == DialogResult.Ignore)
                {
                    DialogResult = DialogResult.OK;
                }
            }

            Database = new KernelDatabase(DataVectorTypes.Uchar1);
            List<CLProgramBuildResult> results = new List<CLProgramBuildResult>();
            bool throwEx = false;
            int kernelCount = 0;
            foreach (string file in files)
            {
                rtbLog.AppendLine("Loading File: " + file, Color.White, rtbLog.BackColor);
                pbProgress.Value++;
                try
                {
                    CLProgram prog = Database.AddProgram(Instance, file, false, out CLProgramBuildResult res);
                    kernelCount += prog.ContainedKernels.Count;
                    throwEx |= !res;
                    results.Add(res);
                }
                catch (Exception e)
                {
                    rtbLog.AppendLine("ERROR: " + e.Message, Color.Red, rtbLog.BackColor);

                    throw e; //Let the Exception Viewer Catch that
                }
                lblStatus.Text = $"Kernels Loaded: {kernelCount}";
            }

            lblStatus.Text = "Loading Finished";
            rtbLog.AppendLine("Loading Finished", Color.White, rtbLog.BackColor);
            rtbLog.AppendLine("Kernels Loaded: " + kernelCount, Color.White, rtbLog.BackColor);

            if (throwEx)
            {
                DialogResult res =
                    MessageBox.Show(
                        "There are errors in one or more OpenCL kernels. Do you want to open the OpenCL Build Excepion Viewer?",
                        "OpenCL Build Errors", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Abort;
                    Close();
                }
                else if (res == DialogResult.Yes)
                {
                    BuildExceptionViewer bvr = new BuildExceptionViewer(new CLBuildException(results));
                    if (bvr.ShowDialog() == DialogResult.Retry)
                    {
                        Database.Dispose();
                        InitializeDatabase();
                        return;
                    }
                }
            }
        }

        private void checkFinishTimer_Tick(object sender, EventArgs e)
        {
            if (LoadTask.IsCompleted)
            {
                checkFinishTimer.Stop();
                if (LoadTask.IsFaulted)
                {
                    throw LoadTask.Exception;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
