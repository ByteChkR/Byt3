using System;
using Byt3.ADL;
using Byt3.CommandRunner;

namespace Byt3.VersionHelper.Console.Commands
{
    public class ChangeVersionCommand : AbstractCommand
    {
        public ChangeVersionCommand() : base(new[] {"--increase", "-i"},
            "Increases the last number in the version string 0.0.0.1 => 0.0.0.2", true)
        {
            CommandAction = ChangeVersion;
        }

        private void ChangeVersion(StartupArgumentInfo arg1, string[] arg2)
        {
            string file = arg2[0];
            string versionChangeStr = "X.X.X.+";
            if (arg2.Length != 1)
            {
                versionChangeStr = arg2[1];
            }

            Version v = ConsoleEntry.GetVersionFromFile(file);
            Version newV = ConsoleEntry.ChangeVersion(v, versionChangeStr);

            Logger.Log(LogType.Log, $"Changing Version {v} => {newV}", 1);

            ConsoleEntry.ChangeVersionInFile(file, newV);
        }
    }
}