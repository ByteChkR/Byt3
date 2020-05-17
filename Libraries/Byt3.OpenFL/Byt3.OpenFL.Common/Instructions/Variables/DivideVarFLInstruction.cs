using System;
using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions.Variables
{
    public class DivideVarFLInstruction : FLInstruction
    {
        public DivideVarFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            decimal v = Parent.Variables.GetVariable(Arguments[0].GetValue().ToString());
            if (Arguments.Count > 1)
            {
                for (int i = 1; i < Arguments.Count; i++)
                {
                    decimal div;
                    if (Arguments[i].Type == FLInstructionArgumentType.Number)
                    {
                        div = (decimal) Arguments[i].GetValue();
                    }
                    else if (Arguments[i].Type == FLInstructionArgumentType.Name)
                    {
                        div = Parent.Variables.GetVariable(Arguments[i].ToString());
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Can not Resolve type {Arguments[i].Type} to a decimal value");
                    }

                    if (div == 0)
                    {
                        throw new DivideByZeroException("Divide by 0 on instruction: " + ToString() +
                                                        "\nArgument Index: " + i);
                    }

                    v /= div;
                }
            }
            else
            {
                throw new InvalidOperationException("there has to be a value to multiply by.");
            }

            Parent.Variables.ChangeVariable(Arguments[0].GetValue().ToString(), v);
        }

        public override string ToString()
        {
            return "divide " + Arguments.Unpack(" ");
        }
    }
}