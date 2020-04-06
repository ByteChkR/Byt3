using System;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class SetNameCommand:AbstractCommand
    {
        public SetNameCommand() : base( new[] {"--set-assembly-name", "-sname"},
            "Sets the Assembly name. Default: TestAssembly")
        {
            CommandAction = SetName;
        }

        private void SetName(StartupInfo info, string[] args)
        {
            if (!Program.HasTarget)
            {
                Logger.Log(LogType.Error, "You need to specify a target config");
                return;
            }

            if (args.Length != 1)
            {
                Logger.Log(LogType.Error, "Only 1 argument allowed.");
                return;
            }

            AssemblyDefinition definition = AssemblyDefinition.Load(Program.Target);
            definition.AssemblyName = args[0];
            AssemblyDefinition.Save(Program.Target, definition);
        }
    }
}