using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.ADL;
using Byt3.ADL.Streams;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.Utilities.ManifestIO;
using Byt3.Utilities.TypeFinding;

namespace FLDebugger
{
    public partial class frmOptimizationView : Form
    {
        private FLParser p;
        private TextReader tr;
        private Stream ms = new PipeStream();
        private KernelDatabase db;
        private FLProgramCheckBuilder builder;
        private FLInstructionSet instructionSet;
        private SerializableFLProgram prog;
        private BufferCreator bufferCreator;
        private string Path;
        private bool optimizationsDirty;

        private const string DEFAULT_SCRIPT = "Main:\n\tsetactive 3\n\tsetv 1\n\tsetactive 0 1 2\n\tsetv 1";


        private bool ControlMod;

        public frmOptimizationView()
        {
            InitializeComponent();
            rtbIn.WriteSource(DEFAULT_SCRIPT);
        }

        public frmOptimizationView(string path):this()
        {

            Path = path;
            rtbIn.WriteSource(File.ReadAllText(Path));

        }


        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFLScript.ShowDialog() == DialogResult.OK)
            {
                Path = openFLScript.FileName;
                rtbIn.Text = File.ReadAllText(Path);
            }
        }

        private bool ignoreChanged = false;


        private static void MakeResizable(Control control, ResizeableControl.EdgeEnum activeEdges, params Control[] dockedChildren)
        {
           ResizeableControl ret= new ResizeableControl(control, dockedChildren);
           ret.DrawOutline = true;
           ret.OutlineColor = CodeViewHelper.SourceBackColor;
           ret.Edges = activeEdges;
        }

        private void InitializeViewer()
        {
            if (builder.IsAttached)
            {
                builder.Detach(false);
            }
            builder = new FLProgramCheckBuilder(instructionSet, bufferCreator);
            List<string> si = lbOptimizations.CheckedItems.Cast<string>().ToList();
            si.Select(x =>
                (FLProgramCheck)Activator.CreateInstance(TypeAccumulator<FLProgramCheck>.GetTypesByName(x)
                    .First())).ToList().ForEach(x => builder.AddProgramCheck(x));
            builder.Attach(p, true);



            Text = "FL Parse Output Viewing: " + Path;

            string source = File.ReadAllText(Path);

            Task<SerializableFLProgram> loadT = new Task<SerializableFLProgram>(() => p.Process(new FLParserInput(Path)));
            loadT.Start();

            while (!loadT.IsCompleted)
            {
                Application.DoEvents();
            }

            if (loadT.IsFaulted)
            {
                rtbOut.Text = "PARSE ERROR:\n" + loadT.Exception.InnerException;
            }
            else
            {
                prog = loadT.Result;
                string s = prog.ToString();
                if (s.Count(x => x == '\n') > 100)
                {
                    rtbOut.Text = s;
                    return;
                }
                rtbOut.WriteSource(s);
            }
        }

        private void rtbIn_TextChanged(object sender, EventArgs e)
        {
            if (ignoreChanged) return;
            if (tmrConsoleColors.Enabled) tmrConsoleColors.Stop();
            tmrConsoleColors.Start();
        }

        private string logOut = "";
        private bool allowLogUpdate = false;
        private void tmrConsoleRefresh_Tick(object sender, EventArgs e)
        {
            string r = tr.ReadToEnd();
            logOut += r;
            allowLogUpdate = true;
            rtbLogOut.AppendText(r);
            allowLogUpdate = false;
        }

        private void frmOptimizationView_Load(object sender, EventArgs e)
        {
            lblFLVersion.Text = "OpenFL Versions:";
            lblFLVersion.Text += "\n   Parser: " + typeof(FLParser).Assembly.GetName().Version;
            lblFLVersion.Text += "\n   Common: " + OpenFLDebugConfig.CommonVersion;

            FLDebugger.Initialize();
            DoubleBuffered = true;
            Closing += FrmOptimizationView_Closing;


            //MakeResizable(panelCodeArea, ResizeableControl.EdgeEnum.None);
            //MakeResizable(panelToolbar, ResizeableControl.EdgeEnum.Left|ResizeableControl.EdgeEnum.Right);
            MakeResizable(panelConsoleOut, ResizeableControl.EdgeEnum.Top, rtbLogOut);
            MakeResizable(panelInput, ResizeableControl.EdgeEnum.Right, rtbIn);


            panelCodeArea.Resize += PanelCodeArea_Resize;

            lbOptimizations.KeyDown += OnKeyDown;
            lbOptimizations.KeyUp += OnKeyUp;

            tmrConsoleRefresh.Start();


            LogTextStream ls = new LogTextStream(ms, false);
            Debug.DefaultInitialization();
            Debug.AddOutputStream(ls);
            tr = new StreamReader(ms);
            TypeAccumulator.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly);
            ManifestReader.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly);
            ManifestReader.PrepareManifestFiles(false);
            ManifestReader.PrepareManifestFiles(true);
            EmbeddedFileIOManager.Initialize();

            db = new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            instructionSet = FLInstructionSet.CreateWithBuiltInTypes(db);
            bufferCreator = BufferCreator.CreateWithBuiltInTypes();


            p = new FLParser(instructionSet, bufferCreator, new WorkItemRunnerSettings(true, 2));



            builder = new FLProgramCheckBuilder(instructionSet, bufferCreator);
            IEnumerable<string> types = typeof(OpenFLDebugConfig).Assembly.GetExportedTypes()
                .Where(x => typeof(FLProgramCheck).IsAssignableFrom(x) && !x.IsAbstract && x != typeof(FLProgramCheck))
                .Select(x => x.Name);
            lbOptimizations.ItemCheck += LbOptimizationsOnItemCheck;
            rtbLogOut.TextChanged += RtbLogOut_TextChanged;
            foreach (string type in types)
            {
                lbOptimizations.Items.Add(type);
            }
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


        private void FrmOptimizationView_Closing(object sender, CancelEventArgs e)
        {
            Application.Exit();
        }

        private void PanelCodeArea_Resize(object sender, EventArgs e)
        {
            panelInput.Width = panelOutput.Width = panelCodeArea.Width / 2;
        }

        private void RtbLogOut_TextChanged(object sender, EventArgs e)
        {
            if (allowLogUpdate)
            {
                rtbLogOut.ScrollToCaret();
            }
            else
            {
                rtbLogOut.Text = logOut;
            }
        }

        private void LbOptimizationsOnItemCheck(object sender, ItemCheckEventArgs e)
        {
            optimizationsDirty = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            InitProgram();
        }

        private void InitProgram()
        {
            if (string.IsNullOrEmpty(Path))
            {
                string path = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "tempeditorcontent.fl");
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

        }

        private void btnDebug_Click(object sender, EventArgs e)
        {

            if (prog == null || optimizationsDirty)
            {
                InitProgram();
            }

            FLDebugger.Start(prog.Initialize(instructionSet));
        }


        private void btnSetHomeDir_Click(object sender, EventArgs e)
        {
            if (fbdSelectHomeDir.ShowDialog() == DialogResult.OK)
            {
                Directory.SetCurrentDirectory(fbdSelectHomeDir.SelectedPath);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
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

        private int previousClickedOptimization = -1;

        private void lbOptimizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ControlMod && previousClickedOptimization != -1 && lbOptimizations.SelectedIndex != -1 && previousClickedOptimization != lbOptimizations.SelectedIndex)
            {
                object objold = lbOptimizations.Items[previousClickedOptimization];
                lbOptimizations.Items[previousClickedOptimization] =
                    lbOptimizations.Items[lbOptimizations.SelectedIndex];
                lbOptimizations.Items[lbOptimizations.SelectedIndex] = objold;
            }

            previousClickedOptimization = lbOptimizations.SelectedIndex;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (sfdScript.ShowDialog() == DialogResult.OK)
            {
                Path = sfdScript.FileName;
                File.WriteAllText(Path,rtbIn.Text);
            }
        }
    }

}
