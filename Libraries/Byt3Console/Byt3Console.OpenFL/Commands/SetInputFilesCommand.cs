using Byt3.CommandRunner;

namespace Byt3Console.OpenFL.Commands
{
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