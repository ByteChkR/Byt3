using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Byt3.AutoUpdate.Helper;
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
            if (UpdateChecker.Check(args, "http://213.109.162.193/flrepo/", "FLDebugger",
                Assembly.GetExecutingAssembly()))
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            new DebugConsole().Run(args.Reverse().Take(Math.Max(args.Length - 1, 0)).Reverse().ToArray());
        }
    }
}