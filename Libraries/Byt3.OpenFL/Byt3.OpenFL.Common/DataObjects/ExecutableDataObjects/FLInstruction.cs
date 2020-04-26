using System.Collections.Generic;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public abstract class FLInstruction : FLParsedObject
    {
        protected const int MIN_INSTRUCTION_SEVERITY = 4;

        protected FLInstruction(List<FLInstructionArgument> arguments)
        {
            Arguments = arguments;
        }

        public List<FLInstructionArgument> Arguments { get; }
        public abstract void Process();

        public override void SetRoot(FLProgram root)
        {
            base.SetRoot(root);
            for (int i = 0; i < Arguments.Count; i++)
            {
                Arguments[i].SetRoot(root);
            }
        }
    }
}