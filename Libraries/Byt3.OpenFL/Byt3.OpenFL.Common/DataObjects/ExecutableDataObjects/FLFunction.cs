using System.Collections.Generic;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class FLFunction : FLParsedObject
    {
        public string Name { get; }
        public List<FLInstruction> Instructions { get; private set; }

        public FLFunction(string name, List<FLInstruction> instructions)
        {
            Name = name;
            Instructions = instructions;
        }

        internal FLFunction(string name)
        {
            Name = name;
        }

        internal void SetInstructions(List<FLInstruction> instructions)
        {
            Instructions = instructions;
        }

        public virtual void Process()
        {
            //Logger.Log(LogType.Log, "Function: "+ Name, 1);
            for (int i = 0; i < Instructions.Count; i++)
            {
                //Thread.Sleep(100);
                //Logger.Log(LogType.Log, "Doing Instruction: " + Instructions[i] + " in function: " + Name, 1);
                Instructions[i].Process();
            }
        }

        public override void SetRoot(FLProgram root)
        {
            if (root == Root) return;
            base.SetRoot(root);
            for (int i = 0; i < Instructions.Count; i++)
            {
                Instructions[i].SetRoot(root);
            }
        }
    }
}