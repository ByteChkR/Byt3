using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.CommandRunner;
using Byt3.CommandRunner.SetSettings;
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

            Runner r = new Runner();
            SetSettingsCommand ss = SetSettingsCommand.CreateSettingsCommand("Debugger",
                new[] {"--set-settings", "-ss"}, FLScriptEditor.Settings);
            r._AddCommand(ss);
            r._RunCommands(args);

            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "configs", "fl_editor", "themes"));

            if (FLScriptEditor.Settings.Theme == null)
            {
                if (!File.Exists(Path.Combine(Application.StartupPath, "configs", "fl_editor", "last_theme.txt")))
                {
                    FLScriptEditor.Settings.Theme = Path.Combine(Application.StartupPath, "configs", "fl_editor",
                        "themes", "default.xml");
                }
                else
                {
                    FLScriptEditor.Settings.Theme = Path.Combine(Application.StartupPath, "configs", "fl_editor",
                        "themes",
                        File.ReadAllText(
                            Path.Combine(Application.StartupPath, "configs", "fl_editor", "last_theme.txt")));
                }
            }

            string themePath = null;

            themePath = FLScriptEditor.Settings.Theme;

            if (themePath != null && File.Exists(themePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(FLEditorTheme));
                Stream s = File.OpenRead(themePath);
                FLScriptEditor.Theme = (FLEditorTheme) xs.Deserialize(s);
                s.Close();
            }

            InnerRun();

            return true;
        }

        private Form GetEditor()
        {
            Form ed = null;


            if (string.IsNullOrEmpty(FLScriptEditor.Settings.WorkingDir) &&
                !string.IsNullOrEmpty(FLScriptEditor.Settings.ScriptPath))
            {
                if (FLScriptEditor.Settings.ScriptPath.EndsWith(".flc"))
                {
                    ed = new ExportViewer(FLScriptEditor.Settings.ScriptPath);
                }
                else
                {
                    ed = new FLScriptEditor(FLScriptEditor.Settings.ScriptPath);
                }
            }
            else if (!string.IsNullOrEmpty(FLScriptEditor.Settings.ScriptPath) &&
                     !string.IsNullOrEmpty(FLScriptEditor.Settings.WorkingDir))
            {
                if (FLScriptEditor.Settings.ScriptPath.EndsWith(".flc"))
                {
                    ed = new ExportViewer(FLScriptEditor.Settings.ScriptPath);
                }
                else
                {
                    ed = new FLScriptEditor(FLScriptEditor.Settings.ScriptPath, FLScriptEditor.Settings.WorkingDir);
                }
            }
            else
            {
                ed = new FLScriptEditor();
            }

            return ed;
        }

        private void InnerRun()
        {
            Application.Run(GetEditor());
            try
            {
            }
            catch (Exception e)
            {
                ExceptionViewer ev = new ExceptionViewer(e);
                if (ev.ShowDialog() == DialogResult.Retry)
                {
                    InnerRunWarm();
                }
            }
        }

        private void InnerRunWarm()
        {
            try
            {
                for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                {
                    Form openForm = Application.OpenForms[i];
                    openForm.Close();
                }

                GetEditor()
                    .ShowDialog(); //Show as dialog to have a blocking call so we do not leave the try catch block
            }
            catch (Exception e)
            {
                ExceptionViewer ev = new ExceptionViewer(e);
                if (ev.ShowDialog() == DialogResult.Retry)
                {
                    InnerRunWarm();
                }
            }
        }
    }
}