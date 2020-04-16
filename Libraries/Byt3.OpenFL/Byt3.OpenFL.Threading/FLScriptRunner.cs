using System;
using System.Collections.Generic;
using System.Drawing;
using Byt3.OpenCL.Memory;
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

            //FLInterpreter ret = new FLInterpreter(Instance, context.Filename, buf, context.Width,
            //    context.Height, 1, 4, Db, true);

            //do
            //{
            //    FLInterpreterStepResult step = ret.Step();
            //    //Console.WriteLine(step);
            //} while (!ret.Terminated);


            //byte[] buffer = ret.GetResult<byte>();
            //Dictionary<string, byte[]> result = new Dictionary<string, byte[]> { { "result", buffer } };

            //foreach (KeyValuePair<string, Bitmap> keyValuePair in context.TextureMap)
            //{
            //    CLBufferInfo mbuf = ret.GetBuffer(keyValuePair.Key);
            //    if (mbuf == null)
            //    {
            //        continue;
            //    }

            //    byte[] spec = CLAPI.ReadBuffer<byte>(Instance, mbuf.Buffer, (int)mbuf.Buffer.Size);
            //    result.Add(keyValuePair.Key, spec);
            //    mbuf.Buffer.Dispose();
            //}

            //return result;
        }
    }
}