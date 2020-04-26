using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class SetExtraStepsCommand : AbstractCommand
    {

        public SetExtraStepsCommand() : base(new[] { "--extra" },
            "Specifies if the FLbenchmark should use any Extra Stages when serializing a FL Script.")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            OpenFLBenchmarkCommand.ExtraSteps = args;
        }

    }
}