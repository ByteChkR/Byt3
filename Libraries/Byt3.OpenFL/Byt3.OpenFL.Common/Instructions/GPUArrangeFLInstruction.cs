using System.Collections.Generic;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Instructions
{
    public class GPUArrangeFLInstruction : ArrangeFLInstruction
    {
        private readonly CLKernel ArrangeKernel;

        public GPUArrangeFLInstruction(List<FLInstructionArgument> arguments, CLKernel arrangeKernel) : base(arguments)
        {
            ArrangeKernel = arrangeKernel;
        }

        protected override void Arrange(byte[] newOrder)
        {
            MemoryBuffer newOrderBuffer =
                CLAPI.CreateBuffer(Root.Instance, newOrder, MemoryFlag.ReadOnly, "gpuarrange_neworder");

            //Copy Active Buffer
            MemoryBuffer source = CLAPI.Copy<byte>(Root.Instance, Root.ActiveBuffer.Buffer);


            ArrangeKernel.SetBuffer(0, Root.ActiveBuffer.Buffer);
            ArrangeKernel.SetBuffer(1, source);
            ArrangeKernel.SetArg(2, Root.ActiveChannels.Length);
            ArrangeKernel.SetBuffer(3, newOrderBuffer);
            CLAPI.Run(Root.Instance, ArrangeKernel,
                (int) Root.ActiveBuffer.Size /
                Root.ActiveChannels
                    .Length); //Only iterating through the length as if it only has one channel. The cl kernel implementation will deal with that
            newOrderBuffer.Dispose();
            source.Dispose();
        }
    }
}