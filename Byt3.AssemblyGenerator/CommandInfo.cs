using System.Diagnostics;

namespace Byt3.BuildSystem
{
    public class CommandInfo
    {
        public string Command;
        public string WorkingDirectory = ".\\";
        public bool WaitForExit = true;
        public int WaitForExitTimeout = -1;
        public bool CreateWindow = false;
        public bool UseShell = false;
        public bool CaptureConsoleOut = false;
        public DataReceivedEventHandler OnErrorReceived = null;
        public DataReceivedEventHandler OnOutputReceived = null;

        public CommandInfo(string command, string workingDirectory = ".\\", bool useShell = false)
        {
            UseShell = useShell;
            WorkingDirectory = workingDirectory;
            Command = command;
        }
    }
}