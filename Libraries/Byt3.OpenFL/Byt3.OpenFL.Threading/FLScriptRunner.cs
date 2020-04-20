using System.Collections.Generic;
using System.IO;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.OpenFL.Serialization;

namespace Byt3.OpenFL.Threading
{
    /// <summary>
    /// Single Threaded FL Runner Implementation
    /// </summary>
    public class FLScriptRunner
    {
        protected FLRunner Runner;
        protected FLParser Parser;
        protected FLInstructionSet InstructionSet;
        protected BufferCreator BufferCreator;
        protected KernelDatabase Db;
        protected CLAPI Instance;

        protected Queue<FlScriptExecutionContext> ProcessQueue;
        public int ItemsInQueue => ProcessQueue.Count;

        private string kernelPath;

        public FLScriptRunner(CLAPI instance, DataVectorTypes dataVectorTypes = DataVectorTypes.Uchar1,
            string kernelFolder = "kernel/")
        {
            kernelPath = kernelFolder;
            InstructionSet = FLInstructionSet.CreateWithBuiltInTypes(kernelFolder);
            BufferCreator = BufferCreator.CreateWithBuiltInTypes();
            Runner = new FLRunner(InstructionSet);
            Parser = new FLParser(InstructionSet, BufferCreator);
            
            Instance = instance;
            Db = new KernelDatabase(instance, kernelFolder, dataVectorTypes);
            ProcessQueue = new Queue<FlScriptExecutionContext>();
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
            FLBuffer input = new FLBuffer(Instance, context.Input, context.Width, context.Height);

            FLProgram program;
            if (context.IsCompiled)
            {
                Stream s = File.OpenRead(context.Filename);
                program = FLSerializer.LoadProgram(s).Initialize(InstructionSet);
                s.Close();
            }
            else
            {
                program = Parser.Process(new FLParserInput(context.Filename)).Initialize(InstructionSet);
            }

            program.Run(Instance, input);

            return program;

            //FLProgram parseResult = FLParserParse(new FLParserInput(context.Filename, Instance));

            //parseResult.Run(Instance, Db, input);

            //return parseResult;
        }
    }
}