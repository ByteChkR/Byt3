using Byt3.CommandRunner;

namespace Byt3Console.Png2Ico.Commands
{
    public class OutputCommand: AbstractCommand
    {
        public static string[] Output { get; private set; } = new string[0];
        public OutputCommand() : base(new[] { "--output", "-o" }, "Sets the ouput Icon")
        {
            CommandAction = (info, strings) => SetInput(strings);
        }

        private void SetInput(string[] args)
        {
            Output = args;
        }
    }
}