using Byt3.CommandRunner;

namespace Byt3Console.OpenFL.Commands
{
    public class SetInputFilesCommand : AbstractCommand
    {
        public SetInputFilesCommand() : base((info, strings) => SetOutputFiles(strings), new[] {"--input", "-i"},
            "Sets the Input Files")
        {
        }

        internal static string[] InputFiles { get; set; } = new string[0];


        private static void SetOutputFiles(string[] args)
        {
            InputFiles = args;
        }
    }
}