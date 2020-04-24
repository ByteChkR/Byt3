using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.OpenCL.Wrapper;
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

namespace Byt3.OpenFL.Benchmarking
{

    public static class OpenFLBenchmarks
    {
#if DEBUG
        private const int BITMAP_RESOLUTION = 128;
#else
        private const int BITMAP_RESOLUTION = 32;
#endif

        private static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(new ProjectDebugConfig("FL Benchmark", -1, 10,
            PrefixLookupSettings.AddPrefixIfAvailable));

        private static void SaveOutput(Bitmap bmp, FLProgram program, FLSetup setup, string file)
        {
#if DEBUG
            CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, program.ActiveBuffer.Buffer);
            string p = Path.GetFileNameWithoutExtension(file) + ".png";
            Stream bmpStream = setup.GetDataFileStream(p);
            bmp.Save(bmpStream, ImageFormat.Png);
            bmpStream.Close();
#endif
        }

        public static string RunParsedFLExecutionBenchmark(List<string> files, int iterations)
        {
            FLSetup setup = new FLSetup("FL_ParsedExecution_Performance", "resources/kernel");
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            for (int i = 0; i < files.Count; i++)
            {
                Logger.Log(LogType.Log, "Running Script: " + files[i], 1);
                Bitmap bmp = null;
                FLBuffer buf = null;
                SerializableFLProgram parsedProgram = null;
                FLProgram program = null;

                string key = "FLParsedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                parsedProgram = setup.Parser.Process(new FLParserInput(files[i]));

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                     () => //BeforeTest
                     {
                         bmp = new Bitmap(BITMAP_RESOLUTION, BITMAP_RESOLUTION);
                         buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);
                         program = parsedProgram.Initialize(setup.InstructionSet);
                     },
                     () =>
                     {

                         program.Run(CLAPI.MainThread, buf);

                     },
                     () => //After Test
                     {
                         Logger.Log(LogType.Log, "------------------------Run Finished------------------------", 1);
                         SaveOutput(bmp, program, setup, files[i]);

                         program.FreeResources();
                         buf.Dispose();
                         bmp.Dispose();
                     });
                logOut.AppendLine("\t" + result);

                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);

                setup.WriteResult(result);


            }


            logOut.AppendLine();
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }
        public static string RunDeserializedFLExecutionBenchmark(List<string> files, int iterations)
        {
            FLSetup setup = new FLSetup("FL_DeserializedExecution_Performance", "resources/kernel");
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            for (int i = 0; i < files.Count; i++)
            {

                Bitmap bmp = null;
                FLBuffer buf = null;
                SerializableFLProgram parsedProgram = null;
                MemoryStream ms = new MemoryStream();
                FLProgram program = null;
                string key = "FLDeserializedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                parsedProgram = setup.Parser.Process(new FLParserInput(files[i]));
                FLSerializer.SaveProgram(ms, parsedProgram, new string[0]);

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                     () => //BeforeTest
                     {
                         bmp = new Bitmap(BITMAP_RESOLUTION, BITMAP_RESOLUTION);
                         buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);

                         ms.Position = 0;
                         program = FLSerializer.LoadProgram(ms).Initialize(setup.InstructionSet);
                     },
                     () => program.Run(CLAPI.MainThread, buf),
                     () => //After Test
                     {
                         SaveOutput(bmp, program, setup, files[i]);
                         program.FreeResources();
                         buf.Dispose();
                         bmp.Dispose();
                     });
                ms.Dispose();
                logOut.AppendLine("\t" + result);

                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                setup.WriteResult(result);
            }
            logOut.AppendLine();
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }

        public static string RunProgramSerializationBenchmark(List<string> files, int iterations)
        {
            FLSetup setup = new FLSetup("FL_SerializationProcess_Performance", "resources/kernel");
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            MemoryStream dst = new MemoryStream();
            for (int i = 0; i < files.Count; i++)
            {
                SerializableFLProgram pr = setup.Parser.Process(new FLParserInput(files[i]));
                string key = "ProgramSerializationPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                    () => dst.Position = 0,
                    () => FLSerializer.SaveProgram(dst, pr, new string[] { }),
                    null);
                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                setup.WriteResult(result);
            }
            dst.Dispose();
            logOut.AppendLine();
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }

        public static string RunProgramDeserializationBenchmark(List<string> files, int iterations)
        {
            FLSetup setup = new FLSetup("FL_DeserializationProcess_Performance", "resources/kernel");
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            MemoryStream dst = new MemoryStream();
            for (int i = 0; i < files.Count; i++)
            {
                SerializableFLProgram pr = setup.Parser.Process(new FLParserInput(files[i]));
                string key = "ProgramDeserializationPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;
                FLSerializer.SaveProgram(dst, pr, new string[] { });
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                    () => dst.Position = 0,
                    () => FLSerializer.LoadProgram(dst),
                    null);
                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                setup.WriteResult(result);
            }
            dst.Dispose();
            logOut.AppendLine();
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }

        public static string RunProgramInitBenchmark(List<string> files,  int iterations)
        {

            FLSetup setup = new FLSetup("FL_ProgramInit_Performance", "resources/kernel");
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            for (int i = 0; i < files.Count; i++)
            {
                //Untimed
                SerializableFLProgram pr = setup.Parser.Process(new FLParserInput(files[i]));
                FLProgram program = null;

                string key = "ProgramInitPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                    null,
                    () => { program = pr.Initialize(setup.InstructionSet); },
                    () => program.FreeResources());

                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                setup.WriteResult(result);
            }
            logOut.AppendLine();
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }

        public static string RunParserProcessBenchmark(List<string> files, int iterations)
        {
            FLSetup setup = new FLSetup("FL_ParserProcess_Performance", "resources/kernel");
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                SerializableFLProgram pr = null;
                string key = "ParserPerformance+" + Path.GetFileNameWithoutExtension(file) + i;
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key,
                    iterations,
                    null,
                    () => setup.Parser.Process(new FLParserInput(file)),
                    null);
                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                setup.WriteResult(result);
            }
            logOut.AppendLine();
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }

        public static string RunParserInitBenchmark(int iterations)
        {

            FLSetup setup = new FLSetup("FL_ParserProcess_Performance", "resources/kernel");
            string performanceOut = "performance/FL_ParserInit_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");



            FLInstructionSet iset = null;
            BufferCreator bc = null;
            FLParser parser = null;
            FLProgramCheckPipeline checkPipeline = null;
            string key = "ParserInitPerformance";

            Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
            PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations, null,
                () =>
                {
                    iset = FLInstructionSet.CreateWithBuiltInTypes(setup.KernelDatabase);
                    bc = BufferCreator.CreateWithBuiltInTypes();
                    checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
                    parser = new FLParser(iset, bc, checkPipeline);
                }, null);

            logOut.AppendLine("\t" + result);
            Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }
    }
}
