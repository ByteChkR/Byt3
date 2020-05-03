using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions.Variables
{
    public class DefineVarFLInstruction : FLInstruction
    {

        public DefineVarFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            Parent.Variables.AddVariable(Arguments[0].Value.ToString(), decimal.Parse(Arguments[1].Value.ToString()));
        }

        public override string ToString()
        {
            return "def " + Arguments.Unpack(" ");
        }
    }
}