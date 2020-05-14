using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Byt3.AutoUpdate.Helper;

namespace FLDebugger.Projects
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (UpdateChecker.Check(args, "http://213.109.162.193/flrepo/", "FLDebugger_Projects", Assembly.GetExecutingAssembly()))
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FLProjectExplorer(args.Reverse().Take(Math.Max(args.Length - 1, 0)).Reverse().ToArray()));
        }
    }




}
