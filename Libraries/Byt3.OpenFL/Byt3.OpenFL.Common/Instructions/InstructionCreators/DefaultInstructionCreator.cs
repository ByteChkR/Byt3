using System;
using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public class DefaultInstructionCreator : FLInstructionCreator
    {
        private readonly string instructionKey;
        private readonly Type type;

        public DefaultInstructionCreator(string key, Type instructionType)
        {
            instructionKey = key;
            type = instructionType;
        }

        public override bool IsInstruction(string key)
        {
            return key == instructionKey;
        }

        public override FLInstruction Create(FLProgram script, SerializableFLInstruction instruction)
        {
            List<FLInstructionArgument> args = new List<FLInstructionArgument>();

            for (int i = 0; i < instruction.Arguments.Count; i++)
            {
                FLInstructionArgument arg = new FLInstructionArgument(instruction.Arguments[i].GetValue(script));
                args.Add(arg);
            }

            return (FLInstruction) Activator.CreateInstance(type, new object[] {args});
        }
    }

    public class DefaultInstructionCreator<T> : DefaultInstructionCreator
        where T : FLInstruction
    {
        public DefaultInstructionCreator(string key) : base(key, typeof(T))
        {
        }
    }
}