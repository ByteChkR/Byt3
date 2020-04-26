using System.Globalization;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3Console.AssemblyGenerator.Commands
{
    public class SetTargetRuntimeCommand : AbstractCommand
    {
        public SetTargetRuntimeCommand() : base(new[] {"--set-target-runtime", "-sruntime"},
            "Sets the Assembly name. Default: TestAssembly")
        {
            CommandAction = (info, strings) => SetTargetRuntime(strings);
        }

        private void SetTargetRuntime(string[] args)
        {
            if (!AssemblyGeneratorConsole.HasTarget)
            {
                Logger.Log(LogType.Error, "You need to specify a target config", 1);
                return;
            }

            if (args.Length != 1)
            {
                Logger.Log(LogType.Error, "Only 1 argument allowed.", 1);
                return;
            }

            AssemblyDefinition definition = AssemblyDefinition.Load(AssemblyGeneratorConsole.Target);

            Logger.Log(LogType.Log, "Setting Target Runtime: " + args[0], 1);
            definition.NoTargetRuntime = args[0].ToLower(CultureInfo.InvariantCulture) == "none";
            definition.BuildTargetRuntime = args[0];

            AssemblyDefinition.Save(AssemblyGeneratorConsole.Target, definition);
        }
    }
}