using Byt3.CommandRunner;

namespace Byt3Console.Png2Ico.Commands
{
    public class InputCommand : AbstractCommand
    {
        public InputCommand() : base(new[] {"--input", "-i"}, "Sets the input Bitmap")
        {
            CommandAction = (info, strings) => SetInput(strings);
        }

        public static string[] Input { get; private set; } = new string[0];

        private void SetInput(string[] args)
        {
            Input = args;
        }
    }
}