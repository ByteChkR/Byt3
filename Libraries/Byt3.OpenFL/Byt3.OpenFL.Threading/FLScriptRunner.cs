using System;
using System.Collections.Generic;
using System.IO;
using Byt3.Callbacks;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.Parsing.StageResults;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Serialization;

namespace Byt3.OpenFL.Threading
{
    /// <summary>
    /// Single Threaded FL Runner Implementation
    /// </summary>
    public class FLScriptRunner : IDisposable
    {
        protected BufferCreator BufferCreator;
        protected KernelDatabase Db;
        protected CLAPI Instance;
        protected FLInstructionSet InstructionSet;
        protected FLParser Parser;

        protected Queue<FlScriptExecutionContext> ProcessQueue;

        public FLScriptRunner(CLAPI instance, KernelDatabase dataBase, BufferCreator creator,
            FLInstructionSet instructionSet, FLProgramCheckBuilder checkBuilder, WorkItemRunnerSettings runnerSettings)
        {
            Db = dataBase;
            InstructionSet = instructionSet;
            BufferCreator = creator;

            Parser = new FLParser(InstructionSet, BufferCreator, runnerSettings);
            ProgramChecks = checkBuilder;
            checkBuilder.Attach(Parser, true);

            Instance = instance;
            ProcessQueue = new Queue<FlScriptExecutionContext>();
        }

        public FLScriptRunner(CLAPI instance, DataVectorTypes dataVectorTypes = DataVectorTypes.Uchar1,
            string kernelFolder = "resources/kernel")
        {
            Db = new KernelDatabase(instance, kernelFolder, dataVectorTypes);
            InstructionSet = FLInstructionSet.CreateWithBuiltInTypes(Db);
            BufferCreator = BufferCreator.CreateWithBuiltInTypes();
            ProgramChecks = FLProgramCheckBuilder.CreateDefaultCheckBuilder(InstructionSet, BufferCreator);
            Parser = new FLParser(InstructionSet, BufferCreator);
            ProgramChecks.Attach(Parser, true);
            Instance = instance;
            ProcessQueue = new Queue<FlScriptExecutionContext>();
        }

        public int ItemsInQueue => ProcessQueue.Count;
        private FLProgramCheckBuilder ProgramChecks { get; }

        public virtual void Dispose()
        {
            Db?.Dispose();
        }

        public void AddProgramCheck(FLProgramCheck check)
        {
            if (ProgramChecks.IsAttached)
            {
                ProgramChecks.Detach(false);
            }

            ProgramChecks.AddProgramCheck(check);
            ProgramChecks.Attach(Parser, true);
        }

        public virtual void Enqueue(FlScriptExecutionContext context)
        {
            ProcessQueue.Enqueue(context);
        }

        public virtual void Process()
        {
            while (ProcessQueue.Count != 0)
            {
                FlScriptExecutionContext fle = ProcessQueue.Dequeue();
                FLProgram ret = Process(fle);
                fle.OnFinishCallback?.Invoke(ret);
            }
        }

        protected FLProgram Process(FlScriptExecutionContext context)
        {
            FLBuffer input = new FLBuffer(Instance, context.Input, context.Width, context.Height, context.Depth, context.Filename);

            FLProgram program;
            if (context.IsCompiled)
            {
                Stream s = IOManager.GetStream(context.Filename);
                program = FLSerializer.LoadProgram(s, InstructionSet).Initialize(Instance, InstructionSet);
                s.Close();
            }
            else
            {
                program = Parser.Process(new FLParserInput(context.Filename)).Initialize(Instance, InstructionSet);
            }

            program.Run(input, true);

            return program;
        }
    }
}