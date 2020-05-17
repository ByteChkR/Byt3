using System;
using System.Collections.Generic;
using System.Reflection;
using Byt3.ADL;
using Byt3.Callbacks;
using Byt3.CommandRunner;
using Byt3.CommandRunner.SetSettings;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Benchmarking;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Instructions;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.ManifestIO;
using Byt3.Utilities.TypeFinding;
using Byt3Console.OpenFL.Benchmarks.Commands;

namespace Byt3Console.OpenFL.Benchmarks
{
    public class FLBenchmarkConsole : AConsole
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLBenchmarkingDebugConfig.Settings, "FLBench-Console");

        public static bool UseProgramChecks = true;
        public static bool UseMultiThread;
        public static FLBenchmarkSettings Settings = FLBenchmarkSettings.Default;

        internal static bool DoExecute = true;
        public override string ConsoleKey => "flbench";
        public override string ConsoleTitle => "Open FL Benchmark Console";

        public override bool Run(string[] args)
        {
            TypeAccumulator.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly);

            Debug.DefaultInitialization();
            Runner.AddCommand(new HelpCommand(new DefaultHelpCommand(true)));
            SetSettingsCommand cmd = SetSettingsCommand.CreateSettingsCommand("Settings", Settings);
            Runner.AddCommand(new ListSettingsCommand(cmd));
            Runner.AddCommand(cmd);
            Runner.AddCommand(new MultiThreadFlagCommand());
            Runner.AddCommand(new AddToCheckPipelineCommand());

            Runner.RunCommands(args);

            if (!DoExecute)
            {
                Console.ReadLine();
                return true;
            }

            string[] checkTypesStr = Settings.CheckPipeline.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            List<Type> checkTypes = new List<Type>();
            foreach (string s in checkTypesStr)
            {
                List<Type> r = TypeAccumulator<FLProgramCheck>.GetTypesByName(s);
                if (r.Count != 1)
                {
                    Console.Write("");
                }

                checkTypes.AddRange(r);
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
                        checkTypes.ToArray(), UseMultiThread), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunProgramInitBenchmark("_run" + i, files, Settings.InitIterations,
                        Settings.PerformanceFolder,
                        checkTypes.ToArray(), true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunProgramSerializationBenchmark("_run" + i, files, Settings.IOIterations,
                        Settings.ExtraSteps.Split(';'),
                        Settings.PerformanceFolder, checkTypes.ToArray(), true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunProgramDeserializationBenchmark("_run" + i, files, Settings.IOIterations,
                        Settings.PerformanceFolder, checkTypes.ToArray(), true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunParsedFLExecutionBenchmark(Settings.WarmProgram, "_run" + i, files,
                        Settings.ExecutionIterations,
                        Settings.PerformanceFolder, checkTypes.ToArray(), true), 1);
                Logger.Log(LogType.Log,
                    OpenFLBenchmarks.RunDeserializedFLExecutionBenchmark("_run" + i, files,
                        Settings.ExecutionIterations,
                        Settings.PerformanceFolder, checkTypes.ToArray(), true), 1);
                OpenFLBenchmarks.FinalizeTestRun(Settings.PerformanceFolder);
                Logger.Log(LogType.Log, $"------------------------Run {i} Finished------------------------", 1);
            }


            Logger.Log(LogType.Log, "------------------------Run Execution Finished------------------------", 1);
            return true;
        }

        private static void InitializeAssemblyFS()
        {
            ManifestReader.RegisterAssembly(Assembly.GetExecutingAssembly());
            ManifestReader.RegisterAssembly(typeof(OpenFLBenchmarks).Assembly);
            ManifestReader.RegisterAssembly(typeof(KernelFLInstruction).Assembly);
            ManifestReader.PrepareManifestFiles(false);
            EmbeddedFileIOManager.Initialize();
        }

        private List<string> GetFileList()
        {
            InitializeAssemblyFS();
            List<string> files = new List<string>();
            string[] dirs = Settings.ScriptDirectories.Split(';');
            for (int i = 0; i < dirs.Length; i++)
            {
                if (IOManager.DirectoryExists(dirs[i]))
                {
                    files.AddRange(IOManager.GetFiles(dirs[i], "*.fl"));
                }
            }

            return files;
        }

        private class HelpCommand : AbstractCommand
        {
            private readonly DefaultHelpCommand cmd;

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
    }
}