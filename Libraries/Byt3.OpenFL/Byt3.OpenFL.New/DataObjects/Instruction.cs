using System.Collections.Generic;

namespace Byt3.OpenFL.New.DataObjects
{
    public abstract class Instruction : ParsedObject
    {
        protected Instruction( List<InstructionArgument> arguments)
        {
            Arguments = arguments;
        }

        public List<InstructionArgument> Arguments;
        public abstract void Process();

        public override void SetRoot(ParsedSource root)
        {
            base.SetRoot(root);
            for (int i = 0; i < Arguments.Count; i++)
            {
                Arguments[i].SetRoot(root);
            }
        }
    }
}