using System.Collections.Generic;
using Byt3.Callbacks;
using Byt3.OpenFL.Benchmarking;
using NUnit.Framework;

namespace Byt3.OpenFL.Tests
{
    public class FL_Common_Tests
    {
        private const int EXECUTION_BENCHMARK_ITERATIONS = 1;
        private const int IO_BENCHMARK_ITERATIONS = 50;

        private static List<string> GetFiles()
        {
            string[] dirs = {"resources/filter/tests"};
            List<string> files = new List<string>();
            for (int i = 0; i < dirs.Length; i++)
            {
                files.AddRange(IOManager.GetFiles(dirs[i], "*.fl"));
            }

            return files;
        }

        [Test]
        public void OpenFL_Program_Serialization_Test()
        {
            TestSetup.SetupTestingSession();
            OpenFLBenchmarks.RunProgramSerializationBenchmark("_test", GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Program_Deserialization_Test()
        {
            TestSetup.SetupTestingSession();
            OpenFLBenchmarks.RunProgramDeserializationBenchmark("_test", GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Parser_Initialization_Test()
        {
            TestSetup.SetupTestingSession();
            OpenFLBenchmarks.RunParserInitBenchmark("_test", IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Parser_Parsing_Test()
        {
            TestSetup.SetupTestingSession();
            OpenFLBenchmarks.RunParserProcessBenchmark("_test", GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Program_Initialization_Test()
        {
            TestSetup.SetupTestingSession();
            OpenFLBenchmarks.RunProgramInitBenchmark("_test", GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_ParsedProgram_Execution_Test()
        {
            TestSetup.SetupTestingSession();
            OpenFLBenchmarks.RunParsedFLExecutionBenchmark(true, "_test", GetFiles(), EXECUTION_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_DeserializedProgram_Execution_Test()
        {
            TestSetup.SetupTestingSession();
            OpenFLBenchmarks.RunDeserializedFLExecutionBenchmark("_test", GetFiles(), EXECUTION_BENCHMARK_ITERATIONS);
        }
    }
}