using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.AssemblyGenerator;
using Byt3.CommandRunner;
using Byt3.Engine.Debug;
using Byt3.Engine.Demos;
using Byt3.Engine.Tutorials;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.Utilities.ConsoleInternals;
using Byt3Console.AssemblyGenerator;
using Byt3Console.Engine.BuildTools;
using Byt3Console.Engine.Player;
using Byt3Console.ExtPP;
using Byt3Console.OpenFL;
using Byt3Console.VersionHelper;
using TestingProjectConsole;

namespace TestingProject
{
    internal static class Program
    {
        internal static bool Exit = false;

        private static readonly Dictionary<string, AConsole> Consoles = new Dictionary<string, AConsole>
        {
            {"testing", new TestingConsole()},
            {"asmgen", new AssemblyGeneratorConsole()},
            {"ebuild", new EngineBuilderConsole()},
            {"eplay", new EnginePlayerConsole()},
            {"tutorials", new TutorialConsoleStarter()},
            {"demos", new DemoConsoleStarter()},
            {"extpp", new ExtPPConsole()},
            {"vh", new VersionHelperConsole()},
            {"fl", new FLConsole()},
        };

        private static void Main(string[] args)
        {
            Debug.DefaultInitialization();

            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            AssemblyGeneratorDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            EngineDebugConfig.Settings.MinSeverity = Verbosity.Level1;

            ADLLogger<LogType> logger = new ADLLogger<LogType>(new ProjectDebugConfig("Testing Project", -1, 10,
                PrefixLookupSettings.AddPrefixIfAvailable));


            logger.Log(LogType.Log, "Available Consoles: " + Consoles.Keys.Unpack(", "), 1);
            logger.Log(LogType.Log, "Type \"exit\" to exit", 1);

            if (args.Length != 0)
            {
                if (Consoles.ContainsKey(args[0]))
                {
                    string[] arg = new string[args.Length - 1];

                    for (int i = 1; i < args.Length; i++)
                    {
                        arg[i - 1] = args[i];
                    }

                    Consoles[args[0]].Run(arg);

                    Runner.RemoveAllCommands();

                    if (Exit)
                    {
                        return;
                    }
                }
            }

            while (true)
            {
                Console.Write("root>");
                string line = Console.ReadLine();
                string[] command = line.Split(new[] {' '});
                if (command[0] == "exit")
                {
                    break;
                }

                if (Consoles.ContainsKey(command[0]))
                {
                    string[] arg = new string[command.Length - 1];

                    for (int i = 1; i < command.Length; i++)
                    {
                        arg[i - 1] = command[i];
                    }

                    Consoles[command[0]].Run(arg);

                    Runner.RemoveAllCommands();
                }

                if (Exit)
                {
                    break;
                }
            }
        }
    }
}