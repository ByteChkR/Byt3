using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Byt3.BuildSystem
{
    public static class ProcessRunner
    {
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static string ShellCommand =>
            IsWindows ? "cmd.exe" : "/bin/bash";

        private static string GetShellArgs(string command)
        {
            return IsWindows ? $" /C {command}" : $" -c \"{command}\"";
        }


        public static void RunCommand(CommandInfo commandInfo)
        {
            ProcessStartInfo info;
            if (commandInfo.UseShell)
            {
                info = new ProcessStartInfo(ShellCommand, GetShellArgs(commandInfo.Command));
            }
            else
            {
                info = new ProcessStartInfo(commandInfo.Command);
            }

            info.WorkingDirectory = commandInfo.WorkingDirectory;
            info.RedirectStandardOutput = commandInfo.CaptureConsoleOut;
            info.RedirectStandardError = commandInfo.CaptureConsoleOut;
            info.UseShellExecute = !commandInfo.CaptureConsoleOut;
            info.CreateNoWindow = !commandInfo.CreateWindow;
            Process p = Process.Start(info);
            p.EnableRaisingEvents = true;
            p.Exited += (sender, args) => ProcessExited(sender, args, p);

            if (commandInfo.CaptureConsoleOut)
            {
                if (commandInfo.OnOutputReceived != null)
                    p.OutputDataReceived += commandInfo.OnOutputReceived;
                if (commandInfo.OnErrorReceived != null)
                    p.ErrorDataReceived += commandInfo.OnErrorReceived;
            }

            p.BeginErrorReadLine();
            p.BeginOutputReadLine();

            if (!commandInfo.WaitForExit)
            {
                return;
            }

            if (commandInfo.WaitForExitTimeout == -1)
                p.WaitForExit();
            else
            {
                p.WaitForExit(commandInfo.WaitForExitTimeout);
                if (!p.HasExited)
                {
                    Console.WriteLine($"Command \"{commandInfo.Command}\" Timed Out");
                    p.Kill();
                    return;
                }
            }
        }

        private static void ProcessExited(object sender, EventArgs e, Process process)
        {
            process.CancelErrorRead();
            process.CancelOutputRead();
        }
    }
}