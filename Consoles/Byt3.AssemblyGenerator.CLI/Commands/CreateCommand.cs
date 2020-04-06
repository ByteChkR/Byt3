using System;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class CreateCommand : AbstractCommand
    {

        public CreateCommand() : base(Create, new[] { "--create", "-c" }, "Creates a new AssemblyModule Config")
        {

        }

        private static void Create(StartupInfo info, string[] args)
        {
            AssemblyDefinition definition = new AssemblyDefinition();
            Console.WriteLine("Saving new Assembly Definition to file: " + Program.Target);
            AssemblyDefinition.Save(Program.Target, definition);
        }

    }
}