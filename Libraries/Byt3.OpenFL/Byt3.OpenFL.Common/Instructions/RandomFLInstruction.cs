﻿using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions
{
    public class RandomFLInstruction : FLInstruction
    {
        public RandomFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            if (Arguments.Count == 0)
            {
                Logger.Log(LogType.Log, $"Writing Random Data to Active Buffer:" + Root.ActiveBuffer.DefinedBufferName,
                    MIN_INSTRUCTION_SEVERITY);
                CLAPI.WriteRandom(Root.Instance, Root.ActiveBuffer.Buffer, RandomInstructionHelper.Randombytesource,
                    Root.ActiveChannels, false);
            }

            for (int i = 0; i < Arguments.Count; i++)
            {
                FLInstructionArgument obj = Arguments[i];

                if (obj.Type != FLInstructionArgumentType.Buffer)
                {
                    throw
                        new FLInvalidArgumentType("Argument: " + obj.Value, "MemoyBuffer/Image");
                }

                FLBuffer func = (FLBuffer) obj.Value;

                Logger.Log(LogType.Log, $"Writing Random Data to Active Buffer:" + func.DefinedBufferName,
                    MIN_INSTRUCTION_SEVERITY);

                CLAPI.WriteRandom(Root.Instance, func.Buffer, RandomInstructionHelper.Randombytesource,
                    Root.ActiveChannels, false);
            }
        }


        public static FLBuffer ComputeRnd()
        {
            LazyLoadingFLBuffer info = new LazyLoadingFLBuffer(root =>
            {
                FLBuffer buf = new FLBuffer(root.Instance, CLAPI.CreateRandom(root.InputSize, new byte[] {1, 1, 1, 1},
                        RandomInstructionHelper.Randombytesource, false), root.Dimensions.x, root.Dimensions.y,
                    "RandomBuffer");
                buf.SetRoot(root);
                return buf;
            });
            return info;
        }

        public override string ToString()
        {
            return "rnd " + Arguments.Unpack(" ");
        }
    }
}