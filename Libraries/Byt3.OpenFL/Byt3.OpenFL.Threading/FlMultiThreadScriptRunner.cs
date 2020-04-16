using System;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Parsing;

namespace Byt3.OpenFL.Threading
{
    /// <summary>
    /// Implementation of the Runner Class that executes FL Scripts on a different thread
    /// </summary>
    public class FlMultiThreadScriptRunner : FLScriptRunner
    {
        //private GameWindow window;

        public FlMultiThreadScriptRunner(Action onFinishQueueCallback,
            DataVectorTypes dataVectorTypes = DataVectorTypes.Uchar1, string kernelFolder = "assets/kernel/") : base(
            CLAPI.GetInstance(), dataVectorTypes, kernelFolder)
        {
        }

        public override void Process()
        {
            ThreadManager.RunTask(_proc, x =>
            {
                
            });
        }

        private object _proc()
        {

            while (ProcessQueue.Count != 0)
            {
                FlScriptExecutionContext fle = ProcessQueue.Dequeue();
                FLParseResult texUpdate = Process(fle);
                fle.OnFinishCallback?.Invoke(texUpdate);
            }

            return new object();
        }
    }
}