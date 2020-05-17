using Byt3.CommandRunner;

namespace Byt3Console.Png2Ico.Commands
{
    public class OutputCommand : AbstractCommand
    {
        public OutputCommand() : base(new[] {"--output", "-o"}, "Sets the ouput Icon")
        {
            CommandAction = (info, strings) => SetInput(strings);
        }

        public static string[] Output { get; private set; } = new string[0];

        private void SetInput(string[] args)
        {
            Output = args;
        }
    }
}