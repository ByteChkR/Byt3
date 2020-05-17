using System.Linq;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.ProgramChecks.Checking.Modifiers
{
    public class ReadWriteAccessValidator : AModifierValidator
    {
        protected string[] InvalidInstructions => new[] { "setactive" };
        protected override InstructionArgumentCategory InvalidArguments => InstructionArgumentCategory.AnyBuffer;

        protected override void Validate(SerializableFLProgram prog, SerializableFLFunction func,
            SerializableFLInstruction inst, SerializableFLInstructionArgument arg)
        {
            SerializableFLBuffer buf = prog.DefinedBuffers.First(x => x.Name == arg.Identifier);
            if (buf.Modifiers.IsReadOnly)
            {
                throw new FLInvalidFLElementModifierUseException(func.Name, FLKeywords.ReadOnlyBufferModifier,
                    $"Can not use instruction {inst.InstructionKey} with a static buffer.");
            }
            if (buf.Modifiers.IsArray)
            {
                throw new FLInvalidFLElementModifierUseException(func.Name, FLKeywords.ArrayKey,
                    $"Can not use instruction {inst.InstructionKey} with an array buffer.");
            }
        }

        protected override bool AppliesOnInstruction(SerializableFLInstruction instr)
        {
            return InvalidInstructions.Contains(instr.InstructionKey);
        }
    }
}