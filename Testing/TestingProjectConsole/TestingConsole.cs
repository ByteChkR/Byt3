﻿using System;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.CommandRunner;
using Byt3.Utilities.ConsoleInternals;
using TestingProjectConsole.Commands;

namespace TestingProjectConsole
{
    public class TestingConsole : AConsole
    {
        internal static bool Exit;

        private static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(
            new ProjectDebugConfig("Test Project", (int) LogType.All, 10, PrefixLookupSettings.AddPrefixIfAvailable));

        public override string ConsoleKey => "testing";
        public override string ConsoleTitle => "Testing Console";

        public override bool Run(string[] args)

        {
            Debug.DefaultInitialization();

            Runner.AddCommand(new DefaultHelpCommand(true));
            Runner.AddCommand(new ExitAfterFlagCommand());
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
                Console.Write("root/testing>");
                string command = Console.ReadLine();
                if (command == "exit")
                {
                    break;
                }

                Runner.RunCommands(command.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
            }

            return true;
        }
    }
}