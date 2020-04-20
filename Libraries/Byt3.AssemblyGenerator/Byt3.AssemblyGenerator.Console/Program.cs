using Byt3.ADL;
using Byt3.AssemblyGenerator.Console.Commands;
using Byt3.CommandRunner;
using Byt3.Utilities.Versioning;

namespace Byt3.AssemblyGenerator.Console
{
    public class ConsoleEntry
    {
        public string ConsoleKey => "asmgen";



        public static bool BuildConsole { get; set; }
        public static string Target { get; set; }
        public static bool HasTarget => !string.IsNullOrEmpty(Target);

        public void Run(string[] args)
        {
            VersionAccumulatorManager.SearchForAssemblies();

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
            System.Console.WriteLine("Press Enter to Exit.");
            System.Console.ReadLine();
#endif
        }
    }
}