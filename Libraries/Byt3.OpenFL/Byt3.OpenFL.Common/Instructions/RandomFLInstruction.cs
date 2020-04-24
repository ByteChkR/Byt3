using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.Exceptions;

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

                FLBuffer func = (FLBuffer)obj.Value;

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
                //MemoryBuffer buf = CLAPI.CreateBuffer(root.Instance,
                //    CLAPI.CreateRandom(root.InputSize, new byte[] {1, 1, 1, 1},
                //        RandomInstructionHelper.Randombytesource, false), MemoryFlag.ReadWrite, "RandomBuffer");
                ////    CLAPI.CreateEmpty<byte>(root.Instance, root.InputSize, MemoryFlag.ReadWrite, "LazyRandomBuffer");
                ////CLAPI.WriteRandom(root.Instance, buf, RandomInstructionHelper.Randombytesource, new byte[] {1, 1, 1, 1},
                ////    false);
                FLBuffer buf = new FLBuffer(root.Instance, CLAPI.CreateRandom(root.InputSize, new byte[] { 1, 1, 1, 1 },
                    RandomInstructionHelper.Randombytesource, false), root.Dimensions.x, root.Dimensions.y, "RandomBuffer");
                buf.SetRoot(root);
                byte[] b = CLAPI.ReadBuffer<byte>(root.Instance, buf.Buffer, (int) buf.Buffer.Size);
                return buf;
            });
            return info;
        }
    }
}