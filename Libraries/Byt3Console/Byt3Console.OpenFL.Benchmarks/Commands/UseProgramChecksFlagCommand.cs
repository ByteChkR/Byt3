using Byt3.CommandRunner;

namespace Byt3Console.OpenFL.Benchmarks.Commands
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
            FLBenchmarkConsole.UseProgramChecks = false;
        }
    }
}