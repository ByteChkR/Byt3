using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing.Instructions
{
    public class JumpInstruction : Instruction
    {
        public JumpInstruction(List<InstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            if (Arguments.Count != 1 || Arguments[0].Type != InstructionArgumentType.Function)
            {
                throw new InvalidOperationException("Jump instruction needs to point to a valid function.");
            }

            FunctionObject obj = (FunctionObject) Arguments[0].Value;

            Logger.Log(LogType.Log, "Jumping to " + obj.Name, MIN_INSTRUCTION_SEVERITY);

            obj.Process();
        }
    }
}