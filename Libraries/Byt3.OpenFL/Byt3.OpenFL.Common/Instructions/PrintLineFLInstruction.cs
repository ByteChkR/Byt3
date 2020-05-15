using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions
{
    public class PrintLineFLInstruction : FLInstruction
    {
        public PrintLineFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }


        public override void Process()
        {
            Logger.Log(LogType.Log, "FLSCRIPT: " + this, 1);
        }

        public override string ToString()
        {
            return Arguments.Unpack(" ");
        }
    }
}