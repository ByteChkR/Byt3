﻿using System.Collections.Generic;
using System.IO;
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
            string[] dirs = new[] {"resources/filter/tests"};
            List<string> files = new List<string> {"resources/filter/game/tennisball.fl"};
            for (int i = 0; i < dirs.Length; i++)
            {
                files.AddRange(Directory.GetFiles(dirs[i], "*.fl", SearchOption.TopDirectoryOnly));
            }

            return files;
        }


        [Test]
        public void OpenFL_Program_Serialization_Test()
        {
            TestSetup.SetupLogOutput();
            OpenFLBenchmarks.RunProgramSerializationBenchmark(GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Program_Deserialization_Test()
        {
            TestSetup.SetupLogOutput();
            OpenFLBenchmarks.RunProgramDeserializationBenchmark(GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Parser_Initialization_Test()
        {
            TestSetup.SetupLogOutput();
            OpenFLBenchmarks.RunParserInitBenchmark(IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Parser_Parsing_Test()
        {
            TestSetup.SetupLogOutput();
            OpenFLBenchmarks.RunParserProcessBenchmark(GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Program_Initialization_Test()
        {
            TestSetup.SetupLogOutput();
            OpenFLBenchmarks.RunProgramInitBenchmark(GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_ParsedProgram_Execution_Test()
        {
            TestSetup.SetupLogOutput();
            OpenFLBenchmarks.RunParsedFLExecutionBenchmark(GetFiles(), EXECUTION_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_DeserializedProgram_Execution_Test()
        {
            TestSetup.SetupLogOutput();
            OpenFLBenchmarks.RunDeserializedFLExecutionBenchmark(GetFiles(), EXECUTION_BENCHMARK_ITERATIONS);
        }
    }
}