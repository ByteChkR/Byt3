using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class ExitAfterFlagCommand : AbstractCommand
    {
        public ExitAfterFlagCommand() : base(new[] {"--exit-after", "-ea"},
            "Directly returns from the command.")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            TestingConsole.Exit = true;
        }
    }
}