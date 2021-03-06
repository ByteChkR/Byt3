﻿using System;
using System.IO;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.Utilities.Benchmarking;

namespace Byt3.OpenFL.Benchmarking
{

    public class FLSetup : BenchmarkHelper, IDisposable
    {
        public readonly KernelDatabase KernelDatabase;
        public readonly FLInstructionSet InstructionSet;
        public readonly BufferCreator BufferCreator;
        public FLProgramCheckBuilder CheckBuilder;
        public FLParser Parser;

        public FLSetup(string testName, string kernelPath, string performance = "performance", Type[] checkPipeline = null,
            bool useMultiThreading = false, int workSizeMultiplier = 2) : base(testName, performance)
        {
            KernelDatabase = new KernelDatabase(CLAPI.MainThread, kernelPath, DataVectorTypes.Uchar1);
            InstructionSet = FLInstructionSet.CreateWithBuiltInTypes(KernelDatabase);
            BufferCreator = BufferCreator.CreateWithBuiltInTypes();
            CheckBuilder = null;
            if (checkPipeline == null)
            {
                CheckBuilder = FLProgramCheckBuilder.CreateDefaultCheckBuilder(InstructionSet, BufferCreator);
            }
            else
            {
               CheckBuilder= new FLProgramCheckBuilder(InstructionSet, BufferCreator);
               foreach (Type useCheck in checkPipeline)
               {
                   CheckBuilder.AddProgramCheck((FLProgramCheck)Activator.CreateInstance(useCheck));
               }
            }


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


    }
}