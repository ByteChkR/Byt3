using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing.DataObjects;
using Byt3.OpenFL.Parsing.Exceptions;

namespace Byt3.OpenFL.Parsing.Instructions
{
    public class URandomInstruction : Instruction
    {
        public URandomInstruction(List<InstructionArgument> arguments) : base(arguments)
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
                InstructionArgument obj = Arguments[i];

                if (obj.Type != InstructionArgumentType.Buffer)
                {
                    throw
                        new FLInvalidArgumentType("Argument: " + obj.Value, "MemoyBuffer/Image");
                }

                FLBufferInfo func = (FLBufferInfo) obj.Value;

                Logger.Log(LogType.Log, $"Writing Unified Random Data to Active Buffer:" + func.DefinedBufferName,
                    MIN_INSTRUCTION_SEVERITY);

                CLAPI.WriteRandom(Root.Instance, func.Buffer, RandomInstructionHelper.Randombytesource,
                    Root.ActiveChannels, true);
            }
        }

        public static FLBufferInfo ComputeUrnd(string[] args)
        {
            UnloadedDefinedFLBufferInfo info = new UnloadedDefinedFLBufferInfo(root =>
            {
                MemoryBuffer buf =
                    CLAPI.CreateEmpty<byte>(root.Instance, root.InputSize, MemoryFlag.ReadWrite);
                CLAPI.WriteRandom(root.Instance, buf, RandomInstructionHelper.Randombytesource, new byte[] {1, 1, 1, 1},
                    true);

                return new FLBufferInfo(buf, root.Dimensions.x, root.Dimensions.y);
            });

            return info;
        }
    }
}