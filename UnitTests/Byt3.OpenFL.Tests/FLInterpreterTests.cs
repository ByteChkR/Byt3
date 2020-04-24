using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.DataTypes;
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
using Byt3.Utilities.Exceptions;
using NUnit.Framework;

namespace Byt3.OpenFL.Tests
{
    public class FLInterpreterTests
    {
        private const int EXECUTION_BENCHMARK_ITERATIONS = 1;
        private const int IO_BENCHMARK_ITERATIONS = 200;


        [Test]
        public void OpenFL_Comments_Test()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            string file =
                "resources/filter/comments/test.fl";

            FLParser parser = new FLParser();

            Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
            parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

            SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            //FLFunction entryPoint = pr.EntryPoint; //Provoking an exception if main function is not found
        }

        [Test]
        public void OpenFL_DefineFile_Wrong_Test()
        {
            string file = "resources/filter/defines/test_wrong_define_invalid_file.fl";
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;

            Assert.Catch<Byt3Exception>(() =>
            {
                BufferCreator bc = new BufferCreator();
                FLInstructionSet iset = new FLInstructionSet();
                FLProgramCheckPipeline checkPipeline = new FLProgramCheckPipeline(iset, bc);
                checkPipeline.AddSubStage(new FilePathValidator());

                FLParser parser = new FLParser(iset, bc, checkPipeline);

                Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            });
        }

        [Test]
        public void OpenFL_Defines_Test()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            string file = Path.GetFullPath("resources/filter/defines/test.fl");

            FLParser parser = new FLParser();

            Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
            parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

            SerializableFLProgram result = parser.Process(new FLParserInput(file));


            Assert.True(result.DefinedBuffers.Count == 5);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "in") == 1);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "textureD") == 1);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "textureC") == 1);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "textureB") == 1);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "textureA") == 1);
        }

        [Test]
        public void OpenFL_DefineScriptFile_Wrong_Test()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            string file = "resources/filter/defines/test_wrong_script_invalid_file.fl";
            Assert.Catch<Byt3Exception>(() =>
            {
                FLParser parser = new FLParser();

                Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            });
        }


        [Test]
        public void OpenFL_DefineScriptNoFile_Wrong_Test()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            string file = "resources/filter/defines/test_wrong_script_.fl";
            Assert.Catch<Byt3Exception>(() =>
            {
                FLParser parser = new FLParser();

                Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            });
        }

        [Test]
        //[Repeat(5)]
        public void OpenFL_Parser_Test()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;

            ADL.Debug.DefaultInitialization();
            string path = "resources/filter/tests";
            List<string> files = Directory.GetFiles(path, "*.fl", SearchOption.TopDirectoryOnly).ToList();
            files.Add("resources/filter/game/tennisball.fl");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);

            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);


            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();

            FLParser parser = new FLParser(iset, bc);

            string performanceOut = "performance/FL_ParsedExec_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");



#if DEBUG
            Directory.CreateDirectory("./out/image");
#endif

            for (int i = 0; i < files.Count; i++)
            {
#if DEBUG
                Bitmap bmp = new Bitmap(128, 128);
#else
                Bitmap bmp = new Bitmap(32, 32);
#endif
                FLBuffer buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);

                SerializableFLProgram parsedProgram = parser.Process(new FLParserInput(files[i]));

                FLProgram program = parsedProgram.Initialize(iset);

                Stopwatch sw = Stopwatch.StartNew();
                program.Run(CLAPI.MainThread, buf);
                sw.Stop();
                decimal result = (decimal)Math.Round(sw.Elapsed.TotalMilliseconds, 4);

                string key = "FLParsedExecutionPerformance+" + Path.GetFileNameWithoutExtension(files[i]) + i;
                bool matched = PerformanceTester.Tester.MatchesTarget(key, result, out decimal deltaFromTarget, out decimal targetActual);
                logOut.AppendLine($"\t{Path.GetFileNameWithoutExtension(files[i]) }+{i}: {result}ms ({Math.Round(result / targetActual * 100, 4)}%); Target Matched: {matched}; Target: {targetActual}ms; Delta: {deltaFromTarget}ms");



