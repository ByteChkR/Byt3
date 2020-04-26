using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class SetWorkSizeMultiplierCommand : AbstractCommand
    {
        public SetWorkSizeMultiplierCommand() : base(new[] {"--work-size-multiplier", "-wsm"},
            "Specifies the work size multiplier the FLBenchmark should use(Default: 2).")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            OpenFLBenchmarkCommand.WorkSizeMultiplier = int.Parse(args[0]);
        }
    }
}