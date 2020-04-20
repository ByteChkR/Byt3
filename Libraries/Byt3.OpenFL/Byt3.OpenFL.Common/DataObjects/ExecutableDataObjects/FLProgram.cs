using System.Collections.Generic;
using System.Linq;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class FLProgram
    {
        private readonly List<FLBuffer> internalBuffers = new List<FLBuffer>();

        //Get set when calling Run/SetCLVariables
        public CLAPI Instance { get; private set; }
        public FLBuffer ActiveBuffer { get; set; }
        public byte[] ActiveChannels { get; set; }
        public FLBuffer Input { get; private set; }
        public int3 Dimensions => new int3(Input.Width, Input.Height, 4);
        public int InputSize => Dimensions.x * Dimensions.y * Dimensions.z;

        

        public Dictionary<string, FLBuffer> DefinedBuffers { get; }
        public FLFunction[] FlFunctions { get; }
        public Dictionary<string, ExternalFlFunction> DefinedScripts { get; }


        private readonly Stack<FLExecutionContext> ContextStack = new Stack<FLExecutionContext>();

        public FLFunction EntryPoint => FlFunctions.First(x => x.Name == "Main");


        public FLProgram(Dictionary<string, ExternalFlFunction> definedScripts,
            Dictionary<string, FLBuffer> definedBuffers, FLFunction[] flFunctions)
        {
            FlFunctions = flFunctions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }

        public void FreeResources()
        {
            foreach (KeyValuePair<string, FLBuffer> definedBuffer in DefinedBuffers)
            {
                definedBuffer.Value.Dispose();
            }

            foreach (FLBuffer internalBuffer in internalBuffers)
            {
                internalBuffer.Dispose();
            }

            internalBuffers.Clear();
        }

        public void PushContext()
        {
            ContextStack.Push(new FLExecutionContext(new List<byte>(ActiveChannels).ToArray(), ActiveBuffer));
            ActiveChannels = new byte[] { 1, 1, 1, 1 };
        }

        public void ReturnFromContext()
        {
            FLExecutionContext context = ContextStack.Pop();
            ActiveChannels = context.ActiveChannels;
            ActiveBuffer = context.ActiveBuffer;
        }

        public FLBuffer RegisterUnmanagedBuffer(FLBuffer buffer)
        {
            internalBuffers.Add(buffer);

            return buffer;
        }


        public void SetCLVariables(CLAPI instance, FLBuffer input)
        {
            //Setting Run Dependent Variables.
            Instance = instance;
            Input = ActiveBuffer = DefinedBuffers["in"] = input;
            Input.SetKey("in");
        }

        public void Run(CLAPI instance,  FLBuffer input, FLFunction entry = null)
        {
            SetCLVariables(instance, input);

            //Start Setup
            ActiveBuffer = input;
            ActiveChannels = new byte[] { 1, 1, 1, 1 };

            FLFunction entryPoint = entry ?? EntryPoint;
            entryPoint.Process();


            CLAPI.Flush(instance);
        }
    }
}