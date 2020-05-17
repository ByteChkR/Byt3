using System;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Threading
{
    /// <summary>
    /// Implementation of the Runner Class that executes FL Scripts on a different thread
    /// </summary>
    public class FlMultiThreadScriptRunner : FLScriptRunner
    {
        private readonly Action OnFinishQueue;

        public FlMultiThreadScriptRunner(Action onFinishQueueCallback,
            DataVectorTypes dataVectorTypes = DataVectorTypes.Uchar1, string kernelFolder = "resources/kernel") : base(
            CLAPI.GetInstance(), dataVectorTypes, kernelFolder)
        {
            OnFinishQueue = onFinishQueueCallback;
        }

        public override void Process()
        {
            ThreadManager.RunTask(_proc, o => OnFinishQueue());
        }

        private object _proc()
        {
            while (ProcessQueue.Count != 0)
            {
                FlScriptExecutionContext fle = ProcessQueue.Dequeue();
                FLProgram texUpdate = Process(fle);
                fle.OnFinishCallback?.Invoke(texUpdate);
            }

            OnFinishQueue?.Invoke();
            return new object();
        }

        public override void Dispose()
        {
            base.Dispose();
            Instance.Dispose();
        }
    }
}