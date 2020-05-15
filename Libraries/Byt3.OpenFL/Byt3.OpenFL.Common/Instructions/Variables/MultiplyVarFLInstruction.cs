using System;
using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions.Variables
{
    public class MultiplyVarFLInstruction : FLInstruction
    {

        public MultiplyVarFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            decimal v = Parent.Variables.GetVariable(Arguments[0].Value.ToString());
            if (Arguments.Count > 1)
            {
                for (int i = 1; i < Arguments.Count; i++)
                {
                    if (Arguments[i].Type == FLInstructionArgumentType.Number)
                    {
                        v *= (decimal)Arguments[i].Value;
                    }
                    else if (Arguments[i].Type == FLInstructionArgumentType.Name)
                    {
                        v *= Parent.Variables.GetVariable(Arguments[i].ToString());
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("there has to be a value to multiply by.");
            }
            Parent.Variables.ChangeVariable(Arguments[0].Value.ToString(), v);
        }

        public override string ToString()
        {
            return "multiply " + Arguments.Unpack(" ");
        }
    }
}