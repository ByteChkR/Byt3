using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.ConsoleInternals;
using Byt3Console.AssemblyGenerator.Commands;

namespace Byt3Console.AssemblyGenerator
{
    public class AssemblyGeneratorConsole : AConsole
    {
        public override string ConsoleKey => "asmgen";
        public override string ConsoleTitle => "Assembly Generator";


        public static string Output;
        public static bool BuildConsole { get; set; }
        public static string Target { get; set; }
        public static bool HasTarget => !string.IsNullOrEmpty(Target);

        public override bool Run(string[] args)
        {

            Debug.DefaultInitialization();
            Runner.AddCommand(new DefaultCommand());
            Runner.AddCommand(new CreateCommand());
            Runner.AddCommand(new AddCommand());
            Runner.AddCommand(new SetBuildConfigCommand());
            Runner.AddCommand(new SetNameCommand());
            Runner.AddCommand(new SetTargetRuntimeCommand());
            Runner.AddCommand(new BuildConsoleFlagCommand());
            Runner.AddCommand(new SetOutputFolderCommand());
            Runner.AddCommand(new BuildCommand());
            Runner.AddCommand(new DefaultHelpCommand());

            bool ret = Runner.RunCommands(args);
#if DEBUG
            System.Console.WriteLine("Press Enter to Exit.");
            System.Console.ReadLine();
#endif
            return ret;
        }
    }
}