using System;
using System.IO;
using System.Xml.Serialization;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;

namespace Byt3.OpenFL.Benchmarking
{
    public struct FLSetup : IDisposable
    {
        private readonly string PerformanceFolder;

        private string RunResultPath => Path.Combine(PerformanceFolder,
            typeof(OpenFLDebugConfig).Assembly.GetName().Version.ToString());

        private string PerformanceOutputFile => Path.Combine(PerformanceFolder, $"{testName}.log");
        private string DataOutputDirectory => Path.Combine(RunResultPath, $"data");

        private readonly string testName;

        public readonly KernelDatabase KernelDatabase;
        public readonly FLInstructionSet InstructionSet;
        public readonly BufferCreator BufferCreator;
        public FLProgramCheckBuilder CheckBuilder;
        public FLParser Parser;


        public FLSetup(string testName, string kernelPath, string performance = "performance", bool useChecks = true,
            bool useMultiThreading = false, int workSizeMultiplier = 2)
        {
            this.testName = testName;
            PerformanceFolder = performance;
            KernelDatabase = new KernelDatabase(CLAPI.MainThread, kernelPath, DataVectorTypes.Uchar1);
            InstructionSet = FLInstructionSet.CreateWithBuiltInTypes(KernelDatabase);
            BufferCreator = BufferCreator.CreateWithBuiltInTypes();
            CheckBuilder =
                useChecks
                    ? FLProgramCheckBuilder.CreateDefaultCheckBuilder(InstructionSet, BufferCreator)
                    : new FLProgramCheckBuilder(InstructionSet, BufferCreator);
            Parser = new FLParser(InstructionSet, BufferCreator,
                new WorkItemRunnerSettings(useMultiThreading, workSizeMultiplier));
            CheckBuilder.Attach(Parser, true);

            Directory.CreateDirectory(RunResultPath);
            Directory.CreateDirectory(DataOutputDirectory);
        }

        public void SetCheckBuilder(FLProgramCheckBuilder checkBuilder, bool attach)
        {
            if (CheckBuilder != null && CheckBuilder.IsAttached)
            {
                CheckBuilder.Detach(false);
            }

            CheckBuilder = checkBuilder;
            if (attach)
            {
                CheckBuilder.Attach(Parser, true);
            }
        }

        public void Dispose()
        {
            KernelDatabase.Dispose();
        }

        public void WriteLog(string log)
        {
#if DEBUG
            File.AppendAllText(PerformanceOutputFile, log);
#endif
        }

        public Stream GetDataFileStream(string filename)
        {
#if DEBUG
            Directory.CreateDirectory(Path.Combine(DataOutputDirectory, Path.GetDirectoryName(filename)));
            return File.Create(Path.Combine(DataOutputDirectory, filename));
#else
            return new MemoryStream();
#endif
        }

        public void WriteResult(PerformanceTester.PerformanceResult result)
        {
#if DEBUG
            XmlSerializer xs = new XmlSerializer(typeof(PerformanceTester.PerformanceResult));
            Stream s = File.Create(Path.Combine(RunResultPath, $"{result.TestName}.xml"));
            xs.Serialize(s, result);
            s.Close();
#endif
        }
    }
}