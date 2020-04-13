using System.Diagnostics;

namespace Byt3.Utilities.Threading
{
    public class CommandInfo
    {
        public string Command { get; set; }
        public string WorkingDirectory { get; set; } = ".\\";
        public bool WaitForExit { get; set; } = true;
        public int WaitForExitTimeout { get; set; } = -1;
        public bool CreateWindow { get; set; } = false;
        public bool UseShell { get; set; } = false;
        public bool CaptureConsoleOut { get; set; } = false;
        public DataReceivedEventHandler OnErrorReceived { get; set; } = null;
        public DataReceivedEventHandler OnOutputReceived { get; set; } = null;

        public CommandInfo(string command, string workingDirectory = ".\\", bool useShell = false)
        {
            UseShell = useShell;
            WorkingDirectory = workingDirectory;
            Command = command;
        }
    }
}