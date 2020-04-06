using System;
using System.IO;
using System.Linq;
using Byt3.CommandRunner;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class BuildConsoleFlagCommand : AbstractCommand
    {
        public BuildConsoleFlagCommand() : base(BuildConsoleFlag, new[] { "--build-console", "-console" },
            "When used the --build command will create a console instead of a library")
        {

        }

        private static void BuildConsoleFlag(StartupInfo info, string[] args)
        {
            Program.BuildConsole = true;
        }
    }
}