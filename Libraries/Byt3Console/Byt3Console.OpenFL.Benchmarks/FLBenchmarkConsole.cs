using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.Callbacks;
using Byt3.CommandRunner;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Benchmarking;
using Byt3.OpenFL.Common;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.ManifestIO;
using Byt3Console.OpenFL.Benchmarks.Commands;
using Byt3.CommandRunner.SetSettings;

namespace Byt3Console.OpenFL.Benchmarks
{
    public class FLBenchmarkConsole : AConsole
    {
        public override string ConsoleKey => "flbench";
        public override string ConsoleTitle => "Open FL Benchmark Console";


        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(new ProjectDebugConfig("FLBench-Console", -1, 3,
                PrefixLookupSettings.AddPrefixIfAvailable));
        public static bool UseProgramChecks = true;
        public static bool UseMultiThread;
        public static FLBenchmarkSettings Settings = FLBenchmarkSettings.Default;

        internal static bool DoExecute = true;

        private class HelpCommand : AbstractCommand
        {
            private DefaultHelpCommand cmd;
            internal HelpCommand(DefaultHelpCommand command) : base(command.CommandKeys, command.HelpText,
                command.DefaultCommand)
            {
                cmd = command;
                CommandAction = RunHelpCommand;
            }

            private void RunHelpCommand(StartupArgumentInfo info, string[] args)
            {
                DoExecute = false;
                cmd.CommandAction(info, args);
            }
        }

        public override bool Run(string[] args)
        {
            Debug.DefaultInitialization();
            Runner.AddCommand(new HelpCommand(new DefaultHelpCommand(true)));
            SetSettingsCommand cmd = SetSettingsCommand.CreateSettingsCommand("Settings", Settings);
            Runner.AddCommand(new ListSettingsCommand(cmd));
            Runner.AddCommand(cmd);
            Runner.AddCommand(new MultiThreadFlagCommand());
            Runner.AddCommand(new UseProgramChecksFlagCommand());

            Runner.RunCommands(args);

            if (!DoExecute)
            {
                Console.ReadLine();
                return true;
            }


            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Silent;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Silent;


            List<string> files = GetFileList();

            Logger.Log(LogType.Log, "------------------------Run Execution Started------------------------", 1);

            for (int i = 0; i < Settings.TotalRepetitions; i++)
            {
                OpenFLBenchmarks.InitializeTestRun(Settings.PerformanceFolder);
                Logger.Log(LogType.Log, $"------------------------Run {i} Started------------------------", 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunParserInitBenchmark("_run" + i, Settings.InitIterations,
                        Settings.PerformanceFolder, true, true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunParserProcessBenchmark("_run" + i, files, Settings.ParsingIterations,
                        Settings.PerformanceFolder,
                        UseProgramChecks, UseMultiThread), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunProgramInitBenchmark("_run" + i, files, Settings.InitIterations,
                        Settings.PerformanceFolder,
                        UseProgramChecks, true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunProgramSerializationBenchmark("_run" + i, files, Settings.IOIterations,
                        Settings.ExtraSteps.Split(new[] { ';' }),
                        Settings.PerformanceFolder, UseProgramChecks, true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunProgramDeserializationBenchmark("_run" + i, files, Settings.IOIterations,
                        Settings.PerformanceFolder, UseProgramChecks, true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunParsedFLExecutionBenchmark("_run" + i, files, Settings.ExecutionIterations,
                        Settings.PerformanceFolder, UseProgramChecks, true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunDeserializedFLExecutionBenchmark("_run" + i, files,
                        Settings.ExecutionIterations,
                        Settings.PerformanceFolder, UseProgramChecks, true), 1);
                OpenFLBenchmarks.FinalizeTestRun(Settings.PerformanceFolder);
                Logger.Log(LogType.Log, $"------------------------Run {i} Finished------------------------", 1);
            }


            Logger.Log(LogType.Log, "------------------------Run Execution Finished------------------------", 1);
            return true;
        }

        private List<string> GetFileList()
        {
            List<string> files = new List<string>();
            string[] dirs = Settings.ScriptDirectories.Split(new[] { ';' });
            for (int i = 0; i < dirs.Length; i++)
            {
                if (IOManager.DirectoryExists(dirs[i]))
                {
                    files.AddRange(IOManager.GetFiles(dirs[i], "*.fl"));
                }
            }

            return files;
        }
    }
}
