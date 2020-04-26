using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class NoMultiThreadFlagCommand : AbstractCommand
    {
        public NoMultiThreadFlagCommand() : base(new[] {"--no-multi-thread", "-no-thread"},
            "Specifies if the FLbenchmark should use Multithreading to accelerate the parsing process(Default: False).")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            OpenFLBenchmarkCommand.UseMultiThread = true;
        }
    }
}