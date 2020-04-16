﻿using System.Collections.Generic;

namespace Byt3.OpenFL.Parsing.DataObjects
{
    public class FunctionObject : ParsedObject
    {
        public string Name { get; }
        public List<Instruction> Instructions { get; }

        public FunctionObject(string name, List<Instruction> instructions)
        {
            Name = name;
            Instructions = instructions;
        }

        public virtual void Process()
        {
            for (int i = 0; i < Instructions.Count; i++)
            {
                Instructions[i].Process();
            }
        }

        public override void SetRoot(FLParseResult root)
        {
            base.SetRoot(root);
            for (int i = 0; i < Instructions.Count; i++)
            {
                Instructions[i].SetRoot(root);
            }
        }
    }
}