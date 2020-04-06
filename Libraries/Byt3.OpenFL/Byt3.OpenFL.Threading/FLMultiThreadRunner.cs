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
    public class FlMultiThreadRunner : FlRunner
    {
        //private GameWindow window;

        public FlMultiThreadRunner(Action onFinishQueueCallback,
            DataTypes dataTypes = DataTypes.Uchar1, string kernelFolder = "assets/kernel/") : base(
            Clapi.GetInstance(), dataTypes, kernelFolder)
        {
        }

        private void FlFinished(Dictionary<Bitmap, byte[]> result)
        {
            foreach (KeyValuePair<Bitmap, byte[]> keyValuePair in result)
            {
                Clapi.UpdateBitmap(Clapi.MainThread, keyValuePair.Key, keyValuePair.Value);
            }
        }

        public override void Enqueue(FlExecutionContext context)
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
                foreach (KeyValuePair<FlExecutionContext, Dictionary<Bitmap, byte[]>> textureUpdate in x)
                {
                    foreach (KeyValuePair<Bitmap, byte[]> bytese in textureUpdate.Value)
                    {
                        Clapi.UpdateBitmap(Clapi.MainThread, bytese.Key, bytese.Value);
                    }
                }
            });
        }

        private Dictionary<FlExecutionContext, Dictionary<Bitmap, byte[]>> _proc(Action onFinish = null)
        {
            //window = new GameWindow(100, 100, GraphicsMode.Default, "FLRunner");
            //window.MakeCurrent();
            Dictionary<FlExecutionContext, Dictionary<Bitmap, byte[]>> ret =
                new Dictionary<FlExecutionContext, Dictionary<Bitmap, byte[]>>();
            while (ProcessQueue.Count != 0)
            {
                FlExecutionContext fle = ProcessQueue.Dequeue();
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