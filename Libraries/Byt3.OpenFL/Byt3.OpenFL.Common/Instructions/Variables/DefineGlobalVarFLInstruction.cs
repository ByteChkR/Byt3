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
            Parent.Variables.AddGlobal(Arguments[0].GetValue().ToString(),
                decimal.Parse(Arguments[1].GetValue().ToString()));
        }

        public override string ToString()
        {
            return "gdef " + Arguments.Unpack(" ");
        }
    }
}