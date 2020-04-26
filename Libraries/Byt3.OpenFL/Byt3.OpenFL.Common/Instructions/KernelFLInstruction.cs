using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Instructions
{
    public class KernelFLInstruction : FLInstruction
    {
        private readonly CLKernel Kernel;
        private readonly float GenMaxSize;

        public KernelFLInstruction(float genMaxSize, CLKernel kernel, List<FLInstructionArgument> arguments) :
            base(arguments)
        {
            Kernel = kernel;
            GenMaxSize = genMaxSize;
        }

        /// <summary>
        /// FL header Count(the offset from 0 where the "user" parameter start)
        /// </summary>
        public const int FL_HEADER_ARG_COUNT = 5;


        public override string ToString()
        {
            return "Kernel Instruction: " + Kernel.Name;
        }

        private struct ArgumentResult
        {
            public object Value;
            public FLInstructionArgumentType Type;
        }

        private ArgumentResult Compute(FLInstructionArgument arg)
        {
            ArgumentResult ret = new ArgumentResult();
            ret.Type = arg.Type;
            switch (arg.Type)
            {
                case FLInstructionArgumentType.Number:
                    ret.Value = arg.Value;
                    break;
                case FLInstructionArgumentType.Buffer:
                    ret.Value = arg.Value;
                    break;
                case FLInstructionArgumentType.Function:
                    ret.Value = ComputeFunction(arg);
                    break;
                default:
                    throw new InvalidOperationException("Can not parse: " + arg.Value);
            }

            return ret;
        }

        private FLBuffer ComputeFunction(FLInstructionArgument arg)
        {
            FLBuffer buffer =
                Root.RegisterUnmanagedBuffer(new FLBuffer(Root.Instance, Root.Dimensions.x,
                    Root.Dimensions.y, "FunctionInputBuffer_Registered"));

            Logger.Log(LogType.Log, $"Storing Current Execution Context", MIN_INSTRUCTION_SEVERITY + 3);
            Root.PushContext(); //Store Dynamic Variables

            FLFunction flFunction = (FLFunction) arg.Value; //Process the Function Object

            Logger.Log(LogType.Log, $"Executing Function: {flFunction.Name}", MIN_INSTRUCTION_SEVERITY + 2);

            Root.Run(Root.Instance, buffer, true, flFunction);

            Logger.Log(LogType.Log, $"[{Kernel.Name}]Argument Buffer{Root.ActiveBuffer.DefinedBufferName}",
                MIN_INSTRUCTION_SEVERITY + 2);

            FLBuffer ret = Root.ActiveBuffer;
            //Kernel.SetBuffer(kernelArgIndex,
            //    Root.ActiveBuffer.Buffer); //Set the Active Buffer as the Kernel Argument

            Logger.Log(LogType.Log, $"Returning from Function Context", MIN_INSTRUCTION_SEVERITY + 3);
            Root.ReturnFromContext(); //Restore active channels and buffer
            return ret;
        }

        public override void Process()
        {
            Logger.Log(LogType.Log, $"Running CL Kernel: {Kernel.Name}", MIN_INSTRUCTION_SEVERITY);

            ArgumentResult[] results = new ArgumentResult[Arguments.Count];
            Logger.Log(LogType.Log,
                $"[{Kernel.Name}]Computing Kernel Arguments", MIN_INSTRUCTION_SEVERITY);
            for (int i = 0; i < Arguments.Count; i++)
            {
                results[i] = Compute(Arguments[i]);
            }

            for (int i = 0; i < results.Length; i++)
            {
                Logger.Log(LogType.Log,
                    $"[{Kernel.Name}]Setting Kernel Argument {Kernel.Parameter.First(x => x.Value.Id == i)}",
                    MIN_INSTRUCTION_SEVERITY + 1);
                int kernelArgIndex = i + FL_HEADER_ARG_COUNT;

                ArgumentResult arg = results[i];

                switch (arg.Type)
                {
                    case FLInstructionArgumentType.Number:
                        Kernel.SetArg(kernelArgIndex, arg.Value); //The Value is a Decimal
                        break;
                    case FLInstructionArgumentType.Buffer:
                        FLBuffer bi = (FLBuffer) arg.Value;
                        Logger.Log(LogType.Log, $"[{Kernel.Name}]Argument Buffer{bi.DefinedBufferName}",
                            MIN_INSTRUCTION_SEVERITY + 2);
                        Kernel.SetBuffer(kernelArgIndex, bi.Buffer);
                        break;
                    case FLInstructionArgumentType.Function:
                        FLBuffer funcBuffer = (FLBuffer) arg.Value;
                        Logger.Log(LogType.Log, $"[{Kernel.Name}]Argument Buffer{funcBuffer.DefinedBufferName}",
                            MIN_INSTRUCTION_SEVERITY + 2);
                        Kernel.SetBuffer(kernelArgIndex, funcBuffer.Buffer);
                        break;
                    default:
                        throw new InvalidOperationException("Can not parse: " + arg.Value);
                }
            }

            CLAPI.Run(Root.Instance, Kernel, Root.ActiveBuffer.Buffer, Root.Dimensions, GenMaxSize,
                Root.ActiveChannelBuffer, 4);
        }
    }
}