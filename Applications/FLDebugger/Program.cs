using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Byt3.AutoUpdate.Helper;
using Byt3.WindowsForms.CustomControls;
using FLDebugger.Forms;
using FLDebugger.Utils;

namespace FLDebugger
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            ProgressIndicator.OnCreate = ProgressIndicatorThemeHelper.ApplyTheme;

            if (UpdateChecker.Check(args, "http://213.109.162.193/flrepo/", "FLDebugger",
                Assembly.GetExecutingAssembly()))
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            string[] argss = args.Length > 0 && args[0] == "-no-update" ? args.Reverse().Take(Math.Max(args.Length - 1, 0)).Reverse().ToArray() : args;
            if (argss.Length == 1 && File.Exists(argss[0]) && (argss[0].EndsWith(".flc") || argss[0].EndsWith(".fl") || argss[0].EndsWith(".flres")))
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), argss[0]);
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
                argss = new[] {"-ss", "Debugger.ScriptPath:" + path};
            }
            new DebugConsole().Run(argss);

        }
    }
}