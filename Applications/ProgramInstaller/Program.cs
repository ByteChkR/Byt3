using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.AutoUpdate;
using Byt3.Utilities.FastString;

namespace ProgramInstaller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 0 && args[0] == "update")
            {
                AutoUpdateEntry.Main(args.Reverse().Take(args.Length - 1).Reverse().ToArray());
                return;
            }

            if (args.Length == 0 || args[0] != "-no-update")
            {
                string tmpFile = Path.Combine(Path.GetTempPath(),
                    Path.GetFileName(Assembly.GetEntryAssembly().Location));
                File.Delete(tmpFile);
                File.Copy(Assembly.GetEntryAssembly().Location,tmpFile );
                string arg =
                    $"update http://213.109.162.193/flrepo/ ProgramInstaller {Assembly.GetEntryAssembly().GetName().Version} {new Uri(Assembly.GetEntryAssembly().CodeBase).AbsolutePath} {Process.GetCurrentProcess().Id} -no-update {args.Unpack(" ")}";
                ProcessStartInfo psi = new ProcessStartInfo(tmpFile, arg);
                Process.Start(psi);
                return;
            }

            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SelectProductForm());
        }
    }
}
