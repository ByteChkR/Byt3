using System;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public abstract class FLInstructionCreator : IDisposable
    {
        public abstract FLInstruction Create(FLProgram script, SerializableFLInstruction instruction);

        public abstract string[] InstructionKeys {get;}

        public virtual string GetArgumentSignatureForInstruction(string instruction)
        {
            return null;
        }

        public virtual string GetDescriptionForInstruction(string instruction)
        {
            return "No Description Provided";
        }

        public abstract bool IsInstruction(string key);

        public virtual void Dispose()
        {

        }
    }
}