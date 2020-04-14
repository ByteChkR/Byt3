using Byt3.CommandRunner;

namespace Byt3.OpenFL.CLI.Commands
{
    public class SetOutputFilesCommand : AbstractCommand
    {
        internal static string[] OutputFiles=new string[0];


        public SetOutputFilesCommand() : base(new[] {"--output", "-o"},
            "Sets the Output for the Files specified as input")
        {
            CommandAction = SetOutputFiles;
        }


        private void SetOutputFiles(StartupArgumentInfo info, string[] args)
        {
            OutputFiles = args;
        }

    }
}