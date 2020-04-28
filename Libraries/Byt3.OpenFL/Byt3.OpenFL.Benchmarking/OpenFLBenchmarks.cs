using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.OpenFL.Serialization;
using Byt3.Utilities.ManifestIO;

namespace Byt3.OpenFL.Benchmarking
{
    public static class OpenFLBenchmarks
    {
#if DEBUG
        private const int BITMAP_RESOLUTION = 256;
#else
        private const int BITMAP_RESOLUTION = 32;
#endif


        public static void InitializeTestRun(string performanceFolder)
        {

            ManifestReader.RegisterAssembly(Assembly.GetExecutingAssembly());
            ManifestReader.RegisterAssembly(Assembly.GetAssembly(typeof(KernelFLInstruction))); //Load the Embedded files from OpenFL.Common(the built in kernels)
            EmbeddedFileIOManager.Initialize();
            ManifestReader.PrepareManifestFiles(false);

            string p =Path.Combine(performanceFolder, "runs.txt");
            if (!File.Exists(p))
            {
                Directory.CreateDirectory(performanceFolder);
                File.WriteAllText(p,"0");
                return;
            }
            string runs = File.ReadAllText(p);
            FLSetup.RunNumber = int.Parse(runs);
        }

        public static void FinalizeTestRun(string performanceFolder)
        {
            string p = Path.Combine(performanceFolder, "runs.txt");
            if (File.Exists(p))
                File.Delete(p);
            File.WriteAllText(p, (FLSetup.RunNumber+1).ToString());
        }

        private static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(new ProjectDebugConfig(
            "FL Benchmark", -1, 10,
            PrefixLookupSettings.AddPrefixIfAvailable));

        private static void SaveOutput(string subcategory, Bitmap bmp, FLProgram program, FLSetup setup, string file)
        {
            FLBuffer outP = program.GetActiveBuffer(false);
            CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, outP.Buffer);
            outP.Dispose();
            string p = Path.GetFileNameWithoutExtension(file) + ".png";
            Stream bmpStream = setup.GetDataFileStream(Path.Combine(subcategory, p));
            bmp.Save(bmpStream, ImageFormat.Png);
            bmpStream.Close();
        }


        #region Execution

