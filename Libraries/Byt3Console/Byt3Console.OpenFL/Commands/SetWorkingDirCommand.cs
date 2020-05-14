using System.IO;
using Byt3.CommandRunner;

namespace Byt3Console.OpenFL.Commands
{
    public class SetWorkingDirCommand : AbstractCommand
    {
        public SetWorkingDirCommand() : base(new[] { "--set-working-dir", "-wd" },
            "Can be used to set the working directory before execution")
        {
            CommandAction = (info, strings) => SetWorkingDir(strings);
        }

        private static void SetWorkingDir(string[] arg2)
        {
            Directory.SetCurrentDirectory(arg2[0]);
        }
    }
}