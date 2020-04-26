using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class UseProgramChecksFlagCommand : AbstractCommand
    {
        public UseProgramChecksFlagCommand() : base(new[] {"--no-checks"},
            "Specifies if the FLbenchmark should use Program Checks to validate the script.")
        {
            CommandAction = (info, strings) => SetFlag();
        }

        private void SetFlag()
        {
            OpenFLBenchmarkCommand.UseProgramChecks = false;
        }
    }
}