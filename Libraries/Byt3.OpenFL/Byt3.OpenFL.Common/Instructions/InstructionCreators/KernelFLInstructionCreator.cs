using System.Linq;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public class KernelFLInstructionCreator : FLInstructionCreator
    {
        public KernelDatabase KernelList { get; }
        public KernelFLInstructionCreator(KernelDatabase kernelList)
        {
            KernelList = kernelList;
        }

        public override FLInstruction Create(FLProgram script, SerializableFLInstruction instruction)
        {
            return new KernelFLInstruction(KernelParameter.GetDataMaxSize(KernelList.GenDataType), KernelList.GetClKernel(instruction.InstructionKey), instruction.Arguments.Select(x => new FLInstructionArgument(x.GetValue(script))).ToList());
        }

        public override bool IsInstruction(string key)
        {
            return KernelList.TryGetClKernel(key, out CLKernel _);
        }
    }
}