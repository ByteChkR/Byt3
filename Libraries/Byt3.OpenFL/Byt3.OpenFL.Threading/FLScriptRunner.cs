using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;

namespace Byt3.OpenFL.Threading
{
    /// <summary>
    /// Single Threaded FL Runner Implementation
    /// </summary>
    public class FLScriptRunner
    {
        protected KernelDatabase Db;
        protected CLAPI Instance;

        protected Queue<FlScriptExecutionContext> ProcessQueue;
        public int ItemsInQueue => ProcessQueue.Count;

        public FLScriptRunner(
            CLAPI instance, DataVectorTypes dataVectorTypes = DataVectorTypes.Uchar1, string kernelFolder = "assets/kernel/")
        {
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
                FLParseResult ret = Process(fle);
                fle.OnFinishCallback?.Invoke(ret);
            }
        }

        protected FLParseResult Process(FlScriptExecutionContext context)
        {
            FLBufferInfo input = new FLBufferInfo(Instance, context.Input, context.Width, context.Height);

            FLParseResult parseResult = FLParser.Parse(new FLParserInput(context.Filename, Instance));
            
            parseResult.Run(Instance, Db, input);

            return parseResult;
        }
    }
}