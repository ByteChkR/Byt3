using System;
using System.Collections.Generic;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing.Instructions
{
    public class KernelInstruction : Instruction
    {
        private readonly string KernelName;

        public KernelInstruction(string kernelName, List<InstructionArgument> arguments) : base(arguments)
        {
            KernelName = kernelName;
        }

        /// <summary>
        /// FL header Count(the offset from 0 where the "user" parameter start)
        /// </summary>
        private const int FL_HEADER_ARG_COUNT = 5;

        public CLKernel Kernel => Root.KernelDB.GetClKernel(KernelName);

        public override void Process()
        {
            for (int i = 0; i < Arguments.Count; i++)
            {
                int kernelArgIndex = i + FL_HEADER_ARG_COUNT;
                if (Arguments[i].Type == InstructionArgumentType.Buffer)
                {
                    FLBufferInfo bi = (FLBufferInfo)Arguments[i].Value;
                    Kernel.SetBuffer(kernelArgIndex, bi.Buffer);
                }
                else if (Arguments[i].Type == InstructionArgumentType.Function)
                {
                    FLBufferInfo buffer = Root.RegisterUnmanagedBuffer(new FLBufferInfo(Root.Instance, Root.Dimensions.x, Root.Dimensions.y));

                    Root.PushContext(); //Store Dynamic Variables

                    FunctionObject function = (FunctionObject) Arguments[i].Value; //Process the Function Object
                    Root.Run(Root.Instance, Root.KernelDB, buffer, function);
                    Kernel.SetBuffer(kernelArgIndex, Root.ActiveBuffer.Buffer); //Set the Active Buffer as the Kernel Argument

                    Root.ReturnFromContext(); //Restore active channels and buffer
                }
                else if (Arguments[i].Type == InstructionArgumentType.Number)
                {
                    Kernel.SetArg(kernelArgIndex, Arguments[i].Value); //The Value is a Decimal
                }
                else
                {
                    throw new InvalidOperationException("Can not parse: " + Arguments[i].Value);
                }
            }

            CLAPI.Run(Root.Instance, Kernel, Root.ActiveBuffer.Buffer, Root.Dimensions,
                KernelParameter.GetDataMaxSize(Root.KernelDB.GenDataType), CLAPI.CreateBuffer(Root.Instance, Root.ActiveChannels, MemoryFlag.ReadOnly), 4);
        }
    }
}