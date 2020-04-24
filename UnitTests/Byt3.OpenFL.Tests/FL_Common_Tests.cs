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
using Byt3.OpenFL.Benchmarking;
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
    public class FL_Common_Tests
    {
        private const int EXECUTION_BENCHMARK_ITERATIONS = 1;
        private const int IO_BENCHMARK_ITERATIONS = 50;

        private static List<string> GetFiles()
        {
            string[] dirs = new[] { "resources/filter/tests" };
            List<string> files = new List<string> { "resources/filter/game/tennisball.fl" };
            for (int i = 0; i < dirs.Length; i++)
            {
                files.AddRange(Directory.GetFiles(dirs[i], "*.fl", SearchOption.TopDirectoryOnly));
            }

            return files;
        }


        private void SetupLogOutput()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
        }

        [Test]
        public void OpenFL_Program_Serialization_Test()
        {
            SetupLogOutput();
            OpenFLBenchmarks.RunProgramSerializationBenchmark(GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Program_Deserialization_Test()
        {
            SetupLogOutput();
            OpenFLBenchmarks.RunProgramDeserializationBenchmark(GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Parser_Initialization_Test()
        {
            SetupLogOutput();
            OpenFLBenchmarks.RunParserInitBenchmark(IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Parser_Parsing_Test()
        {
            SetupLogOutput();
            OpenFLBenchmarks.RunParserProcessBenchmark(GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_Program_Initialization_Test()
        {
            SetupLogOutput();
            OpenFLBenchmarks.RunProgramInitBenchmark(GetFiles(), IO_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_ParsedProgram_Execution_Test()
        {
            SetupLogOutput();
            OpenFLBenchmarks.RunParsedFLExecutionBenchmark(GetFiles(), EXECUTION_BENCHMARK_ITERATIONS);
        }

        [Test]
        public void OpenFL_DeserializedProgram_Execution_Test()
        {
            SetupLogOutput();
            OpenFLBenchmarks.RunDeserializedFLExecutionBenchmark(GetFiles(), EXECUTION_BENCHMARK_ITERATIONS);
        }

    }
}