using System;
using Byt3.CommandRunner;
using Byt3.OpenFL.Common.ProgramChecks;

namespace Byt3Console.OpenFL.Benchmarks.Commands
{
    public class AddToCheckPipelineCommand : AbstractCommand
    {
        public AddToCheckPipelineCommand() : base(new[] {"--add-checks"},
            "Specifies any additional checks to be included to the already configured checks.")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            FLProgramCheckType flag = FLProgramCheckType.None;
            for (int i = 0; i < args.Length; i++)
            {
                flag |= (FLProgramCheckType)Enum.Parse(typeof(FLProgramCheckType), args[i], true);
            }

            FLBenchmarkConsole.Settings.CheckProfile = flag;
        }
    }
}