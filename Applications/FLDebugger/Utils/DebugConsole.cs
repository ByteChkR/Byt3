using System;
using System.Windows.Forms;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.ManifestIO;
using Byt3.WindowsForms.Forms;
using FLDebugger.Forms;

namespace FLDebugger.Utils
{
    public class DebugConsole : AConsole
    {
        public override string ConsoleKey => "fldbg";
        public override string ConsoleTitle => "Open FL Debugger GUI";
        public override bool Run(string[] args)
        {

            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            InternalADLProjectDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            ManifestIODebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;

            InnerRun(args);


            return true;
        }

        private FLScriptEditor GetEditor(string[] args)
        {
            FLScriptEditor ed = null;
            if (args.Length == 1)
            {
                ed = new FLScriptEditor(args[0]);
            }
            else if (args.Length == 2)
            {
                ed = new FLScriptEditor(args[0], args[1]);
            }
            else
            {
                ed = new FLScriptEditor();
            }

            return ed;
        }

        private void InnerRun(string[] args)
        {
            try
            {
                Application.Run(GetEditor(args));
            }
            catch (Exception e)
            {
                ExceptionViewer ev = new ExceptionViewer(e);
                if (ev.ShowDialog() == DialogResult.Retry)
                {
                    InnerRunWarm(args);
                }
            }
        }

        private void InnerRunWarm(string[] args)
        {
            try
            {
                for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                {
                    Form openForm = Application.OpenForms[i];
                    openForm.Close();
                }

                GetEditor(args).ShowDialog(); //Show as dialog to have a blocking call so we do not leave the try catch block
            }
            catch (Exception e)
            {
                ExceptionViewer ev = new ExceptionViewer(e);
                if (ev.ShowDialog() == DialogResult.Retry)
                {
                    InnerRunWarm(args);
                }
            }
        }
    }
}