using System.Collections.Generic;
using Byt3.OpenFL.Common.Instructions.Variables;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class FLFunction : FLParsedObject, IFunction
    {
        public FLFunction(string name, List<FLInstruction> instructions)
        {
            Name = name;
            Instructions = instructions;
        }

        internal FLFunction(string name)
        {
            Name = name;
        }

        public List<FLInstruction> Instructions { get; private set; }
        public VariableManager<decimal> Variables { get; private set; }
        public string Name { get; }

        public virtual void Process()
        {
            FLProgram.Debugger?.ProcessEvent(this);
            for (int i = 0; i < Instructions.Count; i++)
            {
                FLProgram.Debugger?.ProcessEvent(Instructions[i]);
                Instructions[i].Process();
            }
        }

        public override void SetRoot(FLProgram root)
        {
            //if (root == Root)
            //{
            //    return;
            //}

            base.SetRoot(root);

            Variables = root.Variables.AddScope();

            if (Instructions == null)
            {
                return;
            }

            for (int i = 0; i < Instructions.Count; i++)
            {
                Instructions[i].SetRoot(root);
                Instructions[i].SetParent(this);
            }
        }

        internal void SetInstructions(List<FLInstruction> instructions)
        {
            Instructions = instructions;
        }
    }
}