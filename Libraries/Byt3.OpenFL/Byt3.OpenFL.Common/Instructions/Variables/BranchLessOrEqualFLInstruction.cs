using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions.Variables
{
    public class BranchLessOrEqualFLInstruction : JumpFLInstruction
    {

        public BranchLessOrEqualFLInstruction(List<FLInstructionArgument> arguments) : base(2, arguments)
        {
        }


        public override void Process()
        {
            FLInstructionArgument left = Arguments[0];
            FLInstructionArgument right = Arguments[1];
            decimal l=0, r=0;
            if (left.Type == FLInstructionArgumentType.Number) l = (decimal) left.Value;
            else if (left.Type == FLInstructionArgumentType.Name && Parent.Variables.IsDefined(left.Value.ToString()))
                l = Parent.Variables.GetVariable(left.Value.ToString());
            if (right.Type == FLInstructionArgumentType.Number) r = (decimal)right.Value;
            else if (right.Type == FLInstructionArgumentType.Name && Parent.Variables.IsDefined(right.Value.ToString()))
                r = Parent.Variables.GetVariable(right.Value.ToString());

            if (l <= r)
            {
                base.Process();
            }

        }

        public override string ToString()
        {
            return "BLE " + Arguments.Unpack(" ");
        }
    }
    public class BranchLessThanFLInstruction : JumpFLInstruction
    {

        public BranchLessThanFLInstruction(List<FLInstructionArgument> arguments) : base(2, arguments)
        {
        }


        public override void Process()
        {
            FLInstructionArgument left = Arguments[0];
            FLInstructionArgument right = Arguments[1];
            decimal l = 0, r = 0;
            if (left.Type == FLInstructionArgumentType.Number) l = (decimal)left.Value;
            else if (left.Type == FLInstructionArgumentType.Name && Parent.Variables.IsDefined(left.Value.ToString()))
                l = Parent.Variables.GetVariable(left.Value.ToString());
            if (right.Type == FLInstructionArgumentType.Number) r = (decimal)right.Value;
            else if (right.Type == FLInstructionArgumentType.Name && Parent.Variables.IsDefined(right.Value.ToString()))
                r = Parent.Variables.GetVariable(right.Value.ToString());

            if (l < r)
            {
                base.Process();
            }

        }

        public override string ToString()
        {
            return "BLT " + Arguments.Unpack(" ");
        }
    }
    public class BranchGreaterOrEqualFLInstruction : JumpFLInstruction
    {

        public BranchGreaterOrEqualFLInstruction(List<FLInstructionArgument> arguments) : base(2, arguments)
        {
        }


        public override void Process()
        {
            FLInstructionArgument left = Arguments[0];
            FLInstructionArgument right = Arguments[1];
            decimal l = 0, r = 0;
            if (left.Type == FLInstructionArgumentType.Number) l = (decimal)left.Value;
            else if (left.Type == FLInstructionArgumentType.Name && Parent.Variables.IsDefined(left.Value.ToString()))
                l = Parent.Variables.GetVariable(left.Value.ToString());
            if (right.Type == FLInstructionArgumentType.Number) r = (decimal)right.Value;
            else if (right.Type == FLInstructionArgumentType.Name && Parent.Variables.IsDefined(right.Value.ToString()))
                r = Parent.Variables.GetVariable(right.Value.ToString());

            if (l < r)
            {
                base.Process();
            }

        }

        public override string ToString()
        {
            return "BGE " + Arguments.Unpack(" ");
        }
    }
}