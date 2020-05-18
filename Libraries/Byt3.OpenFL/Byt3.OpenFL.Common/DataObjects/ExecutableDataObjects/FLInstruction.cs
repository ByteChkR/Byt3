using System.Collections.Generic;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public abstract class FLInstruction : FLParsedObject
    {
        protected const int MIN_INSTRUCTION_SEVERITY = 4;
        protected FLFunction Parent;

        protected FLInstruction( List<FLInstructionArgument> arguments)
        {
            Arguments = arguments;
        }

        public List<FLInstructionArgument> Arguments { get; }
        public abstract void Process();

        public virtual void SetParent(FLFunction func)
        {
            Parent = func;
            for (int i = 0; i < Arguments.Count; i++)
            {
                Arguments[i].SetParent(Parent);
            }
        }

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