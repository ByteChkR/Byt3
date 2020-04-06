using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Byt3.AssemblyGenerator.CLI.Commands;
using Byt3.CommandRunner;

namespace Byt3.AssemblyGenerator.CLI
{
    internal class Program
    {
        public static bool BuildConsole;
        public static string Target;
        public static bool HasTarget => !string.IsNullOrEmpty(Target);
        static void Main(string[] args)
        {

            Runner.AddCommand(new DefaultCommand());
            Runner.AddCommand(new CreateCommand());
            Runner.AddCommand(new AddCommand());
            Runner.AddCommand(new SetBuildConfigCommand());
            Runner.AddCommand(new SetNameCommand());
            Runner.AddCommand(new SetTargetRuntimeCommand());
            Runner.AddCommand(new BuildConsoleFlagCommand());
            Runner.AddCommand(new BuildCommand());

            Runner.RunCommands(args);

#if DEBUG
            Console.WriteLine("Press enter to exit....");
            Console.ReadLine();
#endif
        }
    }
}
