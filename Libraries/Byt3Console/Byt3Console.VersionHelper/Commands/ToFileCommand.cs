using System.IO;
using Byt3.CommandRunner;

namespace Byt3Console.VersionHelper.Commands
{
    public class ToFileCommand : AbstractCommand
    {
        public static string File;

        public ToFileCommand() : base(new[] {"--to-file", "-2file"}, "Writes the new version into a file.")
        {
            CommandAction = (info, strings) => NoWrapFlag(strings);
        }

        private void NoWrapFlag(string[] args)
        {
            File = Path.GetFullPath(args[0]);
        }
    }
}