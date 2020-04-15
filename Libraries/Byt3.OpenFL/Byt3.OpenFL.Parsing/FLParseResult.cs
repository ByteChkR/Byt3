using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing
{
    public class FLParseResult
    {
        public CLAPI Instance;
        public KernelDatabase KernelDB;
        public FLBufferInfo ActiveBuffer;
        public byte[] ActiveChannels;

        public FLBufferInfo Input;
        public int3 Dimensions => new int3(Input.Width, Input.Height, 4);
        public int InputSize => Dimensions.x * Dimensions.y * Dimensions.z;

        public readonly string Filename;
        public readonly string[] Source;
        public readonly Dictionary<string, FLBufferInfo> DefinedBuffers;
        public readonly FunctionObject[] Functions;
        public readonly Dictionary<string, FunctionObject> DefinedScripts;


        private readonly Stack<FLExecutionContext> ContextStack = new Stack<FLExecutionContext>();

        public FunctionObject EntryPoint => Functions.First(x => x.Name == "Main");


        public FLParseResult(string filename, string[] source, Dictionary<string, FunctionObject> definedScripts, Dictionary<string, FLBufferInfo> definedBuffers, FunctionObject[] functions)
        {
            Filename = filename;
            Source = source;
            Functions = functions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }

        public void PushContext()
        {
            ContextStack.Push(new FLExecutionContext(new List<byte>(ActiveChannels).ToArray(), ActiveBuffer));
        }

        public void ReturnFromContext()
        {
            FLExecutionContext context = ContextStack.Pop();
            ActiveChannels = context.ActiveChannels;
            ActiveBuffer = context.ActiveBuffer;
        }

        private List<FLBufferInfo> internalBuffers = new List<FLBufferInfo>();
        internal FLBufferInfo RegisterUnmanagedBuffer(FLBufferInfo buffer)
        {
            internalBuffers.Add(buffer);

            return buffer;
        }

        public void Run(CLAPI instance, KernelDatabase kernelDB, FLBufferInfo input, FunctionObject entry = null)
        {
            //Setting Run Dependent Variables.
            Instance = instance;
            KernelDB = kernelDB;
            Input = input;

            //Start Setup
            ActiveBuffer = input;
            ActiveChannels = new byte[] { 1, 1, 1, 1 };

            FunctionObject entryPoint = entry ?? EntryPoint;
            entryPoint.Process();
        }
    }
}
