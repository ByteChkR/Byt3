﻿using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3Console.AssemblyGenerator.Commands
{
    public class SetBuildConfigCommand : AbstractCommand
    {
        public SetBuildConfigCommand() : base(new[] {"--set-build-config", "-sbuild"},
            "Sets the build Config. Default: Release")
        {
            CommandAction = (info, strings) => SetBuildConfig(strings);
        }

        private void SetBuildConfig(string[] args)
        {
            if (!AssemblyGeneratorConsole.HasTarget)
            {
                Logger.Log(LogType.Error, "You need to specify a target config", 1);
                return;
            }

            if (args.Length != 1)
            {
                Logger.Log(LogType.Error, "Only 1 argument allowed.", 1);
                return;
            }

            AssemblyDefinition definition = AssemblyDefinition.Load(AssemblyGeneratorConsole.Target);
            definition.BuildConfiguration = args[0];
            AssemblyDefinition.Save(AssemblyGeneratorConsole.Target, definition);
        }
    }
}