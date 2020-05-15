using System.Collections.Generic;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Instructions
{
    public class GPUArrangeFLInstruction : ArrangeFLInstruction
    {
        private readonly CLKernel ArrangeKernel;
        private readonly CLKernel CopyKernel;
        public GPUArrangeFLInstruction(List<FLInstructionArgument> arguments, CLKernel arrangeKernel, CLKernel copyKernel) : base(arguments)
        {
            CopyKernel = copyKernel;
            ArrangeKernel = arrangeKernel;
        }

        protected override void Arrange(byte[] newOrder)
        {
            MemoryBuffer mb = CLAPI.CreateBuffer(Root.Instance, newOrder, MemoryFlag.ReadOnly, "gpuarrange_neworder");

            //Copy Active Buffer
            MemoryBuffer dst = CLAPI.CreateEmpty<byte>(Root.Instance, (int) Root.ActiveBuffer.Size, MemoryFlag.ReadWrite, "ActiveBufferCopy");
            CopyKernel.SetBuffer(0, dst);
            CopyKernel.SetBuffer(1, Root.ActiveBuffer.Buffer);
            CLAPI.Run(Root.Instance, CopyKernel, (int)dst.Size);


            
            ArrangeKernel.SetBuffer(0, Root.ActiveBuffer.Buffer);
            ArrangeKernel.SetBuffer(1, dst);
            ArrangeKernel.SetArg(2, Root.ActiveChannels.Length);
            ArrangeKernel.SetBuffer(3, mb);
            CLAPI.Run(Root.Instance, ArrangeKernel, (int)Root.ActiveBuffer.Size / Root.ActiveChannels.Length);
            //CLAPI.Run(Root.Instance, ArrangeKernel, Root.ActiveBuffer.Buffer, Root.Dimensions, GenMaxSize, Root.ActiveChannelBuffer, Root.ActiveChannels.Length);
            mb.Dispose();
            dst.Dispose();
        }
    }
}