using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.CommandRunner;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.OpenFL.Serialization;
using Byt3.OpenFL.Tests;

namespace TestingProjectConsole.Commands
{
    public class OpenFLBenchmarkCommand : AbstractCommand
    {
        private const int EXECUTION_BENCHMARK_ITERATIONS = 6;
        private const int CL_BENCHMARK_ITERATIONS = 10000;
        private const int IO_BENCHMARK_ITERATIONS = 200;
        private readonly ADLLogger<LogType> Logger;

        public OpenFLBenchmarkCommand() : base(new[] { "--fl-benchmark", "-flbench" }, "Runs the OpenFL Benchmark")
        {
            Logger = new ADLLogger<LogType>(new ProjectDebugConfig("FL Benchmark", -1, 10,
                PrefixLookupSettings.AddPrefixIfAvailable));
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

            Directory.CreateDirectory("performance");
            string runResultPath = $"performance/{typeof(OpenFLDebugConfig).Assembly.GetName().Version}";
            Directory.CreateDirectory(runResultPath);
            Directory.CreateDirectory("./out/image");

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
                Logger.Log(LogType.Log, RunParsedFLExecutionBenchmark(files, runResultPath), 1);
                Logger.Log(LogType.Log, RunDeserializedFLExecutionBenchmark(files, runResultPath), 1);
                Logger.Log(LogType.Log, RunParserInitBenchmark(files, runResultPath), 1);
                Logger.Log(LogType.Log, RunParserProcessBenchmark(files, runResultPath), 1);
                Logger.Log(LogType.Log, RunProgramInitBenchmark(files, runResultPath), 1);
                Logger.Log(LogType.Log, RunProgramSerializationBenchmark(files, runResultPath), 1);
                Logger.Log(LogType.Log, RunProgramDeserializationBenchmark(files, runResultPath), 1);
                Logger.Log(LogType.Log, $"------------------------Run {i} Finished------------------------", 1);
            }
            Logger.Log(LogType.Log, "------------------------Run Execution Finished------------------------", 1);
        }

        private string CLBufferCreationTest()
        {
            float[] b = new float[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = i;
            }

            bool pass = true;
            PerformanceTester.Tester.RunTest("CLTest", CL_BENCHMARK_ITERATIONS, null, () =>
            {
                MemoryBuffer buffer =
                    CLAPI.CreateBuffer(CLAPI.MainThread, b, MemoryFlag.ReadWrite, "TestBuffer");
                float[] c = CLAPI.ReadBuffer<float>(CLAPI.MainThread, buffer, b.Length);
                buffer.Dispose();
                pass &= CheckValues(c, b);
            },
                null);


            return "CL Test: " + (pass ? "Passed" : "Failed");
        }

        private static bool CheckValues(float[] values, float[] reference)
        {
            bool working = true;
            for (int i = 0; i < values.Length; i++)
            {
                if (Math.Abs(values[i] - reference[i]) > 0.01f)
                {
                    working = false;
                }
            }

            return working;
        }