#if DEBUG
                CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, program.ActiveBuffer.Buffer);


                string p = Path.Combine("./out/image", Path.GetFileNameWithoutExtension(files[i]) + ".png");
                bmp.Save(p);
#endif
                program.FreeResources();
                buf.Dispose();
                bmp.Dispose();
            }
            db.Dispose();
            logOut.AppendLine();
            File.AppendAllText(performanceOut, logOut.ToString());
        }

        [Test]
        public void OpenFL_IO_Performance_Tests()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            string[] dirs = new[] { "resources/filter/tests" };
            List<string> files = new List<string>();
            files.Add("resources/filter/game/tennisball.fl");
            for (int i = 0; i < dirs.Length; i++)
            {
                files.AddRange(Directory.GetFiles(dirs[i], "*.fl", SearchOption.TopDirectoryOnly));
            }

            Directory.CreateDirectory("performance");

            string performanceOut = "performance/FL_IO_Performance.log";
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            bool matchedAll = true;
            for (int j = 0; j < files.Count; j++)
            {
                string path = files[j];

                logOut.AppendLine($"\tPerformance Tests: {path}");


                int totalIterations = 200;
                Stopwatch sw = new Stopwatch();
                FLInstructionSet iset;
                BufferCreator bc;
                FLParser parser;
                FLProgramCheckPipeline checkPipeline;

                //Untimed Initialization
                iset = FLInstructionSet.CreateWithBuiltInTypes(db);
                bc = BufferCreator.CreateWithBuiltInTypes();
                checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
                parser = new FLParser(iset, bc, checkPipeline);

                sw.Start();
                //Initialization
                for (int i = 0; i < totalIterations; i++)
                {
                    iset = FLInstructionSet.CreateWithBuiltInTypes(db);
                    bc = BufferCreator.CreateWithBuiltInTypes();
                    checkPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc);
                    parser = new FLParser(iset, bc, checkPipeline);
                }
                sw.Stop();
                decimal delta = (decimal)sw.ElapsedTicks / totalIterations;
                decimal deltaFromTarget;
                decimal targetActual;
                bool matched = PerformanceTester.Tester.MatchesTarget("ParserInitPerformance+" + Path.GetFileNameWithoutExtension(path) + j, delta, out deltaFromTarget, out targetActual);
                matchedAll &= matched;
                logOut.AppendLine($"\t\tParser Init Performance(N = {totalIterations}): {sw.ElapsedTicks} ticks; Target Matched: {matched}; Result: {delta}({Math.Round(delta / targetActual * 100, 4)}%) ticks; Target: {targetActual} ticks; Delta: {deltaFromTarget}");

                //Untimed

                SerializableFLProgram pr;

                sw.Restart();
                for (int i = 0; i < totalIterations; i++)
                {
                    pr = parser.Process(new FLParserInput(path));
                }
                pr = parser.Process(new FLParserInput(path));

                sw.Stop();
                delta = (decimal)sw.ElapsedTicks / totalIterations;
                matched = PerformanceTester.Tester.MatchesTarget("ParserPerformance+" + Path.GetFileNameWithoutExtension(path) + j, delta, out deltaFromTarget, out targetActual);
                matchedAll &= matched;
                logOut.AppendLine($"\t\tParser Performance(N = {totalIterations}): {sw.ElapsedTicks} ticks; Target Matched: {matched}; Result: {delta}({Math.Round(delta / targetActual * 100, 4)}%) ticks; Target: {targetActual} ticks; Delta: {deltaFromTarget}");

                //Untimed
                FLProgram program;

                sw.Restart();
                for (int i = 0; i < totalIterations; i++)
                {
                    program = pr.Initialize(iset);
                    program.FreeResources();
                }
                sw.Stop();
                delta = (decimal)sw.ElapsedTicks / totalIterations;
                matched = PerformanceTester.Tester.MatchesTarget("ProgramInitPerformance+" + Path.GetFileNameWithoutExtension(path) + j, delta, out deltaFromTarget, out targetActual);
                matchedAll &= matched;
                logOut.AppendLine($"\t\tProgram Init Performance(N = {totalIterations}): {sw.ElapsedTicks} ticks; Target Matched: {matched}; Result: {delta}({Math.Round(delta / targetActual * 100, 4)}%) ticks; Target: {targetActual} ticks; Delta: {deltaFromTarget}");

                MemoryStream dst = new MemoryStream();

                sw.Restart();
                for (int i = 0; i < totalIterations; i++)
                {
                    FLSerializer.SaveProgram(dst, pr, new string[] { });
                    dst.Position = 0;
                }
                sw.Stop();
                delta = (decimal)sw.ElapsedTicks / totalIterations;
                matched = PerformanceTester.Tester.MatchesTarget("ProgramSerializationPerformance+" + Path.GetFileNameWithoutExtension(path) + j, delta, out deltaFromTarget, out targetActual);
                matchedAll &= matched;
                logOut.AppendLine($"\t\tProgram Serialization Performance(N = {totalIterations}): {sw.ElapsedTicks} ticks; Target Matched: {matched}; Result: {delta}({Math.Round(delta / targetActual * 100, 4)}%) ticks; Target: {targetActual} ticks; Delta: {deltaFromTarget}");

                logOut.AppendLine();

            }

            db.Dispose();

            File.AppendAllText(performanceOut, logOut.ToString());

            Assert.True(matchedAll);

        }

        [Test]
        public void OpenFL_Serializer_Serialize_Tests()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            ADL.Debug.DefaultInitialization();
            string path = "resources/filter/tests";
            string[] files = Directory.GetFiles(path, "*.fl", SearchOption.TopDirectoryOnly);
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);

            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
            BufferCreator bc = BufferCreator.CreateWithBuiltInTypes();
            FLParser parser = new FLParser(iset, bc, FLProgramCheckPipeline.CreateDefaultCheckPipeline(iset, bc));

            Directory.CreateDirectory("./out/serialized");
