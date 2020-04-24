using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3Console.AssemblyGenerator.Commands
{
    public class SetNameCommand : AbstractCommand
    {
        public SetNameCommand() : base(new[] {"--set-assembly-name", "-sname"},
            "Sets the Assembly name. Default: TestAssembly")
        {
            CommandAction = (info, strings) => SetName(strings);
        }

        private void SetName(string[] args)
        {
            if (!ConsoleEntry.HasTarget)
            {
                Logger.Log(LogType.Error, "You need to specify a target config", 1);
                return;
            }

            if (args.Length != 1)
            {
                Logger.Log(LogType.Error, "Only 1 argument allowed.", 1);
                return;
            }

            AssemblyDefinition definition = AssemblyDefinition.Load(ConsoleEntry.Target);
            definition.AssemblyName = args[0];
            AssemblyDefinition.Save(ConsoleEntry.Target, definition);
        }
    }
}