        public static string RunParsedFLExecutionBenchmark(string testAdd, List<string> files, int iterations,
            string performanceFolder = "performance", bool useChecks = true, bool useMultiThreading = false,
            int workSizeMultiplier = 2)
        {
            FLSetup setup = new FLSetup("FL_ParsedExecution_Performance" + testAdd, "resources/kernel", performanceFolder,
                useChecks, useMultiThreading, workSizeMultiplier);
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            for (int i = 0; i < files.Count; i++)
            {
                Logger.Log(LogType.Log, "Running Script: " + files[i], 1);
                Bitmap bmp = null;
                FLBuffer buf = null;
                SerializableFLProgram parsedProgram = null;
                FLProgram program = null;

                string key = "FLParsedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + "." + i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                parsedProgram = setup.Parser.Process(new FLParserInput(files[i]));

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                    (int its) => //BeforeTest
                    {
                        bmp = new Bitmap(BITMAP_RESOLUTION, BITMAP_RESOLUTION);
                        buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);
                        program = parsedProgram.Initialize(setup.InstructionSet);
                    },
                    (int its) => { program.Run(CLAPI.MainThread, buf, true); },
                    (int its) => //After Test
                    {
                        Logger.Log(LogType.Log, "------------------------Run Finished------------------------", 1);

                        if (its == iterations - 1)
                        {
                            SaveOutput("parsed-output", bmp, program, setup, files[i]);
                        }

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

        public static string RunDeserializedFLExecutionBenchmark(string testAdd, List<string> files, int iterations,
            string performanceFolder = "performance", bool useChecks = true, bool useMultiThreading = false,
            int workSizeMultiplier = 2)
        {
            FLSetup setup = new FLSetup("FL_DeserializedExecution_Performance" + testAdd, "resources/kernel", performanceFolder,
                useChecks, useMultiThreading, workSizeMultiplier);
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            for (int i = 0; i < files.Count; i++)
            {
                Bitmap bmp = null;
                FLBuffer buf = null;
                SerializableFLProgram parsedProgram = null;
                MemoryStream ms = new MemoryStream();
                FLProgram program = null;
                string key = "FLDeserializedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + "." +
                             i;

                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                parsedProgram = setup.Parser.Process(new FLParserInput(files[i]));
                FLSerializer.SaveProgram(ms, parsedProgram, new string[0]);

                ms.Position = 0;
                parsedProgram = FLSerializer.LoadProgram(ms);

                ms.Dispose();
                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                    (int its) => //BeforeTest
                    {
                        bmp = new Bitmap(BITMAP_RESOLUTION, BITMAP_RESOLUTION);
                        buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);
                        program = parsedProgram.Initialize(setup.InstructionSet);
                    },
                    (int its) => program.Run(CLAPI.MainThread, buf, true),
                    (int its) => //After Test
                    {
                        if (its == iterations - 1)
                        {
                            SaveOutput("deserialized-output", bmp, program, setup, files[i]);
                        }

                        program.FreeResources();
                        bmp.Dispose();
                        buf.Dispose();
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

        #endregion

        public static string RunProgramSerializationBenchmark(string testAdd, List<string> files, int iterations,
            string[] extraSteps = null,
            string performanceFolder = "performance", bool useChecks = true, bool useMultiThreading = false,
            int workSizeMultiplier = 2)
        {
            FLSetup setup = new FLSetup("FL_SerializationProcess_Performance" + testAdd, "resources/kernel", performanceFolder,
                useChecks, useMultiThreading, workSizeMultiplier);

            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            string[] steps = extraSteps ?? new string[0];

            for (int i = 0; i < files.Count; i++)
            {
                MemoryStream dst = new MemoryStream();
                SerializableFLProgram pr = setup.Parser.Process(new FLParserInput(files[i]));
                string key = "ProgramSerializationPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + "." + i;


                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                    (int its) => dst.Position = 0,
                    (int its) => FLSerializer.SaveProgram(dst, pr, steps),
                    null);

                dst.Position = 0;
                Stream s = setup.GetDataFileStream(Path.Combine("serialized-scripts", key + ".flc"));
                dst.CopyTo(s);
                dst.Close();
                s.Close();
                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                setup.WriteResult(result);
            }

            logOut.AppendLine();
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }

        public static string RunProgramDeserializationBenchmark(string testAdd, List<string> files, int iterations,
            string performanceFolder = "performance", bool useChecks = true, bool useMultiThreading = false,
            int workSizeMultiplier = 2)
        {
            FLSetup setup = new FLSetup("FL_DeserializationProcess_Performance" + testAdd, "resources/kernel", performanceFolder,
                useChecks, useMultiThreading, workSizeMultiplier);


            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            MemoryStream dst = new MemoryStream();
            for (int i = 0; i < files.Count; i++)
            {
                SerializableFLProgram pr = setup.Parser.Process(new FLParserInput(files[i]));
                string key = "ProgramDeserializationPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + "." +
                             i;
                FLSerializer.SaveProgram(dst, pr, new string[] { });
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);

                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                    (int its) => dst.Position = 0,
                    (int its) => FLSerializer.LoadProgram(dst),
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

        public static string RunProgramInitBenchmark(string testAdd, List<string> files, int iterations,
            string performanceFolder = "performance", bool useChecks = true, bool useMultiThreading = false,
            int workSizeMultiplier = 2)
        {
            FLSetup setup = new FLSetup("FL_ProgramInit_Performance" + testAdd, "resources/kernel", performanceFolder, useChecks,
                useMultiThreading, workSizeMultiplier);
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            for (int i = 0; i < files.Count; i++)
            {
                //Untimed
                SerializableFLProgram pr = setup.Parser.Process(new FLParserInput(files[i]));
                FLProgram program = null;

                string key = "ProgramInitPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + "." + i;
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations,
                    null,
                    (int its) => { program = pr.Initialize(setup.InstructionSet); },
                    (int its) => program.FreeResources());

                logOut.AppendLine("\t" + result);
                Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
                setup.WriteResult(result);
            }

            logOut.AppendLine();
            setup.WriteLog(logOut.ToString());
            setup.Dispose();
            return logOut.ToString();
        }

        public static string RunParserProcessBenchmark(string testAdd, List<string> files, int iterations,
            string performanceFolder = "performance", bool useChecks = true, bool useMultiThreading = false,
            int workSizeMultiplier = 2)
        {
            FLSetup setup = new FLSetup("FL_ParserProcess_Performance"+testAdd, "resources/kernel", performanceFolder,
                useChecks, useMultiThreading, workSizeMultiplier);
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                string key = "ParserPerformance+" + Path.GetFileNameWithoutExtension(file) + "." + i;
                Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
                PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key,
                    iterations,
                    null,
                    (int its) => setup.Parser.Process(new FLParserInput(file)),
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

        public static string RunParserInitBenchmark(string testAdd, int iterations,
            string performanceFolder = "performance", bool useChecks = true, bool useMultiThreading = false,
            int workSizeMultiplier = 2)
        {
            //FLSetup setup = new FLSetup("FL_ParserProcess_Performance"+ testAdd, "resources/kernel", performanceFolder,
            //    useChecks, useMultiThreading, workSizeMultiplier);
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");


            FLInstructionSet iset = null;
            BufferCreator bc = null;
            FLParser parser = null;
            FLProgramCheckBuilder checkBuilder = null;
            KernelDatabase db = new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            string key = "ParserInitPerformance";

            Logger.Log(LogType.Log, $"------------------------Run {key} Starting------------------------", 1);
            PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(key, iterations, null,
                (int its) =>
                {
                    iset = FLInstructionSet.CreateWithBuiltInTypes(db);
                    bc = BufferCreator.CreateWithBuiltInTypes();
                    checkBuilder = FLProgramCheckBuilder.CreateDefaultCheckBuilder(iset, bc);
                    parser = new FLParser(iset, bc);
                }, null);

            logOut.AppendLine("\t" + result);
            Logger.Log(LogType.Log, $"------------------------Run {key} Finished------------------------", 1);
            return logOut.ToString();
        }
    }
}