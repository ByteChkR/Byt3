using System;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class SetNameCommand:AbstractCommand
    {
        public SetNameCommand() : base(SetName, new[] { "--set-assembly-name", "-sname" }, "Sets the Assembly name. Default: TestAssembly") { }

        private static void SetName(StartupInfo info, string[] args)
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
            definition.AssemblyName = args[0];
            AssemblyDefinition.Save(Program.Target, definition);
        }
    }
}