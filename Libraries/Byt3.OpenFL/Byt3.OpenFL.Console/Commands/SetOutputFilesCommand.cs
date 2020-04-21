using Byt3.CommandRunner;

namespace Byt3.OpenFL.Console.Commands
{
    public class SetOutputFilesCommand : AbstractCommand
    {
        internal static string[] OutputFiles { get; set; } = new string[0];


        public SetOutputFilesCommand() : base((info, strings) => SetOutputFiles(strings), new[] {"--output", "-o"},
            "Sets the Output for the Files specified as input")
        {
        }


        private static void SetOutputFiles(string[] args)
        {
            OutputFiles = args;
        }
    }

    public class SetInputFilesCommand : AbstractCommand
    {
        internal static string[] InputFiles { get; set; } = new string[0];


        public SetInputFilesCommand() : base((info, strings) => SetOutputFiles(strings), new[] {"--input", "-i"},
            "Sets the Input Files")
        {
        }


        private static void SetOutputFiles(string[] args)
        {
            InputFiles = args;
        }
    }
}