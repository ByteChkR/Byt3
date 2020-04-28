using Byt3.CommandRunner;

namespace Byt3Console.OpenFL.Benchmarks.Commands
{
    public class MultiThreadFlagCommand : AbstractCommand
    {
        public MultiThreadFlagCommand() : base(new[] {"--multi-thread", "-m"},
            "Specifies if the FLbenchmark should use Multithreading to accelerate the parsing process(Default: False).")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            FLBenchmarkConsole.UseMultiThread = true;
        }
    }
}