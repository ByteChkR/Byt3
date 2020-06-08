using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using Byt3.Utilities.FastString;

namespace Byt3.AutoUpdate.Helper
{
    public static class UpdateChecker
    {
        public static bool UpdaterPresent => File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Byt3.AutoUpdate.exe"));

        public static bool Check(string[] args, string url, string projectName, Assembly asm)
        {
            return Check(args, url, projectName, asm.Location, asm.GetName().Version);
        }


        public static bool Check(string[] args, string url, string projectName, string location, Version version)
        {
            if (UpdaterPresent && (args.Length == 0 || args[0] != "-no-update"))
            {
                string tempUpdater = Path.Combine(Path.GetTempPath(), "Byt3.AutoUpdate.exe");
                File.Copy("Byt3.AutoUpdate.exe", tempUpdater, true);
                string arg =
                    $"{url} {projectName} {version} {location} {Process.GetCurrentProcess().Id} -no-update {args.Unpack(" ")}";
                ProcessStartInfo psi = new ProcessStartInfo(tempUpdater, arg);
                Process.Start(psi);
                return true;
            }

            return false;
        }

        public static void Direct(string url, string projectName, string location, Version version,
            Version targetVersion)
        {
            string tempUpdater = Path.Combine(Path.GetTempPath(), "Byt3.AutoUpdate.exe");
            if(File.Exists(tempUpdater))File.Delete(tempUpdater);
            File.Copy("Byt3.AutoUpdate.exe", tempUpdater, true);
            string arg =
                $"-direct {url} {projectName} {version} {location} {Process.GetCurrentProcess().Id} {targetVersion} -no-update";
            ProcessStartInfo psi = new ProcessStartInfo(tempUpdater, arg);
            Process.Start(psi);
        }
    }
}