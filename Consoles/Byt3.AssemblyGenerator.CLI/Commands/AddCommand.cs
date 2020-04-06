using System;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class AddCommand : AbstractCommand
    {

        public AddCommand() : base(Add, new[] { "--add", "-a" }, "Adds a new csproj or moduleconfig file to the target")
        {

        }

        private static void Add(StartupInfo info, string[] args)
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

            ModuleDefinition def = null;
            if (args[0].EndsWith(".csproj"))
            {
                def = AssemblyGenerator.GenerateModuleDefinition(args[0]);
            }
            else if(args[0].EndsWith(".moduleconfig"))
            {
                def= ModuleDefinition.Load(args[0]);
            }
            else
            {
                Console.WriteLine("Can not Parse File: "+ args[0]);
                return;
            }

            AssemblyDefinition definition = AssemblyDefinition.Load(Program.Target);
            definition.IncludedModules.Add(def);
            AssemblyDefinition.Save(Program.Target, definition);
        }
    }
}