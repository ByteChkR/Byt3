using System.Collections.Generic;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.Engine.Rendering;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Threading;

namespace Byt3.Engine.OpenFL
{
    /// <summary>
    /// FLGeneratorComponent that implements a Demo usecase of OpenFL
    /// </summary>
    public class FlGeneratorComponent : AbstractComponent
    {
        /// <summary>
        /// List of previews
        /// </summary>
        private readonly List<LitMeshRendererComponent> previews;

        private FLScriptRunner flRunner;


        /// <summary>
        /// The height of the output texture
        /// </summary>
        private readonly int height = 512;

        private readonly bool multiThread;

        /// <summary>
        /// The width of the output texture
        /// </summary>
        private readonly int width = 512;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="previews">List of previews</param>
        /// <param name="width">Width/height of the output texture</param>
        /// <param name="height"></param>
        /// <param name="multiThread">Flag that determines the way the FL Scripts gets executed</param>
        public FlGeneratorComponent(List<LitMeshRendererComponent> previews, int width, int height,
            bool multiThread = false)
        {
            this.multiThread = multiThread;

            this.width = width;
            this.height = height;
            this.previews = previews;
        }

        /// <summary>
        /// the texture that is beeing used to update the previews
        /// </summary>
        private Texture Tex { get; set; }

        /// <summary>
        /// the texture that is beeing used to update the previews
        /// </summary>
        private Texture SpecularTex { get; set; }

        /// <summary>
        /// Command to run a FL script
        /// </summary>
        /// <param name="args">The filename to the script</param>
        /// <returns>Function Result</returns>
        private string cmd_RunFL(string[] args)
        {
            if (args.Length != 1)
            {
                return "Only One Filepath.";
            }

            RunOnObjImage(args[0]);

            return "Command Finished";
        }


        /// <summary>
        /// Command to reset the Output Texture
        /// </summary>
        /// <param name="args">None</param>
        /// <returns>Function Result</returns>
        private string cmd_FLReset(string[] args)
        {
            Tex.Dispose();
            SpecularTex.Dispose();
            Tex = TextureLoader.ParameterToTexture(width, height, "FLGeneratorMainTexture");
            SpecularTex = TextureLoader.ParameterToTexture(width, height, "FLGeneratorSpecularTexture");
            SpecularTex.TexType = TextureType.Specular;
            for (int i = 0; i < previews.Count; i++)
            {
                previews[i].Textures[0] = Tex;
            }


            return "Texture Reset.";
        }

        /// <summary>
        /// Overridden Awake method for setting up the Interpreter and add the commands to the console
        /// </summary>
        protected override void Awake()
        {
            Tex = TextureLoader.ParameterToTexture(width, height, "FLGeneratorMainTexture");
            SpecularTex = TextureLoader.ParameterToTexture(width, height, "FLGeneratorSpecularTexture");
            SpecularTex.TexType = TextureType.Specular;
            for (int i = 0; i < previews.Count; i++)
            {
                previews[i].Textures[0] = Tex;
                previews[i].Textures[1] = SpecularTex;
            }


            if (multiThread)
            {
                flRunner = new FlMultiThreadScriptRunner(null, DataVectorTypes.Uchar1, "assets/kernel/");
            }
            else
            {
                flRunner = new FLScriptRunner(CLAPI.MainThread, DataVectorTypes.Uchar1, "assets/kernel/");
            }


            DebugConsoleComponent console =
                Owner.Scene.GetChildWithName("Console").GetComponent<DebugConsoleComponent>();
            console?.AddCommand("runfl", cmd_RunFL);
            console?.AddCommand("r", cmd_FLReset);
        }

        /// <summary>
        /// OnDestroy Function. Gets called when the Component or the GameObject got removed from the game
        /// This function is called AFTER the engines update function. So it can happen that before the object is destroyed it can still collide and do other things until its removed at the end of the frame.
        /// </summary>
        protected override void OnDestroy()
        {
            Tex = null;
            SpecularTex = null;
            previews.Clear();
        }


        /// <summary>
        /// Runs a FL Script
        /// </summary>
        /// <param name="filename">Path to the FL Script</param>
        public void RunOnObjImage(string filename)
        {
            Dictionary<string, Texture> otherTex = new Dictionary<string, Texture>
            {
                {"result", Tex}, {"specularOut", SpecularTex}
            };
            MemoryBuffer b = TextureLoader.TextureToMemoryBuffer(CLAPI.MainThread, Tex, "BufferFromFLResult");

            byte[] buf = CLAPI.ReadBuffer<byte>(CLAPI.MainThread, b, (int) b.Size);
            FlScriptExecutionContext exec = new FlScriptExecutionContext(filename, buf, (int) Tex.Width,
                (int) Tex.Height, program => OnFinishCallback(program, otherTex));

            flRunner.Enqueue(exec);
            flRunner.Process();
        }

        private void OnFinishCallback(FLProgram obj, Dictionary<string, Texture> map)
        {
            foreach (KeyValuePair<string, Texture> keyValuePair in map)
            {
                if (!obj.HasBufferWithName(keyValuePair.Key))
                {
                    Logger.Log(DebugChannel.OpenFL | DebugChannel.Warning,
                        "Can not Find Buffer with Name: " + keyValuePair.Key, 1);
                    continue;
                }

                FLBuffer
                    buf = obj.GetBufferWithName(keyValuePair.Key,
                        false); //The buffer will get cleaned up when the program gets disposed.
                keyValuePair.Value.UpdateTexture(obj.Instance, buf.Buffer, buf.Width, buf.Height);
            }
        }
    }
}