using System;
using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions.Variables
{
    public class DefineGlobalVarFLInstruction : FLInstruction
    {
        public DefineGlobalVarFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            decimal d;
            if (Arguments[1].Type == FLInstructionArgumentType.Number)
            {
                d = (decimal) Arguments[1].GetValue();
            }
            else if (Arguments[1].Type == FLInstructionArgumentType.Name)
            {
                d = Parent.Variables.GetVariable(Arguments[1].GetValue().ToString());
            }
            else
            {
                throw new InvalidOperationException("Can not get value from Argument: " + Arguments[1]);
            }

            Parent.Variables.ChangeGlobalVariable(Arguments[0].GetValue().ToString(), d);
        }

        public override string ToString()
        {
            return "gdef " + Arguments.Unpack(" ");
        }
    }
}