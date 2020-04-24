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
        private string RunResultPath => Path.Combine($"performance", typeof(OpenFLDebugConfig).Assembly.GetName().Version.ToString());
        private string PerformanceOutputFile => $"performance/{testName}.log";
        private string DataOutputDirectory => Path.Combine(RunResultPath, $"data");

        private readonly string testName;

        public readonly KernelDatabase KernelDatabase;
        public readonly FLInstructionSet InstructionSet;
        public readonly BufferCreator BufferCreator;
        public readonly FLProgramCheckPipeline CheckPipeline;
        public readonly FLParser Parser;

        public FLSetup(string testName, string kernelPath)
        {
            this.testName = testName;
            KernelDatabase = new KernelDatabase(CLAPI.MainThread, kernelPath, DataVectorTypes.Uchar1);
            InstructionSet = FLInstructionSet.CreateWithBuiltInTypes(KernelDatabase);
            BufferCreator = BufferCreator.CreateWithBuiltInTypes();
            CheckPipeline = FLProgramCheckPipeline.CreateDefaultCheckPipeline(InstructionSet, BufferCreator);
            Parser = new FLParser(InstructionSet, BufferCreator, CheckPipeline);

            Directory.CreateDirectory(RunResultPath);
            Directory.CreateDirectory(DataOutputDirectory);
        }

        public void Dispose()
        {
            KernelDatabase.Dispose();
        }

        public void WriteLog(string log)
        {
#if DEBUG
            File.WriteAllText(PerformanceOutputFile, log);
#endif
        }

        public Stream GetDataFileStream(string filename)
        {
#if DEBUG
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