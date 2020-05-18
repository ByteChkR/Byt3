using System.Collections.Generic;
using System.Linq;
using System.Text;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public class KernelFLInstructionCreator : FLInstructionCreator
    {
        public KernelFLInstructionCreator(KernelDatabase kernelList)
        {
            KernelList = kernelList;
        }

        public KernelDatabase KernelList { get; }

        public override string[] InstructionKeys => KernelList.KernelNames.ToArray();

        public override void Dispose()
        {
            base.Dispose();
            KernelList.Dispose();
        }

        public override string GetDescriptionForInstruction(string instruction)
        {
            if (!KernelList.TryGetClKernel(instruction, out CLKernel kernel))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            bool compatible = !instruction.StartsWith("_");
            if (!compatible)
            {
                sb.AppendLine(
                    "IMPORTANT: THIS CL KERNEL DOES NOT HAVE THE RIGHT FL HEADER SIGNATURE AND IS THEREFORE NOT DIRECTLY USABLE IN A FL SCRIPT!\n");
            }

            sb.AppendLine($"OpenCL Kernel: {kernel.Name} (Automatic Generated Description):");
            sb.AppendLine("Arguments:");
            foreach (KeyValuePair<string, KernelParameter> kernelParameter in kernel.Parameter)
            {
                if (compatible && kernelParameter.Value.Id < KernelFLInstruction.FL_HEADER_ARG_COUNT)
                {
                    continue;
                }

                int idx = compatible
                    ? kernelParameter.Value.Id - KernelFLInstruction.FL_HEADER_ARG_COUNT
                    : kernelParameter.Value.Id;

                sb.Append($"\t Argument: {kernelParameter.Key}[{idx}]:");

                if (kernelParameter.Value.IsArray)
                {
                    if (kernelParameter.Key.StartsWith("array"))
                    {
                        sb.AppendLine("Array Buffer");
                    }
                    else
                    {
                        sb.AppendLine("Buffer");
                    }
                }
                else
                {
                    sb.AppendLine("NumberResolvable");
                }
            }

            return sb.ToString();
        }

        public override string GetArgumentSignatureForInstruction(string instruction)
        {
            if (!KernelList.TryGetClKernel(instruction, out CLKernel kernel))
            {
                return null;
            }

            bool includeHeader = instruction.StartsWith("_");

            char[] arg = new char[includeHeader
                ? kernel.Parameter.Count
                : kernel.Parameter.Count - KernelFLInstruction.FL_HEADER_ARG_COUNT];
            foreach (KeyValuePair<string, KernelParameter> kernelParameter in kernel.Parameter)
            {
                if (!includeHeader && kernelParameter.Value.Id < KernelFLInstruction.FL_HEADER_ARG_COUNT)
                {
                    continue;
                }

                int argIdx = includeHeader
                    ? kernelParameter.Value.Id
                    : kernelParameter.Value.Id - KernelFLInstruction.FL_HEADER_ARG_COUNT;
                if (kernelParameter.Value.IsArray)
                {
                    if (kernelParameter.Key.StartsWith("array"))
                    {
                        arg[argIdx] = 'C';
                    }
                    else
                    {
                        arg[argIdx] = 'E';
                    }
                }
                else
                {
                    arg[argIdx] = 'V';
                }
            }

            return new string(arg);
        }

        public override FLInstruction Create(FLProgram script, FLFunction func, SerializableFLInstruction instruction)
        {
            return new KernelFLInstruction(KernelParameter.GetDataMaxSize(KernelList.GenDataType),
                KernelList.GetClKernel(instruction.InstructionKey),
                instruction.Arguments.Select(x => new FLInstructionArgument(x.GetValue(script, func))).ToList());
        }

        public override bool IsInstruction(string key)
        {
            return !key.StartsWith("_") && InstructionKeys.Contains(key);
            //return KernelList.TryGetClKernel(key, out CLKernel _);
        }
    }
}