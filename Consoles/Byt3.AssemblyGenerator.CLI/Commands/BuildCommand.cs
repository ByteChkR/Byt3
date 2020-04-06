using System;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class BuildCommand : AbstractCommand
    {

        public BuildCommand() : base(Build, new[] { "--build", "-b" }, "Builds the Target Assembly Config and stores the build output in a folder in the current working directory")
        {

        }

        private static void Build(StartupInfo info, string[] args)
        {
            if (!Program.HasTarget)
            {
                Console.WriteLine("You need to specify a target config");
                return;
            }

            AssemblyDefinition def = AssemblyDefinition.Load(Program.Target);

            AssemblyGeneratorBuildType buildType = AssemblyGeneratorBuildType.Publish;
            if (args.Length == 1 && !Enum.TryParse(args[0], out buildType))
            {
                Console.WriteLine("Can not parse the BuildType. Using Default:" + buildType);
            }

            AssemblyGenerator.GenerateAssembly("dotnet", def, $".\\{def.AssemblyName}_Build\\", buildType, !Program.BuildConsole);
        }

    }
}