using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;
using Byt3.Utilities.FastString;

namespace Byt3.AutoUpdate.Helper
{
    public static class UpdateChecker
    {

        public static bool Check(string[] args, string url, string projectName, Assembly asm)
        {
            if (File.Exists("Byt3.AutoUpdate.exe") && (args.Length == 0 || args[0] != "-no-update"))
            {
                string tempUpdater = Path.Combine(Path.GetTempPath(), "Byt3.AutoUpdate.exe");
                File.Copy("Byt3.AutoUpdate.exe", tempUpdater, true);
                string arg =
                    $"{url} {projectName} {asm.GetName().Version} {asm.Location} {Process.GetCurrentProcess().Id} -no-update {args.Unpack(" ")}";
                ProcessStartInfo psi = new ProcessStartInfo(tempUpdater, arg);
                Process.Start(psi);
                return true;
            }

            return false;
        }


    }
}
