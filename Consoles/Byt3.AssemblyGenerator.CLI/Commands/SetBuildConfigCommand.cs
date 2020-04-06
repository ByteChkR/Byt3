using System;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class SetBuildConfigCommand : AbstractCommand
    {
        public SetBuildConfigCommand():base(SetBuildConfig, new []{"--set-build-config", "-sbuild"}, "Sets the build Config. Default: Release") { }

        private static void SetBuildConfig(StartupInfo info, string[] args)
        {
            if (!Program.HasTarget)
            {
                Console.WriteLine("You need to specify a target config");
                return;
            }

            if (args.Length != 1)
            {
                Console.WriteLine("Only 1 argument allowed.");
                return;
            }

            AssemblyDefinition definition = AssemblyDefinition.Load(Program.Target);
            definition.BuildConfiguration = args[0];
            AssemblyDefinition.Save(Program.Target, definition);
        }
    }
}