using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class SetPerformanceFolderCommand : AbstractCommand
    {
        public SetPerformanceFolderCommand() : base(new[] {"--set-output", "-out"},
            "Specifies if the FLbenchmark should use any Other directory than the default directory to store the outputs.")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            OpenFLBenchmarkCommand.PerformanceFolder = args[0];
        }
    }
}