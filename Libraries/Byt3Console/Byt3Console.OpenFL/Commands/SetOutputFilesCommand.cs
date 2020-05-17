using Byt3.CommandRunner;

namespace Byt3Console.OpenFL.Commands
{
    public class SetOutputFilesCommand : AbstractCommand
    {
        public SetOutputFilesCommand() : base((info, strings) => SetOutputFiles(strings), new[] {"--output", "-o"},
            "Sets the Output for the Files specified as input")
        {
        }

        internal static string[] OutputFiles { get; set; } = new string[0];


        private static void SetOutputFiles(string[] args)
        {
            OutputFiles = args;
        }
    }
}