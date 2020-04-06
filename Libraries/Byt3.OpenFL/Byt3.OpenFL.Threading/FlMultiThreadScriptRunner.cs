using System;
using System.Collections.Generic;
using System.Drawing;
using Byt3.OpenCL.Common;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;

namespace Byt3.OpenFL.Threading
{

    /// <summary>
    /// Implementation of the Runner Class that executes FL Scripts on a different thread
    /// </summary>
    public class FlMultiThreadScriptRunner : FLScriptRunner
    {
        //private GameWindow window;

        public FlMultiThreadScriptRunner(Action onFinishQueueCallback,
            DataTypes dataTypes = DataTypes.Uchar1, string kernelFolder = "assets/kernel/") : base(
            CLAPI.GetInstance(), dataTypes, kernelFolder)
        {
        }

        private void FlFinished(Dictionary<Bitmap, byte[]> result)
        {
            foreach (KeyValuePair<Bitmap, byte[]> keyValuePair in result)
            {
                CLAPI.UpdateBitmap(CLAPI.MainThread, keyValuePair.Key, keyValuePair.Value);
            }
        }

        public override void Enqueue(FlScriptExecutionContext context)
        {
            if (context.OnFinishCallback == null)
            {
                context.OnFinishCallback = FlFinished;
            }
            else
            {
                context.OnFinishCallback += FlFinished;
            }

            base.Enqueue(context);
        }

        public override void Process(Action onFinish = null)
        {
            ThreadManager.RunTask(() => _proc(onFinish), x =>
            {
                foreach (KeyValuePair<FlScriptExecutionContext, Dictionary<Bitmap, byte[]>> textureUpdate in x)
                {
                    foreach (KeyValuePair<Bitmap, byte[]> bytese in textureUpdate.Value)
                    {
                        CLAPI.UpdateBitmap(CLAPI.MainThread, bytese.Key, bytese.Value);
                    }
                }
            });
        }

        private Dictionary<FlScriptExecutionContext, Dictionary<Bitmap, byte[]>> _proc(Action onFinish = null)
        {
            //window = new GameWindow(100, 100, GraphicsMode.Default, "FLRunner");
            //window.MakeCurrent();
            Dictionary<FlScriptExecutionContext, Dictionary<Bitmap, byte[]>> ret =
                new Dictionary<FlScriptExecutionContext, Dictionary<Bitmap, byte[]>>();
            while (ProcessQueue.Count != 0)
            {
                FlScriptExecutionContext fle = ProcessQueue.Dequeue();
                Dictionary<string, byte[]> texUpdate = Process(fle);
                Dictionary<Bitmap, byte[]> texMap = new Dictionary<Bitmap, byte[]>();
                foreach (KeyValuePair<string, byte[]> bytese in texUpdate)
                {
                    if (fle.TextureMap.ContainsKey(bytese.Key))
                    {
                        texMap.Add(fle.TextureMap[bytese.Key], bytese.Value);
                    }
                }

                ret.Add(fle, texMap);
            }

            //window.Dispose();
            onFinish?.Invoke();
            return ret;
        }
    }
}