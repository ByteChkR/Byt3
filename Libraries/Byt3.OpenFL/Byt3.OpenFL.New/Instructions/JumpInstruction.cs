using System;
using System.Collections.Generic;
using Byt3.OpenFL.New.DataObjects;

namespace Byt3.OpenFL.New.Instructions
{
    public class JumpInstruction : Instruction
    {
        public JumpInstruction(List<InstructionArgument> arguments) : base(arguments) { }


        public override void Process()
        {
            if (Arguments.Count != 1 || Arguments[0].Type != InstructionArgumentType.Function)
            {
                throw new InvalidOperationException("Jump instruction needs to point to a valid function.");
            }

            ((FunctionObject)Arguments[0].Value).Process();

        }
    }
}