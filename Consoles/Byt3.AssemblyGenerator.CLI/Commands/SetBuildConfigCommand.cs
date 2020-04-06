using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class SetBuildConfigCommand : AbstractCommand
    {
        public SetBuildConfigCommand() : base(new[] {"--set-build-config", "-sbuild"},
            "Sets the build Config. Default: Release")
        {
            CommandAction = SetBuildConfig;
        }

        private  void SetBuildConfig(StartupArgumentInfo argumentInfo, string[] args)
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
            definition.BuildConfiguration = args[0];
            AssemblyDefinition.Save(Program.Target, definition);
        }
    }
}