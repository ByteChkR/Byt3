using System;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class SetTargetRuntimeCommand : AbstractCommand
    {
        public SetTargetRuntimeCommand() : base(SetTargetRuntime, new[] { "--set-target-runtime", "-sruntime" }, "Sets the Assembly name. Default: TestAssembly") { }

        private static void SetTargetRuntime(StartupInfo info, string[] args)
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

            Console.WriteLine("Setting Target Runtime: " + args[0]);
            definition.NoTargetRuntime = args[0].ToLower() == "none";
            definition.BuildTargetRuntime = args[0];

            AssemblyDefinition.Save(Program.Target, definition);
        }
    }
}