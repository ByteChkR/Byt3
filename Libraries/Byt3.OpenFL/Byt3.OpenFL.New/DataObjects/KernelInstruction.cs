using System;
using System.Collections.Generic;
using System.Threading;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.New.DataObjects
{
    public class KernelInstruction : Instruction
    {
        private string KernelName;

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
            for (int i = FL_HEADER_ARG_COUNT; i < Arguments.Count; i++)
            {
                if (Arguments[i].Type == InstructionArgumentType.Buffer)
                {
                    Kernel.SetBuffer(i, ((FLBufferInfo)Arguments[i].Value).Buffer);
                }
                else if (Arguments[i].Type == InstructionArgumentType.Function)
                {
                    FLBufferInfo buffer = new FLBufferInfo(Root.Instance, Root.Dimensions.x, Root.Dimensions.y);

                    Root.PushContext(); //Store Dynamic Variables

                    Root.ActiveChannels = new byte[] { 1, 1, 1, 1 }; //Reset the Active Channels

                    Root.ActiveBuffer = buffer; //Set the Active buffer as new buffer

                    ((FunctionObject)Arguments[i].Value).Process(); //Process the Function Object

                    Kernel.SetBuffer(i, Root.ActiveBuffer.Buffer); //Set the Active Buffer as the Kernel Argument

                    Root.ReturnFromContext(); //Restore active channels and buffer
                }
                else if(Arguments[i].Type == InstructionArgumentType.Number)
                {
                    Kernel.SetArg(i, Arguments[i].Value); //The Value is a Decimal
                }
                else
                {
                    throw new InvalidOperationException("Can not parse: "+ Arguments[i].Value);
                }
            }

            CLAPI.Run(Root.Instance, Kernel, Root.ActiveBuffer.Buffer, Root.Dimensions,
                KernelParameter.GetDataMaxSize(Root.KernelDB.GenDataType), CLAPI.CreateBuffer(Root.Instance, Root.ActiveChannels, MemoryFlag.CopyHostPointer), 4);
        }
    }
}