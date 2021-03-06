﻿using Byt3.CommandRunner;
using Byt3.Utilities.FastString;

namespace Byt3Console.OpenFL.Benchmarks.Commands
{
    public class AddToCheckPipelineCommand : AbstractCommand
    {
        public AddToCheckPipelineCommand() : base(new[] { "--add-checks" },
            "Specifies any additional checks to be included to the already configured checks.")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            FLBenchmarkConsole.Settings.CheckPipeline += ";" + args.Unpack(";");
        }
    }
}