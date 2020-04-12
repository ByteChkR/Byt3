using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.DotNet;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class CreateCommand : AbstractCommand
    {
        public CreateCommand() : base(new[] {"--create", "-c"}, "Creates a new AssemblyModule Config")
        {
            CommandAction = Create;
        }

        private void Create(StartupArgumentInfo argumentInfo, string[] args)
        {
            AssemblyDefinition definition = new AssemblyDefinition();
            Logger.Log(LogType.Log, "Saving new Assembly Definition to file: " + Program.Target);
            AssemblyDefinition.Save(Program.Target, definition);
        }
    }
}