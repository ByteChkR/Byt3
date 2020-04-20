﻿using System;
using System.IO;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.Console.Commands
{
    public class BuildCommand : AbstractCommand
    {
        public BuildCommand() : base(new[] { "--build", "-b" },
            "Builds the Target Assembly Config and stores the build output in a folder in the current working directory")
        {
            CommandAction = (info, strings) => Build(strings);
        }

        private void Build(string[] args)
        {
            if (!ConsoleEntry.HasTarget)
            {
                Logger.Log(LogType.Log, "You need to specify a target config", 1);
                return;
            }

            AssemblyDefinition def = AssemblyDefinition.Load(ConsoleEntry.Target);

            AssemblyGeneratorBuildType buildType = AssemblyGeneratorBuildType.Publish;
            if (args.Length == 1 && !Enum.TryParse(args[0], out buildType))
            {
                Logger.Log(LogType.Log, "Can not parse the BuildType. Using Default:" + buildType, 1);
            }

            string path = Path.Combine(Path.GetDirectoryName(ConsoleEntry.Target), $"{ConsoleEntry.Output ?? def.AssemblyName}");
            AssemblyGenerator.GenerateAssembly("dotnet", def, path, buildType, !ConsoleEntry.BuildConsole);
        }
    }
}