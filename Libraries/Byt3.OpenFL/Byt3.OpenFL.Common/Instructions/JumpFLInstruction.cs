using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Instructions
{
    public class JumpFLInstruction : FLInstruction
    {
        public JumpFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            if (Arguments.Count != 1 || Arguments[0].Type != FLInstructionArgumentType.Function)
            {
                throw new InvalidOperationException("Jump instruction needs to point to a valid function.");
            }

            FLFunction obj = (FLFunction) Arguments[0].Value;

            Logger.Log(LogType.Log, "Jumping to " + obj.Name, MIN_INSTRUCTION_SEVERITY);

            obj.Process();
        }
    }
}