#if DEBUG
            Directory.CreateDirectory("./out/image-serialized");
#endif

            for (int i = 0; i < files.Length; i++)
            {
#if DEBUG
                Bitmap bmp = new Bitmap(128, 128);
#else
                Bitmap bmp = new Bitmap(32, 32);
#endif

                FLBuffer buf = new FLBuffer(CLAPI.MainThread, bmp, files[i]);

                SerializableFLProgram pr = parser.Process(new FLParserInput(files[i]));
                string pCompiledOut =
                    Path.Combine("./out/serialized", Path.GetFileNameWithoutExtension(files[i]) + ".flc");


                Stream cs = File.OpenWrite(pCompiledOut);
                FLSerializer.SaveProgram(cs, pr, new string[] { });
                cs.Close();


                cs = File.OpenRead(pCompiledOut);
                SerializableFLProgram loaded = FLSerializer.LoadProgram(cs);
                cs.Close();


                FLProgram program = loaded.Initialize(iset);
                program.Run(CLAPI.MainThread, buf);


#if DEBUG
                CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, program.ActiveBuffer.Buffer);
                string p = Path.Combine("./out/image-serialized", Path.GetFileNameWithoutExtension(files[i]) + ".png");
                bmp.Save(p);
#endif

                program.FreeResources();
                buf.Dispose();
                bmp.Dispose();
            }
            db.Dispose();
        }

        [Test]
        public void OpenFL_WFCDefines_Wrong_Test()
        {
            string[] files = Directory.GetFiles("resources/filter/defines/", "test_wrong_define_wfc_*.fl");
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;

            foreach (string file in files)
            {
                Assert.Catch<Byt3Exception>(() =>
                {
                    FLParser parser = new FLParser();

                    Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                    parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                    SerializableFLProgram pr = parser.Process(new FLParserInput(file));
                });
            }
        }

        [Test]
        public void OpenFL_TypeConversion_Test()
        {
            float f = float.MaxValue / 2;
            byte b = (byte)CLTypeConverter.Convert(typeof(byte), f);
            float4 f4 = new float4(f);
            uchar4 i4 = (uchar4)CLTypeConverter.Convert(typeof(uchar4), f4);
            Assert.True(b == 128);

            for (int i = 0; i < 4; i++)
            {
                byte s = i4[i];
                Assert.True(s == 128);
            }
        }
    }
}