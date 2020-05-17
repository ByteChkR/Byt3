using System;
using System.IO;
using Byt3.ADL;
using Byt3.CommandRunner;

namespace Byt3Console.VersionHelper.Commands
{
    public class ChangeVersionCommand : AbstractCommand
    {
        public ChangeVersionCommand() : base(new[] {"--increase", "-i"},
            "Increases the last number in the version string 0.0.0.1 => 0.0.0.2", true)
        {
            CommandAction = (info, strings) => ChangeVersion(strings);
        }

        private void ChangeVersion(string[] arg2)
        {
            string file = arg2[0];
            string versionChangeStr = "X.X.X.+";
            if (arg2.Length != 1)
            {
                versionChangeStr = arg2[1];
            }

            Version v = VersionHelperConsole.GetVersionFromFile(file);
            Version newV = VersionHelperConsole.ChangeVersion(v, versionChangeStr);

            Logger.Log(LogType.Log, $"Changing Version {v} => {newV}", 1);

            if (ToFileCommand.File != null)
            {
                File.WriteAllText(ToFileCommand.File, newV.ToString());
            }

            VersionHelperConsole.ChangeVersionInFile(file, newV);
        }
    }
}