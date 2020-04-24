using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3Console.AssemblyGenerator.Commands
{
    public class AddCommand : AbstractCommand
    {
        public AddCommand() : base(new[] {"--add", "-a"}, "Adds a new csproj or moduleconfig file to the target")
        {
            CommandAction = (info, strings) => Add(strings);
        }

        private void Add(string[] args)
        {
            if (!ConsoleEntry.HasTarget)
            {
                Logger.Log(LogType.Log, "You need to specify a target config", 1);
                return;
            }

            if (args.Length != 1)
            {
                Logger.Log(LogType.Log, "Only 1 argument allowed.", 1);
                return;
            }

            ModuleDefinition def = null;
            if (args[0].EndsWith(".csproj"))
            {
                def = Byt3.AssemblyGenerator.AssemblyGenerator.GenerateModuleDefinition(args[0]);
            }
            else if (args[0].EndsWith(".moduleconfig"))
            {
                def = ModuleDefinition.Load(args[0]);
            }
            else
            {
                Logger.Log(LogType.Log, "Can not Parse File: " + args[0], 1);
                return;
            }

            AssemblyDefinition definition = AssemblyDefinition.Load(ConsoleEntry.Target);
            definition.IncludedModules.Add(def);
            AssemblyDefinition.Save(ConsoleEntry.Target, definition);
        }
    }
}