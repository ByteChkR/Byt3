using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Byt3.AutoUpdate.Helper;
using FLDebugger.Projects.Forms;

namespace FLDebugger.Projects
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            if (UpdateChecker.Check(args, "http://213.109.162.193/flrepo/", "FLDebugger_Projects",
                Assembly.GetExecutingAssembly()))
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string[] arg = args.Length > 0 && args[0] == "-no-update"
                ? args.Reverse().Take(Math.Max(args.Length - 1, 0)).Reverse().ToArray()
                : args;
            Application.Run(new FLProjectExplorer(arg));
        }
    }
}