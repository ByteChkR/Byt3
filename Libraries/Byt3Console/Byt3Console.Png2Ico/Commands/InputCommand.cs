using Byt3.CommandRunner;

namespace Byt3Console.Png2Ico.Commands
{
    public class InputCommand : AbstractCommand
    {
        public static  string[] Input { get; private set; } = new string[0];
        public InputCommand() : base(new[] {"--input", "-i"}, "Sets the input Bitmap")
        {
            CommandAction = (info, strings) => SetInput(strings);
            
        }

        private void SetInput(string[] args)
        {
            Input = args;
        }
    }
}