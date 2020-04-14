using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.New.DataObjects
{
    public class ParsedSource
    {
        internal ParsedSource(FunctionObject[] functions, Dictionary<string, FLBufferInfo> definedBuffers)
        {
            Functions = functions;
            DefinedBuffers = definedBuffers;
        }

        //Parsed Objects
        public FunctionObject[] Functions;
        public FunctionObject EntryPoint => Functions.First(x => x.Name == "Main");
        public Dictionary<string, FLBufferInfo> DefinedBuffers;

        //Semi Static Objects. Get Set when the Source is actually beeing ran
        public CLAPI Instance;
        public KernelDatabase KernelDB;
        public FLBufferInfo Input;
        public int3 Dimensions => new int3(Input.Width, Input.Height, 4);


        //Dynamic Variables that change during the execution
        public FLBufferInfo ActiveBuffer;
        public byte[] ActiveChannels;

        private Stack<FLExecutionContext> ContextStack = new Stack<FLExecutionContext>();

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

        public void Run(CLAPI instance, KernelDatabase kernelDB, FLBufferInfo input)
        {
            //Setting Run Dependent Variables.
            Instance = instance;
            KernelDB = kernelDB;
            Input = input;

            //Start Setup
            ActiveBuffer = input;
            ActiveChannels = new byte[]{1,1,1,1};

            EntryPoint.Process();
        }

        
    }
}