        private string RunParsedFLExecutionBenchmark(List<string> files, string runResultPath)
        {
            string performanceOut = "performance/FL_ParsedExecution_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();
            FLProgramCheckPipeline checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
            FLParser parser = new FLParser(iset, bc, checkPipeline);
            

            for (int i = 0; i < files.Count; i++)
            {
                Logger.Log(LogType.Log, "Running Script: " + files[i], 1);
                Bitmap bmp = null;
                FLBuffer buf = null;
                SerializableFLProgram parsedProgram = null;
                FLProgram program = null;
                string key = "FLParsedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                parsedProgram = parser.Process(new FLParserInput(files[i]));

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, EXECUTION_BENCHMARK_ITERATIONS,
                     () => //BeforeTest
                     {
                         bmp = new Bitmap(128, 128);
                         buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);
                         program = parsedProgram.Initialize(iset);
                     },
                     () =>
                     {

                         program.Run(CLAPI.MainThread, buf);

                     },
                     () => //After Test
                     {
                         Logger.Log(LogType.Log, "------------------------Run Finished------------------------", 1);
#if DEBUG
                         CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, program.ActiveBuffer.Buffer);
                         string p = Path.Combine("./out/image", Path.GetFileNameWithoutExtension(files[i]) + ".png");
                         bmp.Save(p);
#endif

                         program.FreeResources();
                         buf.Dispose();
                         bmp.Dispose();
                     });
                logOut.AppendLine("\t" + result);

                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);

                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTester.PerformanceResult));
                Stream s = File.Create(Path.Combine(runResultPath, key + ".xml"));
                xs.Serialize(s,result);
                s.Close();

                //Stopwatch sw = Stopwatch.StartNew();

                //sw.Stop();
                //decimal result = (decimal)Math.Round(sw.Elapsed.TotalMilliseconds, 4);

                //string key = "FLParsedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;
                //bool matched = PerformanceTester.Tester.MatchesTarget(key, result, out decimal deltaFromTarget, out decimal targetActual);
                //logOut.AppendLine($"\t{Path.GetFileNameWithoutExtension(files[i]) }+{i}: {result}ms ({Math.Round(result / targetActual * 100, 4)}%); Target Matched: {matched}; Target: {targetActual}ms; Delta: {deltaFromTarget}ms");


            }
            db.Dispose();
            logOut.AppendLine();
            File.AppendAllText(performanceOut, logOut.ToString());
            return logOut.ToString();
        }
        private string RunDeserializedFLExecutionBenchmark(List<string> files, string runResultPath)
        {
            string performanceOut = "performance/FL_DeserializedExecution_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();
            FLProgramCheckPipeline checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
            FLParser parser = new FLParser(iset, bc, checkPipeline);

            for (int i = 0; i < files.Count; i++)
            {

                Bitmap bmp = null;
                FLBuffer buf = null;
                SerializableFLProgram parsedProgram = null;
                MemoryStream ms = new MemoryStream();
                FLProgram program = null;
                string key = "FLDeserializedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                parsedProgram = parser.Process(new FLParserInput(files[i]));
                FLSerializer.SaveProgram(ms, parsedProgram, new string[0]);

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, EXECUTION_BENCHMARK_ITERATIONS,
                     () => //BeforeTest
                     {
                         bmp = new Bitmap(128, 128);
                         buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);

                         ms.Position = 0;
                         program = FLSerializer.LoadProgram(ms).Initialize(iset);
                     },
                     () => program.Run(CLAPI.MainThread, buf),
                     () => //After Test
                     {
                         program.FreeResources();
                         buf.Dispose();
                         bmp.Dispose();
                     });
                ms.Dispose();
                logOut.AppendLine("\t" + result);

                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTester.PerformanceResult));
                Stream s = File.Create(Path.Combine(runResultPath, key + ".xml"));
                xs.Serialize(s, result);
                s.Close();
            }
            db.Dispose();
            logOut.AppendLine();
            File.AppendAllText(performanceOut, logOut.ToString());
            return logOut.ToString();
        }

        private string RunProgramSerializationBenchmark(List<string> files, string runResultPath)
        {
            string performanceOut = "performance/FL_SerializationProcess_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();
            FLProgramCheckPipeline checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
            FLParser parser = new FLParser(iset, bc, checkPipeline);
            MemoryStream dst = new MemoryStream();
            for (int i = 0; i < files.Count; i++)
            {
                SerializableFLProgram pr = parser.Process(new FLParserInput(files[i]));
                string key = "ProgramSerializationPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, IO_BENCHMARK_ITERATIONS,
                    () => dst.Position = 0,
                    () => FLSerializer.SaveProgram(dst, pr, new string[] { }),
                    null);
                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTester.PerformanceResult));
                Stream s = File.Create(Path.Combine(runResultPath, key + ".xml"));
                xs.Serialize(s, result);
                s.Close();
            }
            logOut.AppendLine();
            db.Dispose();
            dst.Dispose();
            File.WriteAllText(performanceOut, logOut.ToString());
            return logOut.ToString();
        }

        private string RunProgramDeserializationBenchmark(List<string> files, string runResultPath)
        {
            string performanceOut = "performance/FL_DeserializationProcess_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();
            FLProgramCheckPipeline checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
            FLParser parser = new FLParser(iset, bc, checkPipeline);
            MemoryStream dst = new MemoryStream();
            for (int i = 0; i < files.Count; i++)
            {
                SerializableFLProgram pr = parser.Process(new FLParserInput(files[i]));
                string key = "ProgramDeserializationPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;
                FLSerializer.SaveProgram(dst, pr, new string[] { });
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, IO_BENCHMARK_ITERATIONS,
                    () => dst.Position = 0,
                    () => FLSerializer.LoadProgram(dst),
                    null);
                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTester.PerformanceResult));
                Stream s = File.Create(Path.Combine(runResultPath, key + ".xml"));
                xs.Serialize(s, result);
                s.Close();
            }
            logOut.AppendLine();
            db.Dispose();
            dst.Dispose();
            File.WriteAllText(performanceOut, logOut.ToString());
            return logOut.ToString();
        }

        private string RunProgramInitBenchmark(List<string> files, string runResultPath)
        {

            string performanceOut = "performance/FL_ProgramInit_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();
            FLProgramCheckPipeline checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
            FLParser parser = new FLParser(iset, bc, checkPipeline);

            for (int i = 0; i < files.Count; i++)
            {
                //Untimed
                SerializableFLProgram pr = parser.Process(new FLParserInput(files[i]));
                FLProgram program = null;

                string key = "ProgramInitPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, IO_BENCHMARK_ITERATIONS,
                    null,
                    () => { program = pr.Initialize(iset); },
                    () => program.FreeResources());

                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTester.PerformanceResult));
                Stream s = File.Create(Path.Combine(runResultPath, key + ".xml"));
                xs.Serialize(s, result);
                s.Close();
            }
            logOut.AppendLine();
            db.Dispose();
            File.WriteAllText(performanceOut, logOut.ToString());
            return logOut.ToString();
        }

        private string RunParserProcessBenchmark(List<string> files, string runResultPath)
        {
            string performanceOut = "performance/FL_ParserProcess_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();
            FLProgramCheckPipeline checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
            FLParser parser = new FLParser(iset, bc, checkPipeline);

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                SerializableFLProgram pr = null;
                string key = "ParserPerformance+" + Path.GetFileNameWithoutExtension(file) + i;
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key,
                    IO_BENCHMARK_ITERATIONS,
                    null,
                    () => parser.Process(new FLParserInput(file)),
                    null);
                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTester.PerformanceResult));
                Stream s = File.Create(Path.Combine(runResultPath, key + ".xml"));
                xs.Serialize(s, result);
                s.Close();
            }
            logOut.AppendLine();
            db.Dispose();
            File.WriteAllText(performanceOut, logOut.ToString());
            return logOut.ToString();
        }

        private string RunParserInitBenchmark(List<string> files, string runResultPath)
        {

            string performanceOut = "performance/FL_ParserInit_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);


            for (int i = 0; i < files.Count; i++)
            {
                FLInstructionSet iset = null;
                BufferCreator bc = null;
                FLParser parser = null;
                FLProgramCheckPipeline checkPipeline = null;
                string key = "ParserInitPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, IO_BENCHMARK_ITERATIONS, null,
                    () =>
                    {
                        iset = FLInstructionSet.CreateWithBuiltInTypes(db);
                        bc = BufferCreator.CreateWithBuiltInTypes();
                        checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
                        parser = new FLParser(iset, bc, checkPipeline);
                    }, null);

                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTester.PerformanceResult));
                Stream s = File.Create(Path.Combine(runResultPath, key + ".xml"));
                xs.Serialize(s, result);
                s.Close();
            }
            logOut.AppendLine();
            db.Dispose();
            File.WriteAllText(performanceOut, logOut.ToString());
            return logOut.ToString();
        }

        //private void RunIOBenchmark()
        //{
        //    string[] dirs = new[] { "resources/filter/tests" };
        //    List<string> files = new List<string>();
        //    files.Add("resources/filter/game/tennisball.fl");
        //    for (int i = 0; i < dirs.Length; i++)
        //    {
        //        files.AddRange(Directory.GetFiles(dirs[i], "*.fl", SearchOption.TopDirectoryOnly));
        //    }

        //    Directory.CreateDirectory("performance");

        //    string performanceOut = "performance/FL_IO_Performance.log";
        //    StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
        //    KernelDatabase db =
        //        new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
        //    bool matchedAll = true;
        //    for (int j = 0; j < files.Count; j++)
        //    {
        //        string path = files[j];

        //        logOut.AppendLine($"\tPerformance Tests: {path}");


        //        FLInstructionSet iset = null;
        //        BufferCreator bc = null;
        //        FLParser parser = null;
        //        FLProgramCheckPipeline checkPipeline = null;

        //        string key = "ParserInitPerformance+" + Path.GetFileNameWithoutExtension(path) + j;

        //        PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, IO_BENCHMARK_ITERATIONS, null,
        //             () =>
        //             {
        //                 iset = FLInstructionSet.CreateWithBuiltInTypes(db);
        //                 bc = BufferCreator.CreateWithBuiltInTypes();
        //                 checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
        //                 parser = new FLParser(iset, bc, checkPipeline);
        //             }, null);


        //        matchedAll &= result.Matched;
        //        logOut.AppendLine(result.ToString());

        //        //Untimed
        //        SerializableFLProgram pr = null;
        //        key = "ParserPerformance+" + Path.GetFileNameWithoutExtension(path) + j;
        //        result = PerformanceTester.Tester.RunTest(key, IO_BENCHMARK_ITERATIONS, null,
        //            () => parser.Process(new FLParserInput(path)), null);
        //        matchedAll &= result.Matched;
        //        logOut.AppendLine(result.ToString());


        //        //Untimed
        //        FLProgram program = null;

        //        key = "ProgramInitPerformance+" + Path.GetFileNameWithoutExtension(path) + j;
        //        result = PerformanceTester.Tester.RunTest(key, IO_BENCHMARK_ITERATIONS,
        //            null,
        //            () => { program = pr.Initialize(iset); },
        //            program.FreeResources);

        //        matchedAll &= result.Matched;
        //        logOut.AppendLine(result.ToString());

        //        MemoryStream dst = new MemoryStream();

        //        key = "ProgramSerializationPerformance+" + Path.GetFileNameWithoutExtension(path) + j;
        //        result = PerformanceTester.Tester.RunTest(key, IO_BENCHMARK_ITERATIONS,
        //            () => dst.Position = 0,
        //            () => FLSerializer.SaveProgram(dst, pr, new string[] { }),
        //            null);
        //        matchedAll &= result.Matched;
        //        logOut.AppendLine(result.ToString());

        //        logOut.AppendLine();

        //    }

        //    db.Dispose();

        //    File.AppendAllText(performanceOut, logOut.ToString());

        //}

        //        private void RunExecutionBenchmark()
        //        {
        //            Byt3.ADL.Debug.DefaultInitialization();
        //            string path = "resources/filter/tests";
        //            List<string> files = Directory.GetFiles(path, "*.fl", SearchOption.TopDirectoryOnly).ToList();
        //            files.Add("resources/filter/game/tennisball.fl");
        //            KernelDatabase db =
        //                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);


        //            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
        //            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();
        //            FLParser parser = new FLParser(iset, bc);
        //            string performanceOut = "performance/FL_ParsedExec_Performance.log";
        //            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");



        //#if DEBUG
        //            Directory.CreateDirectory("./out/image");
        //#endif

        //            for (int i = 0; i < files.Count; i++)
        //            {

        //                Bitmap bmp = null;
        //                FLBuffer buf = null;
        //                SerializableFLProgram parsedProgram = null;
        //                FLProgram program = null;
        //                string key = "FLParsedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;

        //                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, EXECUTION_BENCHMARK_ITERATIONS,
        //                     () => //BeforeTest
        //                     {
        //                         bmp = new Bitmap(128, 128);
        //                         buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);
        //                         parsedProgram = parser.Process(new FLParserInput(files[i]));
        //                         program = parsedProgram.Initialize(iset);
        //                     },
        //                     () => program.Run(CLAPI.MainThread, buf),
        //                     () => //After Test
        //                     {
        //#if DEBUG
        //                         CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, buf.Buffer);
        //                         string p = Path.Combine("./out/image", Path.GetFileNameWithoutExtension(files[i]) + ".png");
        //                         bmp.Save(p + ".png");
        //#endif
        //                         program.FreeResources();
        //                         buf.Dispose();
        //                         bmp.Dispose();
        //                     });
        //                logOut.AppendLine(result.ToString());


        //                //Stopwatch sw = Stopwatch.StartNew();

        //                //sw.Stop();
        //                //decimal result = (decimal)Math.Round(sw.Elapsed.TotalMilliseconds, 4);

        //                //string key = "FLParsedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;
        //                //bool matched = PerformanceTester.Tester.MatchesTarget(key, result, out decimal deltaFromTarget, out decimal targetActual);
        //                //logOut.AppendLine($"\t{Path.GetFileNameWithoutExtension(files[i]) }+{i}: {result}ms ({Math.Round(result / targetActual * 100, 4)}%); Target Matched: {matched}; Target: {targetActual}ms; Delta: {deltaFromTarget}ms");


        //            }
        //            db.Dispose();
        //            logOut.AppendLine();
        //            File.AppendAllText(performanceOut, logOut.ToString());
        //        }

    }
}