using System;
using System.Collections.Generic;
using System.Drawing;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;

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
            CLAPI instance, DataTypes dataTypes = DataTypes.Uchar1, string kernelFolder = "assets/kernel/")
        {
            Instance = instance;
            Db = new KernelDatabase(instance, kernelFolder, dataTypes);
            ProcessQueue = new Queue<FlScriptExecutionContext>();
        }

        public virtual void Enqueue(FlScriptExecutionContext context)
        {
            ProcessQueue.Enqueue(context);
        }

        public virtual void Process(Action onFinish = null)
        {
            while (ProcessQueue.Count != 0)
            {
                FlScriptExecutionContext fle = ProcessQueue.Dequeue();
                Dictionary<string, byte[]> ret = Process(fle);
                Dictionary<Bitmap, byte[]> texMap = new Dictionary<Bitmap, byte[]>();
                foreach (KeyValuePair<string, byte[]> bytese in ret)
                {
                    if (fle.TextureMap.ContainsKey(bytese.Key))
                    {
                        texMap.Add(fle.TextureMap[bytese.Key], bytese.Value);
                    }
                }

                foreach (KeyValuePair<Bitmap, byte[]> textureUpdate in texMap)
                {
                    CLAPI.UpdateBitmap(CLAPI.MainThread, textureUpdate.Key, textureUpdate.Value);
                }

                fle.OnFinishCallback?.Invoke(texMap);
            }

            onFinish?.Invoke();
        }

        protected Dictionary<string, byte[]> Process(FlScriptExecutionContext context)
        {
            MemoryBuffer buf = CLAPI.CreateBuffer(Instance, context.Input, MemoryFlag.ReadWrite);
            FLInterpreter ret = new FLInterpreter(Instance, context.Filename, buf, context.Width,
                context.Height, 1, 4, Db, true);

            do
            {
                ret.Step();
            } while (!ret.Terminated);


            byte[] buffer = ret.GetResult<byte>();
            Dictionary<string, byte[]> result = new Dictionary<string, byte[]> {{"result", buffer}};

            foreach (KeyValuePair<string, Bitmap> keyValuePair in context.TextureMap)
            {
                CLBufferInfo mbuf = ret.GetBuffer(keyValuePair.Key);
                if (mbuf == null)
                {
                    continue;
                }

                byte[] spec = CLAPI.ReadBuffer<byte>(Instance, mbuf.Buffer, (int) mbuf.Buffer.Size);
                result.Add(keyValuePair.Key, spec);
                mbuf.Buffer.Dispose();
            }

            return result;
        }
    }
}