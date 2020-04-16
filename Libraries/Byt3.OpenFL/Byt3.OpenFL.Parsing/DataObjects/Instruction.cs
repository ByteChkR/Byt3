using System.Collections.Generic;

namespace Byt3.OpenFL.Parsing.DataObjects
{
    public abstract class Instruction : ParsedObject
    {
        protected const int MIN_INSTRUCTION_SEVERITY = 2;
        protected Instruction(List<InstructionArgument> arguments)
        {
            Arguments = arguments;
        }

        public List<InstructionArgument> Arguments;
        public abstract void Process();

        public override void SetRoot(FLParseResult root)
        {
            base.SetRoot(root);
            for (int i = 0; i < Arguments.Count; i++)
            {
                Arguments[i].SetRoot(root);
            }
        }
    }
}