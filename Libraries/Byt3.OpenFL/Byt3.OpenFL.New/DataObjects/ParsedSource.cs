using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.New.DataObjects
{
    public class ParsedSource
    {
        public string ScriptName;

        internal ParsedSource(string scriptName, FunctionObject[] functions,
            Dictionary<string, FLBufferInfo> definedBuffers, Dictionary<string, FunctionObject> definedScripts)
        {
            ScriptName = scriptName;
            Functions = functions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }

        public void FreeResources()
        {
            List<string> removeList = new List<string>();
            foreach (KeyValuePair<string, FLBufferInfo> definedBuffer in DefinedBuffers)
            {
                definedBuffer.Value.Dispose();
                if (!(definedBuffer.Value is UnloadedDefinedFLBufferInfo)) removeList.Add(definedBuffer.Key);
            }

            for (int i = 0; i < removeList.Count; i++)
            {
                throw new Exception("Check this");
                DefinedBuffers.Remove(removeList[i]);
            }

            for (int i = 0; i < internalBuffers.Count; i++)
            {
                internalBuffers[i].Dispose();
            }
            internalBuffers.Clear();
        }

        //Parsed Objects
        public FunctionObject[] Functions;
        public FunctionObject EntryPoint => Functions.First(x => x.Name == "Main");
        public Dictionary<string, FLBufferInfo> DefinedBuffers;
        public Dictionary<string, FunctionObject> DefinedScripts;

        //Semi Static Objects. Get Set when the Source is actually beeing ran
        public CLAPI Instance;
        public KernelDatabase KernelDB;
        public FLBufferInfo Input;
        public int3 Dimensions => new int3(Input.Width, Input.Height, 4);
        public int InputSize => Dimensions.x * Dimensions.y * Dimensions.z;


        //Dynamic Variables that change during the execution
        public FLBufferInfo ActiveBuffer;
        public byte[] ActiveChannels;

        private readonly Stack<FLExecutionContext> ContextStack = new Stack<FLExecutionContext>();

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