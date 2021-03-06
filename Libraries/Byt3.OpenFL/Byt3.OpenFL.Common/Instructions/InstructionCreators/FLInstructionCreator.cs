﻿using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public abstract class FLInstructionCreator
    {
        public abstract FLInstruction Create(FLProgram script, SerializableFLInstruction instruction);

        public abstract string[] InstructionKeys {get;}

        public virtual string GetArgumentSignatureForInstruction(SerializableFLInstruction instruction)
        {
            return null;
        }

        public abstract bool IsInstruction(string key);
    }
}