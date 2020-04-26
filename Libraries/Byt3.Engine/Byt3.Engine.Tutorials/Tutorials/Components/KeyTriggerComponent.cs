using System.Collections.Generic;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace Byt3.Engine.Tutorials.Tutorials.Components
{
    public class KeyTriggerComponent : AbstractComponent
    {
        //Our 2 Textures
        private Texture _tex;
        private Texture _tex2;

        private bool red = true;

        FLScriptRunner flRunner = new FlMultiThreadScriptRunner(null, DataVectorTypes.Uchar1, "assets/kernel/");
        public KeyTriggerComponent(Texture tex, Texture tex2)
        {
            _tex = tex;
            _tex2 = tex2;
        }

        protected override void OnDestroy()
        {
            flRunner.Dispose();
            base.OnDestroy();
        }

        protected override void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Space) // Space enqueues a FLExecutionContext for each texture
            {
                //Creating a Texture Map that maps the buffers of the Interpreter to the Textures
                //"result" is always the result buffer of the interpreter
                //but if we wanted, we can define a buffer in the FL Script and map this buffer to any texture by its name.
                //Thats for example a cheap way to do specular textures alongside the actual textures.
                Dictionary<string, Texture> texMap = new Dictionary<string, Texture>
                {
                    //In This example we could also use "in" as a key,
                    //but this can be wrong at times when the fl execution context starts with a different input texture
                    //than output texture
                    {"in", _tex }
                };
                Dictionary<string, Texture> texMap2 = new Dictionary<string, Texture>
                {
                    {"in", _tex2 }
                };

                //We change the color every enqueue, to be able to see the change
                string path = red ? "assets/filter/red.fl" : "assets/filter/blue.fl";
                red = !red;


                byte[] texBuf = TextureLoader.TextureToByteArray(_tex);
                byte[] tex2Buf = TextureLoader.TextureToByteArray(_tex2);
                //Creating the Execution Context
                
                FlScriptExecutionContext fle = new FlScriptExecutionContext(path, texBuf, (int)_tex.Width, (int)_tex.Height, program => OnFinishCallback(program, texMap));
                FlScriptExecutionContext fle2 = new FlScriptExecutionContext(path, tex2Buf, (int)_tex2.Width, (int)_tex2.Height, program => OnFinishCallback(program, texMap2));
                
                //Enqueuing the Contexts
                flRunner.Enqueue(fle);
                flRunner.Enqueue(fle2);


                Logger.Log(DebugChannel.Log | DebugChannel.GameOpenFL, "Enqueued 2 Items. Items In Queue: " + flRunner.ItemsInQueue, 1);
            }

            if (e.Key == Key.Enter && flRunner.ItemsInQueue != 0) //When we press enter we will process our queue.
            {
                flRunner.Process();
            }
            base.OnKeyDown(sender, e);
        }

        private void OnFinishCallback(FLProgram obj, Dictionary<string, Texture> map)
        {
            GameWindow window = new GameWindow(100, 100, GraphicsMode.Default, "FLRunner");
            window.MakeCurrent();

            foreach (KeyValuePair<string, Texture> keyValuePair in map)
            {
                if (!obj.HasBufferWithName(keyValuePair.Key))
                {
                    Logger.Log(DebugChannel.OpenFL | DebugChannel.Warning, "Can not Find Buffer with Name: " + keyValuePair.Key, 1);
                    continue;
                }

                FLBuffer buf = obj.GetBufferWithName(keyValuePair.Key, false); //program will clean the buffer
                keyValuePair.Value.UpdateTexture(obj.Instance, buf.Buffer, buf.Width, buf.Height);
            }

            obj.FreeResources();
            window.Dispose();
        }
    }
}