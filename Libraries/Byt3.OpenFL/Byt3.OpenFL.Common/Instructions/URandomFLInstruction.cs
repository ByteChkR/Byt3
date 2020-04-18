using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.Instructions
{
    public class URandomFLInstruction : FLInstruction
    {
        public URandomFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            if (Arguments.Count == 0)
            {
                Logger.Log(LogType.Log,
                    $"Writing Unified Random Data to Active Buffer:" + Root.ActiveBuffer.DefinedBufferName,
                    MIN_INSTRUCTION_SEVERITY);
                CLAPI.WriteRandom(Root.Instance, Root.ActiveBuffer.Buffer, RandomInstructionHelper.Randombytesource,
                    Root.ActiveChannels, true);
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

                Logger.Log(LogType.Log, $"Writing Unified Random Data to Active Buffer:" + func.DefinedBufferName,
                    MIN_INSTRUCTION_SEVERITY);

                CLAPI.WriteRandom(Root.Instance, func.Buffer, RandomInstructionHelper.Randombytesource,
                    Root.ActiveChannels, true);
            }
        }

        public static FLBuffer ComputeUrnd(string[] args)
        {
            LazyLoadingFLBuffer info = new LazyLoadingFLBuffer(root =>
            {
                MemoryBuffer buf =
                    CLAPI.CreateEmpty<byte>(root.Instance, root.InputSize, MemoryFlag.ReadWrite);
                CLAPI.WriteRandom(root.Instance, buf, RandomInstructionHelper.Randombytesource, new byte[] {1, 1, 1, 1},
                    true);

                return new FLBuffer(buf, root.Dimensions.x, root.Dimensions.y);
            });

            return info;
        }
    }
}