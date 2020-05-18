using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions.Variables
{
    public class DecrementVarFLInstruction : FLInstruction
    {
        public DecrementVarFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            decimal v = Parent.Variables.GetVariable(Arguments[0].GetValue().ToString());
            if (Arguments.Count > 1)
            {
                for (int i = 1; i < Arguments.Count; i++)
                {
                    if (Arguments[i].Type == FLInstructionArgumentType.Number)
                    {
                        v -= (decimal) Arguments[i].GetValue();
                    }
                    else if (Arguments[i].Type == FLInstructionArgumentType.Name)
                    {
                        object o = Arguments[i].GetValue();
                        v -= Parent.Variables.GetVariable(o.ToString());
                    }
                }
            }
            else
            {
                v--;
            }

            Parent.Variables.ChangeVariable(Arguments[0].GetValue().ToString(), v);
        }

        public override string ToString()
        {
            return "dec " + Arguments.Unpack(" ");
        }
    }
}