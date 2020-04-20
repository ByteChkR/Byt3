using Byt3.CommandRunner;

namespace Byt3.AssemblyGenerator.Console.Commands
{
    public class SetOutputFolderCommand : AbstractCommand
    {
        public SetOutputFolderCommand() : base((info, strings) => SetOutputFolder(strings),
            new[] { "--set-output", "-o" },
            "Specifies the Output Folder of the Compiled Assembly.(Default is CurrentDirectory + Assembly Name")
        {
        }

        private static void SetOutputFolder(string[] args)
        {
            ConsoleEntry.Output = args[0];
        }
    }
}
