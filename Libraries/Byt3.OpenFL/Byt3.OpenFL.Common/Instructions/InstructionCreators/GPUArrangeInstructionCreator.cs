﻿using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public class GPUArrangeInstructionCreator : FLInstructionCreator
    {
        private readonly CLKernel ArrangeKernel;

        public GPUArrangeInstructionCreator(CLKernel arrangeKernel)
        {
            ArrangeKernel = arrangeKernel;
        }

        public override string[] InstructionKeys => new[] {"gpu_arrange"};

        public override string GetArgumentSignatureForInstruction(string instruction)
        {
            return "V|VV|VVV|VVVV";
        }

        public override FLInstruction Create(FLProgram script, FLFunction func, SerializableFLInstruction instruction)
        {
            List<FLInstructionArgument> args = new List<FLInstructionArgument>();

            for (int i = 0; i < instruction.Arguments.Count; i++)
            {
                FLInstructionArgument arg = new FLInstructionArgument(instruction.Arguments[i].GetValue(script, func));
                args.Add(arg);
            }

            return new GPUArrangeFLInstruction(args, ArrangeKernel);
        }

        public override string GetDescriptionForInstruction(string instruction)
        {
            return "GPU Version of instruction \"arrange\"";
        }

        public override bool IsInstruction(string key)
        {
            return key == "gpu_arrange";
        }
    }
}