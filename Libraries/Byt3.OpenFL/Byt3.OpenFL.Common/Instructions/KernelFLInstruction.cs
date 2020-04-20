using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Instructions
{
    public class KernelFLInstruction : FLInstruction
    {
        private readonly CLKernel Kernel;
        private readonly float GenMaxSize;

        public KernelFLInstruction(float genMaxSize, CLKernel kernel, List<FLInstructionArgument> arguments) : base(arguments)
        {
            Kernel = kernel;
            GenMaxSize = genMaxSize;
        }

        /// <summary>
        /// FL header Count(the offset from 0 where the "user" parameter start)
        /// </summary>
        private const int FL_HEADER_ARG_COUNT = 5;
        

        public override void Process()
        {
            Logger.Log(LogType.Log, $"Running CL Kernel: {Kernel.Name}", MIN_INSTRUCTION_SEVERITY);
            for (int i = 0; i < Arguments.Count; i++)
            {
                Logger.Log(LogType.Log,
                    $"[{Kernel.Name}]Setting Kernel Argument {Kernel.Parameter.First(x => x.Value.Id == i)}",
                    MIN_INSTRUCTION_SEVERITY + 1);
                int kernelArgIndex = i + FL_HEADER_ARG_COUNT;

                if (Arguments[i].Type == FLInstructionArgumentType.Buffer)
                {
                    FLBuffer bi = (FLBuffer) Arguments[i].Value;
                    Logger.Log(LogType.Log, $"[{Kernel.Name}]Argument Buffer{bi.DefinedBufferName}",
                        MIN_INSTRUCTION_SEVERITY + 2);
                    Kernel.SetBuffer(kernelArgIndex, bi.Buffer);
                }
                else if (Arguments[i].Type == FLInstructionArgumentType.Function)
                {
                    FLBuffer buffer =
                        Root.RegisterUnmanagedBuffer(new FLBuffer(Root.Instance, Root.Dimensions.x,
                            Root.Dimensions.y));

                    Logger.Log(LogType.Log, $"Storing Current Execution Context", MIN_INSTRUCTION_SEVERITY + 3);
                    Root.PushContext(); //Store Dynamic Variables

                    FLFunction flFunction = (FLFunction) Arguments[i].Value; //Process the Function Object

                    Logger.Log(LogType.Log, $"Executing Function: {flFunction.Name}", MIN_INSTRUCTION_SEVERITY + 2);

                    Root.Run(Root.Instance,  buffer, flFunction);

                    Logger.Log(LogType.Log, $"[{Kernel.Name}]Argument Buffer{Root.ActiveBuffer.DefinedBufferName}",
                        MIN_INSTRUCTION_SEVERITY + 2);
                    Kernel.SetBuffer(kernelArgIndex,
                        Root.ActiveBuffer.Buffer); //Set the Active Buffer as the Kernel Argument

                    Logger.Log(LogType.Log, $"Returning from Function Context", MIN_INSTRUCTION_SEVERITY + 3);
                    Root.ReturnFromContext(); //Restore active channels and buffer
                }
                else if (Arguments[i].Type == FLInstructionArgumentType.Number)
                {
                    Logger.Log(LogType.Log, $"[{Kernel.Name}]Argument Value{Arguments[i].Value}",
                        MIN_INSTRUCTION_SEVERITY + 1);
                    Kernel.SetArg(kernelArgIndex, Arguments[i].Value); //The Value is a Decimal
                }
                else
                {
                    throw new InvalidOperationException("Can not parse: " + Arguments[i].Value);
                }
            }

            CLAPI.Run(Root.Instance, Kernel, Root.ActiveBuffer.Buffer, Root.Dimensions, GenMaxSize,
                CLAPI.CreateBuffer(Root.Instance, Root.ActiveChannels, MemoryFlag.ReadOnly), 4);
        }
    }
}