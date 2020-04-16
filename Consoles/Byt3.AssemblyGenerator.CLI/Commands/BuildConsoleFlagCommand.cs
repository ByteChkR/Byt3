using Byt3.CommandRunner;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class BuildConsoleFlagCommand : AbstractCommand
    {
        public BuildConsoleFlagCommand() : base((info, strings) => BuildConsoleFlag(),
            new[] {"--build-console", "-console"},
            "When used the --build command will create a console instead of a library")
        {
        }

        private static void BuildConsoleFlag()
        {
            Program.BuildConsole = true;
        }
    }
}