using Byt3.CommandRunner;

namespace Byt3.OpenFL.CLI.Commands
{
    public class SetOutputFilesCommand : AbstractCommand
    {
        internal static string[] OutputFiles { get; set; } = new string[0];


        public SetOutputFilesCommand() : base(SetOutputFiles,new[] {"--output", "-o"},
            "Sets the Output for the Files specified as input")
        {
        }


        private static void SetOutputFiles(StartupArgumentInfo info, string[] args)
        {
            OutputFiles = args;
        }

    }
}