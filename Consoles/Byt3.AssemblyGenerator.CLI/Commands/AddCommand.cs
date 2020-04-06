using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class AddCommand : AbstractCommand
    {

        public AddCommand() : base( new[] { "--add", "-a" }, "Adds a new csproj or moduleconfig file to the target")
        {
            CommandAction = Add;
        }

        private void Add(StartupArgumentInfo argumentInfo, string[] args)
        {
            if (!Program.HasTarget)
            {
                Logger.Log(LogType.Log, "You need to specify a target config");
                return;
            }

            if (args.Length != 1)
            {
                Logger.Log(LogType.Log, "Only 1 argument allowed.");
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
                Logger.Log(LogType.Log, "Can not Parse File: " + args[0]);
                return;
            }

            AssemblyDefinition definition = AssemblyDefinition.Load(Program.Target);
            definition.IncludedModules.Add(def);
            AssemblyDefinition.Save(Program.Target, definition);
        }
    }
}