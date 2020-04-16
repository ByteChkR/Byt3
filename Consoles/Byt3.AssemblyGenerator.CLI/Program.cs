using System;
using Byt3.ADL;
using Byt3.AssemblyGenerator.CLI.Commands;
using Byt3.CommandRunner;

namespace Byt3.AssemblyGenerator.CLI
{
    internal static class Program
    {
        public static bool BuildConsole { get; set; }
        public static string Target { get; set; }
        public static bool HasTarget => !string.IsNullOrEmpty(Target);

        private static void Main(string[] args)
        {
            Debug.DefaultInitialization();
            Runner.AddCommand(new DefaultCommand());
            Runner.AddCommand(new CreateCommand());
            Runner.AddCommand(new AddCommand());
            Runner.AddCommand(new SetBuildConfigCommand());
            Runner.AddCommand(new SetNameCommand());
            Runner.AddCommand(new SetTargetRuntimeCommand());
            Runner.AddCommand(new BuildConsoleFlagCommand());
            Runner.AddCommand(new BuildCommand());
            Runner.AddCommand(new DefaultHelpCommand());

            Runner.RunCommands(args);

#if DEBUG
            Console.WriteLine("Press enter to exit....");
            Console.ReadLine();
#endif
        }
    }
}