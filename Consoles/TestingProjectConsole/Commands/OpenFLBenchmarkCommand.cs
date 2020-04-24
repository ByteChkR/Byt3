using System.Collections.Generic;
using System.IO;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Benchmarking;
using Byt3.OpenFL.Common;

namespace TestingProjectConsole.Commands
{

    

    public class OpenFLBenchmarkCommand : AbstractCommand
    {
        private const int EXECUTION_BENCHMARK_ITERATIONS = 6;
        //private const int CL_BENCHMARK_ITERATIONS = 10000;
        private const int IO_BENCHMARK_ITERATIONS = 200;

        public OpenFLBenchmarkCommand() : base(new[] { "--fl-benchmark", "-flbench" }, "Runs the OpenFL Benchmark")
        {
            CommandAction = RunBenchmark;
        }

        private void RunBenchmark(StartupArgumentInfo info, string[] args)
        {
            List<string> files = new List<string>();
            string[] dirs = new[] { "resources/filter/tests" };
            files.Add("resources/filter/game/tennisball.fl");
            for (int i = 0; i < dirs.Length; i++)
            {
                files.AddRange(Directory.GetFiles(dirs[i], "*.fl", SearchOption.TopDirectoryOnly));
            }


            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Silent;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Silent;


            //for (int i = 0; i < 10; i++)
            //{
            //    Logger.Log(LogType.Log, CLBufferCreationTest(), 1);
            //}

            Logger.Log(LogType.Log, "------------------------Run Execution Started------------------------", 1);
            for (int i = 0; i < 1; i++)
            {
                Logger.Log(LogType.Log, $"------------------------Run {i} Started------------------------", 1);
                Logger.Log(LogType.Log, OpenFLBenchmarks.RunParserInitBenchmark( IO_BENCHMARK_ITERATIONS), 1);
                Logger.Log(LogType.Log, OpenFLBenchmarks.RunParserProcessBenchmark(files,  IO_BENCHMARK_ITERATIONS), 1);
                Logger.Log(LogType.Log, OpenFLBenchmarks.RunProgramInitBenchmark(files,  IO_BENCHMARK_ITERATIONS), 1);
                Logger.Log(LogType.Log, OpenFLBenchmarks.RunProgramSerializationBenchmark(files,  IO_BENCHMARK_ITERATIONS), 1);
                Logger.Log(LogType.Log, OpenFLBenchmarks.RunProgramDeserializationBenchmark(files,  IO_BENCHMARK_ITERATIONS), 1);
                Logger.Log(LogType.Log, OpenFLBenchmarks.RunParsedFLExecutionBenchmark(files,  EXECUTION_BENCHMARK_ITERATIONS), 1);
                Logger.Log(LogType.Log, OpenFLBenchmarks.RunDeserializedFLExecutionBenchmark(files,  EXECUTION_BENCHMARK_ITERATIONS), 1);
                Logger.Log(LogType.Log, $"------------------------Run {i} Finished------------------------", 1);
            }
            Logger.Log(LogType.Log, "------------------------Run Execution Finished------------------------", 1);
        }
        


    }

}