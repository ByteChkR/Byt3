﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.ADL;
using Byt3.ADL.Streams;
using Byt3.Callbacks;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Parsing.StageResults;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.Utilities.FastString;
using Byt3.Utilities.ManifestIO;
using Byt3.Utilities.TypeFinding;
using Byt3.WindowsForms.CustomControls;
using Byt3.WindowsForms.Forms;
using FLDebugger.Properties;
using FLDebugger.Utils;

namespace FLDebugger.Forms
{
    public partial class FLScriptEditor : Form
    {
        internal static FLDebuggerSettings Settings = new FLDebuggerSettings();

        public static FLEditorTheme Theme = new FLEditorTheme();

        private static readonly string TempEditorContentPath = System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "tempeditorcontent_" + System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) +
            ".fl");

        private static readonly string DEFAULT_SCRIPT =
            $"{FLKeywords.EntryFunctionKey}:\n\tsetactive 3\n\tSet_v 1\n\tsetactive 0 1 2\n\tSet_v 1";


        private readonly Stream ms = new PipeStream();

        private string _path;


        private AboutInfo aboutForm;

        private FLDataContainer Container;

        private bool ControlMod;

        private bool ignoreChanged;
        private InstructionViewer iv;
        private LogDisplay logDisplay;
        private bool optimizationsDirty;
        private bool outputDirty = true;

        private ContainerForm previewForm;

        private PictureBox previewPicture;


        private Task previewTask;


        private TextReader tr;

        public FLScriptEditor()
        {
            InitializeComponent();

            RegisterDefaultTheme(panelInput);
            RegisterDefaultTheme(panelToolbar);
            RegisterDefaultTheme(btnOpen);
            RegisterCodeTheme(rtbIn);
            RegisterCodeTheme(rtbOut);
            RegisterDefaultTheme(panelConsoleOut);
            RegisterDefaultTheme(panelCodeArea);
            RegisterDefaultTheme(panelOutput);
            RegisterLogTheme(rtbParserOutput);
            RegisterDefaultTheme(lbOptimizations);
            RegisterDefaultTheme(btnUpdate);
            RegisterDefaultTheme(btnDebug);
            RegisterDefaultTheme(btnSwitchDockSide);
            RegisterDefaultTheme(btnSave);
            RegisterDefaultTheme(btnShowLog);
            RegisterDefaultTheme(btnPopOutBuildLog);
            RegisterDefaultTheme(btnPopOutInput);
            RegisterDefaultTheme(btnPopOutOutput);
            RegisterDefaultTheme(cbLiveView);
            RegisterDefaultTheme(cbAutoBuild);
            RegisterDefaultTheme(btnShowInstructions);
            RegisterDefaultTheme(lblBuildMode);
            RegisterDefaultTheme(cbBuildMode);
            RegisterDefaultTheme(btnSettings);
            RegisterDefaultTheme(btnClear);

            lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            rtbIn.WriteSource(DEFAULT_SCRIPT);
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

        internal static string ConfigPath => System.IO.Path.Combine(Application.StartupPath, "configs", "fl_editor");

        private string Path
        {
            get
            {
                if (_path == null)
                {
                    _path = TempEditorContentPath;
                }

                return _path;
            }
            set => _path = value;
        }

        private static void DefaultOnThemeChange(FLEditorTheme theme, Control c)
        {
            c.BackColor = theme.PrimaryBackgroundColor;
            c.ForeColor = theme.PrimaryFontColor;
        }


        private static void CodeTextOnThemeChange(FLEditorTheme theme, RichTextBox c)
        {
            c.Font = new Font(c.Font.FontFamily, theme.CodeFontSize, c.Font.Style);
        }

        private static void LogTextOnThemeChange(FLEditorTheme theme, RichTextBox c)
        {
            c.Font = new Font(c.Font.FontFamily, theme.LogFontSize, c.Font.Style);
        }


        private static void PreviewOnThemeChange(FLEditorTheme theme, PictureBox c)
        {
            c.BackColor = theme.BufferViewBackgroundColor;
        }


        public static void RegisterDefaultTheme(Control c)
        {
            Action<FLEditorTheme> a = theme => DefaultOnThemeChange(theme, c);
            Theme.Register(a);
        }

        public static void RegisterCodeTheme(RichTextBox c)
        {
            Action<FLEditorTheme> a = theme => DefaultOnThemeChange(theme, c);
            a += theme => CodeTextOnThemeChange(theme, c);
            Theme.Register(a);
        }

        public static void RegisterLogTheme(RichTextBox c)
        {
            Action<FLEditorTheme> a = theme => DefaultOnThemeChange(theme, c);
            a += theme => LogTextOnThemeChange(theme, c);
            Theme.Register(a);
        }

        public static void RegisterPreviewTheme(PictureBox c)
        {
            Action<FLEditorTheme> a = theme => DefaultOnThemeChange(theme, c);
            a += theme => PreviewOnThemeChange(theme, c);
            Theme.Register(a);
        }


        public void SetResolution(int width, int height, int depth)
        {
            Settings.ResX = width;
            Settings.ResY = height;
            Settings.ResZ = depth;
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
                new Task<SerializableFLProgram>(() =>
                    Container.Parser.Process(new FLParserInput(Path, cbBuildMode.SelectedItem.ToString().ToUpper())));
            return loadT;
        }

        private Exception GetInnerIfAggregate(Exception ex)
        {
            return ex is AggregateException ag ? GetInnerIfAggregate(ag.InnerExceptions.First()) : ex;
        }


        public void UnpackResources()
        {
            ProgressBar pb = new ProgressBar();
            pb.Dock = DockStyle.Fill;
            Size size = new Size(50, 200);

            Enabled = false;
            ContainerForm loading = ContainerForm.CreateContainer(pb, (control, args) =>
            {
                pb.Dispose();
                Enabled = true;
            }, "Unpacking Kernels...", Resources.OpenFL_Icon, FormBorderStyle.None, size, size);

            Task t = new Task(() =>
            {
                string workingDir = Settings.WorkingDir ?? Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(Application.StartupPath);
                string[] files = IOManager.GetFiles("resources");
                pb.Minimum = 0;
                pb.Value = 0;
                pb.Maximum = files.Length;

                foreach (string file in files)
                {
                    string dir = System.IO.Path.GetDirectoryName(file);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    if (!File.Exists(file))
                    {
                        Stream s = IOManager.GetStream(file);
                        Stream dst = File.Create(file);
                        s.CopyTo(dst);
                        s.Dispose();
                        dst.Dispose();
                    }

                    pb.Value++;
                }

                Directory.SetCurrentDirectory(workingDir);
                loading.Close();
                loading.Dispose();
            });
            t.Start();
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

                Container.SerializedProgram = null;

                string s = $"Errors: {loadT.Exception.InnerExceptions.Count}\n";

                foreach (Exception exceptionInnerException in loadT.Exception.InnerExceptions)
                {
                    Exception ex = GetInnerIfAggregate(exceptionInnerException);
                    s += $"PARSE ERROR:\n\t{ex.Message}\n";
                    if (Settings.LogParserStacktrace)
                    {
                        s += $"\t\t{ex.StackTrace.Split('\n').Unpack("\n\t\t")}\n";
                    }
                }

                SetLogOutput(s, Theme.ErrorColor);
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
                SetLogOutput("Build Succeeded.\n", Theme.SuccessColor);
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

            Container = new FLDataContainer(Settings.KernelPath);


            List<FLProgramCheck> types = typeof(OpenFLDebugConfig).Assembly.GetExportedTypes()
                .Where(x => typeof(FLProgramCheck).IsAssignableFrom(x) && !x.IsAbstract && x != typeof(FLProgramCheck))
                .Select(x => (FLProgramCheck) Activator.CreateInstance(x)).ToList();

            types.Sort((x, y) => x.Priority.CompareTo(y.Priority));

            lbOptimizations.ItemCheck += LbOptimizationsOnItemCheck;
            rtbParserOutput.TextChanged += RtbParserOutputTextChanged;
            for (int i = 0; i < types.Count; i++)
            {
                FLProgramCheck type = types[i];
                lbOptimizations.Items.Add(type);
            }

            cbBuildMode.Items.AddRange(
                Enum.GetNames(typeof(FLProgramCheckType)).Select(x => x.Trim('[', ']')).ToArray());
            cbBuildMode.SelectedIndex = 0;
        }

        private void FLScriptEditor_Closing(object sender, CancelEventArgs e)
        {
            if (File.Exists(TempEditorContentPath))
            {
                File.Delete(TempEditorContentPath);
            }

            Application.Exit();
        }

        private void SetLogOutput(string r, Color color)
        {
            string pr = "";
            pr += $"Parse Result: {Path}\n";
            pr += r;
            pr += "____________________________________________________\n";
            rtbParserOutput.AppendText(pr, color, rtbParserOutput.BackColor);
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
            rtbParserOutput.ScrollToCaret();
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

                        pro.Run(
                            new FLBuffer(Container.Instance, Settings.ResX, Settings.ResY, Settings.ResZ,
                                "Preview Buffer"), true);
                        if (previewPicture != null)
                        {
                            Bitmap bmp = new Bitmap(Settings.ResX, Settings.ResY);
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

                        throw ex;
                    }
                });
                Task t = new Task(() =>
                {
                    while (!previewTask.IsCompleted)
                    {
                        Thread.Sleep(100);
                    }

                    if (previewTask.IsFaulted)
                    {
                        string s = $"Errors: {previewTask.Exception.InnerExceptions.Count}\n";

                        foreach (Exception exceptionInnerException in previewTask.Exception.InnerExceptions)
                        {
                            Exception ex = GetInnerIfAggregate(exceptionInnerException);
                            s +=
                                $"PROGRAM ERROR:\n\t{ex.Message}\n";
                            if (Settings.LogProgramStacktrace)
                            {
                                s += $"\t\t{ex.StackTrace.Split('\n').Unpack("\n\t\t")}\n";
                            }
                        }

                        SetLogOutput(s, Theme.ErrorColor);
                    }
                });
                previewTask.Start();
                t.Start();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            InitProgram();
            ComputePreview();
        }

        private void InitProgram()
        {
            File.WriteAllText(Path, rtbIn.Text);
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
            //Path = "";
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

            if (Container.SerializedProgram == null)
            {
                return;
            }

            Enabled = false;
            FLDebugger.Start(Container.Instance,
                Container.SerializedProgram.Initialize(Container.Instance, Container.InstructionSet), Settings.ResX,
                Settings.ResY, Settings.ResZ);
            Enabled = true;
        }


        public void SetWorkingDirectory()
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

        public void ReloadKernels()
        {
            foreach (Form openForm in Application.OpenForms.Cast<Form>().ToList())
            {
                if (openForm != this && openForm != logDisplay)
                {
                    openForm.Close();
                    openForm.Dispose();
                }
            }

            Hide();
            Container.Dispose();
            string workingDir = Settings.WorkingDir ?? Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Application.StartupPath);
            Container = new FLDataContainer(Settings.KernelPath);
            Directory.SetCurrentDirectory(workingDir);
            Show();
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


                    RegisterPreviewTheme(previewPicture);

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

        public void ShowAbout()
        {
            if (aboutForm == null || aboutForm.IsDisposed)
            {
                aboutForm = new AboutInfo();
            }

            aboutForm.Show();
        }

        private void cbBuildMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lbOptimizations.Items.Count; i++)
            {
                object lbOptimizationsItem = lbOptimizations.Items[i];
                FLProgramCheck pc = (FLProgramCheck) lbOptimizationsItem;
                lbOptimizations.SetItemChecked(i,
                    (pc.CheckType & (FLProgramCheckType) Enum.Parse(typeof(FLProgramCheckType),
                         cbBuildMode.SelectedItem.ToString())) != 0);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsDialog diag = new SettingsDialog(this);
            diag.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbParserOutput.Text = "";
        }

        public class FLDebuggerSettings
        {
            public string KernelPath = "resources/kernel";
            public bool LogParserStacktrace;
            public bool LogProgramStacktrace;
            public int ResX = 512;
            public int ResY = 512;
            public int ResZ = 1;
            public string ScriptPath;
            public string Theme;
            public string WorkingDir;
        }
    }
}