using System.Collections.Generic;
using System.Linq;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public class KernelFLInstructionCreator : FLInstructionCreator
    {
        public KernelDatabase KernelList { get; }

        public override string[] InstructionKeys => KernelList.KernelNames.ToArray();

        public KernelFLInstructionCreator(KernelDatabase kernelList)
        {
            KernelList = kernelList;
        }

        public override string GetArgumentSignatureForInstruction(string instruction)
        {
            if (!KernelList.TryGetClKernel(instruction, out CLKernel kernel))
            {
                return null;
            }

            char[] arg = new char[kernel.Parameter.Count - KernelFLInstruction.FL_HEADER_ARG_COUNT];
            foreach (KeyValuePair<string, KernelParameter> kernelParameter in kernel.Parameter)
            {
                if (kernelParameter.Value.Id < KernelFLInstruction.FL_HEADER_ARG_COUNT)
                {
                    continue;
                }

                if (kernelParameter.Value.IsArray)
                {
                    if (kernelParameter.Key.StartsWith("array"))
                    {
                        arg[kernelParameter.Value.Id - KernelFLInstruction.FL_HEADER_ARG_COUNT] = 'C';
                    }
                    else
                    {
                        arg[kernelParameter.Value.Id - KernelFLInstruction.FL_HEADER_ARG_COUNT] = 'E';
                    }
                }
                else
                {
                    arg[kernelParameter.Value.Id - KernelFLInstruction.FL_HEADER_ARG_COUNT] = 'V';
                }
            }

            return new string(arg);
        }

        public override FLInstruction Create(FLProgram script, SerializableFLInstruction instruction)
        {
            return new KernelFLInstruction(KernelParameter.GetDataMaxSize(KernelList.GenDataType),
                KernelList.GetClKernel(instruction.InstructionKey),
                instruction.Arguments.Select(x => new FLInstructionArgument(x.GetValue(script))).ToList());
        }

        public override bool IsInstruction(string key)
        {
            return KernelList.TryGetClKernel(key, out CLKernel _);
        }
    }
}