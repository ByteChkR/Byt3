using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class ExitAfterFlagCommand : AbstractCommand
    {
        public ExitAfterFlagCommand() : base(new[] {"--exit-after", "-ea"},
            "Directly returns from the command.")
        {
            CommandAction = (info, strings) => SetFlag();
        }

        private void SetFlag()
        {
            TestingConsole.Exit = true;
        }
    }
}