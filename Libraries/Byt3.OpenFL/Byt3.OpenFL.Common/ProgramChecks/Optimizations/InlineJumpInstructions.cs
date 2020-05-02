using System.Linq;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.ProgramChecks.Optimizations
{
    public class InlineJumpInstructionsOptimization : FLProgramCheck<SerializableFLProgram>
    {
        public override bool ChangesOutput => true;

        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram)o;


            foreach (SerializableFLFunction f in input.Functions)
            {
                for (int i = f.Instructions.Count - 1; i >= 0; i--)
                {
                    SerializableFLInstruction t = f.Instructions[i];
                    if (t.InstructionKey == "jmp" &&
                        t.Arguments[0].ArgumentCategory == InstructionArgumentCategory.Function)
                    {
                        string fname = t.Arguments[0].Identifier;
                        SerializableFLFunction func = input.Functions.First(x => x.Name == fname);
                        f.Instructions.RemoveAt(i);
                        f.Instructions.AddRange(func.Instructions);
                    }
                }
            }


            return input;
        }
    }
}