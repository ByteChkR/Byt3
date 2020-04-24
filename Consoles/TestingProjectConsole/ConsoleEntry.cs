using System;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.CommandRunner;
using Byt3.ExtPP.Base;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.Versioning;
using TestingProjectConsole.Commands;

namespace TestingProjectConsole
{
    

    public class ConsoleEntry : AConsole
    {
        internal static bool Exit = false;
        private static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(new ProjectDebugConfig("Test Project", (int)LogType.All, 10, PrefixLookupSettings.AddPrefixIfAvailable));
        public override string ConsoleKey => "testing";
        public override string ConsoleTitle => "Testing Console";

        public override bool Run(string[] args)

        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;

            VersionAccumulatorManager.SearchForAssemblies();
            Debug.DefaultInitialization();

            Runner.AddCommand(new DefaultHelpCommand());
            Runner.AddCommand(new OpenFLBenchmarkCommand());
            Runner.AddCommand(new EngineTimeoutFlagCommand());
            Runner.AddCommand(new EngineSceneRunCommand());

            Logger.Log(LogType.Log, "exit = Exit Current Shell", 1);
            

            if (args.Length != 0)
            {
                Runner.RunCommands(args);
                if (Exit)
                {
                    return true;
                }
            }

            while (true)
            {
                System.Console.Write("root>");
                string command = System.Console.ReadLine();
                if (command == "exit") break;
                Runner.RunCommands(command.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries));
                //if (Exit) break;
            }

            //EngineStatisticsManager.CrashOnLeak();
           // HandleBase.CrashOnLeak();
            return true;
        }
    }
}
