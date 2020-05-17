using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.ADL.Streams;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Parsing.StageResults;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.Utilities.ManifestIO;
using Byt3.Utilities.TypeFinding;
using Byt3.WindowsForms.CustomControls;
using Byt3.WindowsForms.Forms;
using FLDebugger.Properties;
using FLDebugger.Utils;
using Debug = Byt3.ADL.Debug;

namespace FLDebugger.Forms
{
    public partial class FLScriptEditor : Form
    {
        private static readonly string TempEditorContentPath = System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "tempeditorcontent_" + System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) +
            ".fl");

        private static readonly string DEFAULT_SCRIPT =
            $"{FLKeywords.EntryFunctionKey}:\n\tsetactive 3\n\tsetv 1\n\tsetactive 0 1 2\n\tsetv 1";


        private AboutInfo aboutForm;
        private bool allowLogUpdate;
        private FLDataContainer Container;


        private bool ControlMod;

        private bool ignoreChanged;

        private InstructionViewer iv;
        private LogDisplay logDisplay;

        private string logOut = "";
        private readonly Stream ms = new PipeStream();
        private bool optimizationsDirty;
        private bool outputDirty = true;
        private string Path;
        private ContainerForm previewForm;

        private PictureBox previewPicture;
        private Task previewTask;

        private TextReader tr;

        public FLScriptEditor()
        {
            InitializeComponent();
            rtbIn.WriteSource(DEFAULT_SCRIPT);
            cbBuildMode.SelectedIndex = 0;
            CheckForIllegalCrossThreadCalls = false;
        }

        public FLScriptEditor(string path) : this()
        {
            Path = path;
            rtbIn.WriteSource(File.ReadAllText(Path));
        }

        public FLScriptEditor(string path, string workingDir) : this(path)
        {
            Directory.SetCurrentDirectory(workingDir);
        }


        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFLScript.ShowDialog() == DialogResult.OK)
            {
                Path = openFLScript.FileName;
                rtbIn.Text = File.ReadAllText(Path);
            }
        }


        private static ResizeableControl MakeResizable(Control control, ResizeableControl.EdgeEnum activeEdges,
            params Control[] dockedChildren)
        {
            ResizeableControl ret = new ResizeableControl(control, dockedChildren);
            ret.DrawOutline = true;
            ret.OutlineColor = CodeViewHelper.SourceBackColor;
            ret.Edges = activeEdges;
            return ret;
        }

        private Task<SerializableFLProgram> ParseProgram()
        {
            Container.CheckBuilder.RemoveAllProgramChecks();
            List<FLProgramCheck> si = lbOptimizations.CheckedItems.Cast<FLProgramCheck>().ToList();
            si.ForEach(x => Container.CheckBuilder.AddProgramCheck(x));
            Container.CheckBuilder.Attach(Container.Parser, true);


            Task<SerializableFLProgram> loadT =
                new Task<SerializableFLProgram>(() => Container.Parser.Process(new FLParserInput(Path, cbBuildMode.SelectedItem.ToString().ToUpper())));
            return loadT;
        }

        private Exception GetInnerIfAggregate(Exception ex)
        {
            return ex is AggregateException ag ? GetInnerIfAggregate(ag.InnerExceptions.First()) : ex;
        }

        private void InitializeViewer()
        {
            if (Container.CheckBuilder.IsAttached)
            {
                Container.CheckBuilder.Detach(false);
            }


            string source = File.ReadAllText(Path);
            Text = "FL Parse Output Viewing: " + Path;
            Task<SerializableFLProgram> loadT = ParseProgram();

            loadT.Start();
            while (!loadT.IsCompleted)
            {
                Application.DoEvents();
            }

            if (loadT.IsFaulted)
            {
                rtbOut.Text = source;

                string s = $"Errors: {loadT.Exception.InnerExceptions.Count}\n";

                foreach (Exception exceptionInnerException in loadT.Exception.InnerExceptions)
                {
                    Exception ex = GetInnerIfAggregate(exceptionInnerException);
                    s += "PARSE ERROR:\n\t" + ex.Message + "\n";
                }

                SetLogOutput(s);
            }
            else
            {
                outputDirty = false;
                Container.SerializedProgram = loadT.Result;
                string s = Container.SerializedProgram.ToString();
                if (s.Count(x => x == '\n') > 100)
                {
                    rtbOut.Text = s;
                    return;
                }

                rtbOut.WriteSource(s);
                SetLogOutput("Build Succeeded.\n");
            }
        }

        private void rtbIn_TextChanged(object sender, EventArgs e)
        {
            outputDirty = true;
            if (ignoreChanged)
            {
                return;
            }

            if (tmrConsoleColors.Enabled)
            {
                tmrConsoleColors.Stop();
            }

            tmrConsoleColors.Start();
        }

        private void tmrConsoleRefresh_Tick(object sender, EventArgs e)
        {
            string r = tr.ReadToEnd();

            logDisplay.Append(r);
        }

        private void frmOptimizationView_Load(object sender, EventArgs e)
        {
            Icon = Resources.OpenFL_Icon;
            Closing += FLScriptEditor_Closing;
            logDisplay = new LogDisplay();


            FLDebugger.Initialize();
            DoubleBuffered = true;


            //MakeResizable(panelCodeArea, ResizeableControl.EdgeEnum.None);
            //MakeResizable(panelToolbar, ResizeableControl.EdgeEnum.Left|ResizeableControl.EdgeEnum.Right);
            MakeResizable(panelConsoleOut, ResizeableControl.EdgeEnum.Top, rtbParserOutput);
            MakeResizable(panelInput, ResizeableControl.EdgeEnum.Right, rtbIn);


            panelCodeArea.Resize += PanelCodeArea_Resize;

            lbOptimizations.KeyDown += OnKeyDown;
            lbOptimizations.KeyUp += OnKeyUp;

            tmrConsoleRefresh.Start();


            LogTextStream ls = new LogTextStream(ms);
            Debug.DefaultInitialization();
            Debug.AddOutputStream(ls);
            tr = new StreamReader(ms);

            TypeAccumulator.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly);
            ManifestReader.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly);
            ManifestReader.PrepareManifestFiles(false);
            ManifestReader.PrepareManifestFiles(true);
            EmbeddedFileIOManager.Initialize();


            Container = new FLDataContainer();


            List<FLProgramCheck> types = typeof(OpenFLDebugConfig).Assembly.GetExportedTypes()
                .Where(x => typeof(FLProgramCheck).IsAssignableFrom(x) && !x.IsAbstract && x != typeof(FLProgramCheck))
                .Select(x => (FLProgramCheck)Activator.CreateInstance(x)).ToList();

            types.Sort((x, y) => x.Priority.CompareTo(y.Priority));

            lbOptimizations.ItemCheck += LbOptimizationsOnItemCheck;
            rtbParserOutput.TextChanged += RtbParserOutputTextChanged;
            for (int i = 0; i < types.Count; i++)
            {
                FLProgramCheck type = types[i];
                lbOptimizations.Items.Add(type);
                if (type.Recommended)
                {
                    lbOptimizations.SetItemChecked(i, true);
                }
            }
        }

        private void FLScriptEditor_Closing(object sender, CancelEventArgs e)
        {
            if (File.Exists(TempEditorContentPath))
            {
                File.Delete(TempEditorContentPath);
            }

            Application.Exit();
        }

        private void SetLogOutput(string r)
        {
            allowLogUpdate = true;
            string pr = "";
            pr += $"Parse Result: {Path}\n";
            pr += r;
            pr += "____________________________________________________\n";
            logOut += pr;
            rtbParserOutput.AppendText(pr);
            allowLogUpdate = false;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ControlMod = false;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ControlMod = true;
            }
        }


        private void PanelCodeArea_Resize(object sender, EventArgs e)
        {
            panelInput.Width = panelOutput.Width = panelCodeArea.Width / 2;
        }

        private void RtbParserOutputTextChanged(object sender, EventArgs e)
        {
            if (allowLogUpdate)
            {
                rtbParserOutput.ScrollToCaret();
            }
            else
            {
                rtbParserOutput.Text = logOut;
            }
        }

        private void LbOptimizationsOnItemCheck(object sender, ItemCheckEventArgs e)
        {
            optimizationsDirty = true;
        }

        private void ComputePreview()
        {
            if (previewPicture != null && (previewTask == null || previewTask.IsCompleted))
            {
                previewTask = new Task(() =>
                {
                    if (Container.SerializedProgram == null)
                    {
                        throw new InvalidOperationException();
                    }

                    FLProgram pro = null;
                    try
                    {
                        pro = Container.SerializedProgram.Initialize(Container.Instance, Container.InstructionSet);

                        pro.Run(new FLBuffer(Container.Instance, 512, 512, "Preview Buffer"), true);
                        if (previewPicture != null)
                        {
                            Bitmap bmp = new Bitmap(512, 512);
                            CLAPI.UpdateBitmap(Container.Instance, bmp, pro.GetActiveBuffer(false).Buffer);
                            previewPicture.Image = bmp;
                        }

                        pro.FreeResources();
                    }
                    catch (Exception ex)
                    {
                        pro?.FreeResources();
                        if (previewPicture != null)
                        {
                            previewPicture.Image = SystemIcons.Error.ToBitmap();
                        }
                    }
                });
                previewTask.Start();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            InitProgram();
            ComputePreview();
        }

        private void InitProgram()
        {
            if (string.IsNullOrEmpty(Path))
            {
                string path = TempEditorContentPath;
                File.WriteAllText(path, rtbIn.Text);
                Path = path;
            }

            InitializeViewer();
        }

        private void tmrConsoleColors_Tick(object sender, EventArgs e)
        {
            tmrConsoleColors.Stop();
            ignoreChanged = true;
            string src = rtbIn.Text;
            if (src.Count(c => c == '\n') > 100)
            {
                rtbIn.Text = src; //no colorcoding to prevent loading forever
                return;
            }

            rtbIn.WriteSource(src);
            Path = "";
            ignoreChanged = false;

            if (cbAutoBuild.Checked)
            {
                InitProgram();
                ComputePreview();
            }
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            if (Container.SerializedProgram == null || optimizationsDirty || outputDirty)
            {
                InitProgram();
            }

            Enabled = false;
            FLDebugger.Start(Container.Instance,
                Container.SerializedProgram.Initialize(Container.Instance, Container.InstructionSet));
            Enabled = true;
        }


        private void btnSetHomeDir_Click(object sender, EventArgs e)
        {
            if (fbdSelectHomeDir.ShowDialog() == DialogResult.OK)
            {
                Directory.SetCurrentDirectory(fbdSelectHomeDir.SelectedPath);
            }
        }


        private void btnSwitchDockSide_Click(object sender, EventArgs e)
        {
            if (panelToolbar.Dock == DockStyle.Left)
            {
                btnSwitchDockSide.Text = "Move to Left";
                panelToolbar.Dock = DockStyle.Right;
            }
            else if (panelToolbar.Dock == DockStyle.Right)
            {
                btnSwitchDockSide.Text = "Move to Right";
                panelToolbar.Dock = DockStyle.Left;
            }
        }

        private void lbOptimizations_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (sfdScript.ShowDialog() == DialogResult.OK)
            {
                Path = sfdScript.FileName;
                File.WriteAllText(Path, rtbIn.Text);
            }
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            logDisplay.Show();
        }


        private void btnPopOutBuildLog_Click(object sender, EventArgs e)
        {
            CreatePopOut(btnPopOutBuildLog, panelConsoleOut, rtbParserOutput, "Parser Build Output");
        }

        private void btnPopOutInput_Click(object sender, EventArgs e)
        {
            CreatePopOut(btnPopOutInput, panelInput, rtbIn, "FL Editor");
        }

        private void btnPopOutOutput_Click(object sender, EventArgs e)
        {
            CreatePopOut(btnPopOutOutput, panelOutput, rtbOut, "Parser Output");
        }

        private void CreatePopOut(Button button, Panel container, Control child, string name)
        {
            button.Visible = false;
            int originalHeight = container.Height;
            ContainerForm.CreateContainer(child, (control, args) =>
            {
                container.Height = originalHeight;
                button.Visible = true;
                button.BringToFront();
            }, name, Icon, FormBorderStyle.Sizable);
            container.Height = 0;
        }

        private void btnRestartDebugger_Click(object sender, EventArgs e)
        {
            string path = TempEditorContentPath + ".restart.fl";
            File.WriteAllText(path, rtbIn.Text);

            Process.Start(Assembly.GetExecutingAssembly().Location,
                $"-no-update {path} {Directory.GetCurrentDirectory()}");
            Application.Exit();
        }

        private void cbLiveView_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbLiveView.Checked)
            {
                previewForm?.Close();
                previewForm = null;
                previewPicture = null;
            }
            else
            {
                if (previewForm == null)
                {
                    previewPicture = new PictureBox();
                    previewPicture.Dock = DockStyle.Fill;
                    previewPicture.Image = Resources.OpenFL;
                    previewPicture.SizeMode = PictureBoxSizeMode.Zoom;
                    previewForm = ContainerForm.CreateContainer(previewPicture, null, "Preview: ",
                        Resources.OpenFL_Icon, FormBorderStyle.SizableToolWindow);
                    CheckForIllegalCrossThreadCalls = false;

                    InitProgram();
                    ComputePreview();
                }
            }
        }

        private void btnShowInstructions_Click(object sender, EventArgs e)
        {
            if (iv == null || iv.IsDisposed)
            {
                iv = new InstructionViewer(Container.InstructionSet);
            }

            iv.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            if (aboutForm == null || aboutForm.IsDisposed)
            {
                aboutForm = new AboutInfo();
            }

            aboutForm.Show();
        }

        private void cbBuildMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBuildMode.SelectedItem.ToString() == "Debug")
            {
                for (int i = 0; i < lbOptimizations.Items.Count; i++)
                {
                    object lbOptimizationsItem = lbOptimizations.Items[i];
                    FLProgramCheck pc = (FLProgramCheck)lbOptimizationsItem;
                    lbOptimizations.SetItemChecked(i, pc.CheckType == FLProgramCheckType.Validation);
                }
            }
            else
            {
                for (int i = 0; i < lbOptimizations.Items.Count; i++)
                {
                    object lbOptimizationsItem = lbOptimizations.Items[i];
                    lbOptimizations.SetItemChecked(i, true);
                }
            }
        }
    }
}