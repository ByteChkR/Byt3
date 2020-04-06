using System;
using System.Diagnostics;
using System.IO;
using Byt3.Utilities.Threading;

namespace Byt3.Utilities.DotNet
{
    public static class DotNetHelper
    {
        public static void DotnetAction(string msbuildCommand, string targetCommand, string arguments, string workingDir)
        {
            CommandInfo info =
                new CommandInfo($"{msbuildCommand} {targetCommand} {arguments}", workingDir, true);
            info.CreateWindow = false;
            info.CaptureConsoleOut = true;
            info.OnErrorReceived = OnErrorReceived;
            info.OnOutputReceived = OnOutReceived;
            ProcessRunner.RunCommand(info);
        }

        public static void New(string msbuildCommand, string workingDir, string projectName, bool lib = true)
        {
            string arguments = $"{(lib ? "classlib" : "console")} -n {projectName}";
            DotnetAction(msbuildCommand, "new", arguments, workingDir);
        }

        public static string BuildProject(string msbuildCommand, string projectFile, AssemblyDefinition definitions, bool lib = true)
        {
            Console.WriteLine("Building Assembly: " + definitions.AssemblyName);
            string arguments = $"-c {definitions.BuildConfiguration}";
            if (!definitions.NoTargetRuntime)
            {
                arguments = $"--runtime {definitions.BuildTargetRuntime} {arguments}";
            }

            string workingDir = Path.GetDirectoryName(projectFile);
            DotnetAction(msbuildCommand, "build", arguments, workingDir);
            string ret = Path.Combine(workingDir, "bin", definitions.BuildConfiguration, lib? "netstandard2.0":"netcoreapp2.2");
            if (!definitions.NoTargetRuntime)
            {
                ret = Path.Combine(ret, definitions.BuildTargetRuntime);
            }

            return ret;
        }

        public static string PublishProject(string msbuildCommand, string projectFile, AssemblyDefinition definitions, bool lib = true)
        {
            Console.WriteLine("Publishing Assembly: " + definitions.AssemblyName);
            string arguments = $"-c {definitions.BuildConfiguration}";
            if (!definitions.NoTargetRuntime)
            {
                arguments = $"--runtime {definitions.BuildTargetRuntime} {arguments}";
            }
            string workingDir = Path.GetDirectoryName(projectFile);
            DotnetAction(msbuildCommand, "publish", arguments, workingDir);
            string ret = Path.Combine(workingDir, "bin", definitions.BuildConfiguration, lib ? "netstandard2.0" : "netcoreapp2.2");
            if (!definitions.NoTargetRuntime)
            {
                ret = Path.Combine(ret, definitions.BuildTargetRuntime);
            }

            ret = Path.Combine(ret, "publish");
            return ret;
        }


        private static void OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            Console.WriteLine("\t[ERR]" + e.Data);

        }

        private static void OnOutReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            Console.WriteLine("\t[OUT]" + e.Data);

        }

    }
}