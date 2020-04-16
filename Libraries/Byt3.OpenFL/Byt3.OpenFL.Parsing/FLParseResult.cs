using System.Collections.Generic;
using System.Linq;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing
{
    public class FLParseResult
    {
        private readonly List<FLBufferInfo> internalBuffers = new List<FLBufferInfo>();

        public CLAPI Instance { get; private set; }
        public KernelDatabase KernelDB { get; private set; }
        public FLBufferInfo ActiveBuffer { get; set; }
        public byte[] ActiveChannels { get; set; }

        public FLBufferInfo Input { get; private set; }
        public int3 Dimensions => new int3(Input.Width, Input.Height, 4);
        public int InputSize => Dimensions.x * Dimensions.y * Dimensions.z;

        public string Filename { get; }
        public string[] Source { get; }
        public Dictionary<string, FLBufferInfo> DefinedBuffers { get; }
        public FunctionObject[] Functions { get; }
        public Dictionary<string, FunctionObject> DefinedScripts { get; }


        private readonly Stack<FLExecutionContext> ContextStack = new Stack<FLExecutionContext>();

        public FunctionObject EntryPoint => Functions.First(x => x.Name == "Main");


        public FLParseResult(string filename, string[] source, Dictionary<string, FunctionObject> definedScripts,
            Dictionary<string, FLBufferInfo> definedBuffers, FunctionObject[] functions)
        {
            Filename = filename;
            Source = source;
            Functions = functions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }

        public void FreeResources()
        {
            foreach (KeyValuePair<string, FLBufferInfo> definedBuffer in DefinedBuffers)
            {
                definedBuffer.Value.Dispose();
            }

            foreach (FLBufferInfo internalBuffer in internalBuffers)
            {
                internalBuffer.Dispose();
            }

            internalBuffers.Clear();
        }

        public void PushContext()
        {
            ContextStack.Push(new FLExecutionContext(new List<byte>(ActiveChannels).ToArray(), ActiveBuffer));
            ActiveChannels = new byte[] {1, 1, 1, 1};
        }

        public void ReturnFromContext()
        {
            FLExecutionContext context = ContextStack.Pop();
            ActiveChannels = context.ActiveChannels;
            ActiveBuffer = context.ActiveBuffer;
        }

        internal FLBufferInfo RegisterUnmanagedBuffer(FLBufferInfo buffer)
        {
            internalBuffers.Add(buffer);

            return buffer;
        }


        public void SetCLVariables(CLAPI instance, KernelDatabase kernelDB, FLBufferInfo input)
        {
            //Setting Run Dependent Variables.
            Instance = instance;
            KernelDB = kernelDB;
            Input = ActiveBuffer = DefinedBuffers["in"] = input;
            Input.SetKey("in");
        }

        public void Run(CLAPI instance, KernelDatabase kernelDB, FLBufferInfo input, FunctionObject entry = null)
        {
            SetCLVariables(instance, kernelDB, input);

            //Start Setup
            ActiveBuffer = input;
            ActiveChannels = new byte[] {1, 1, 1, 1};

            FunctionObject entryPoint = entry ?? EntryPoint;
            entryPoint.Process();
        }
    }
}