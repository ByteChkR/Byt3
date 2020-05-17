using System.Linq;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.ProgramChecks.Checking.Modifiers
{
    public class NoCallAccessValidator : AModifierValidator
    {
        protected string[] InvalidInstructions => new[] {"jmp"};
        protected override InstructionArgumentCategory InvalidArguments => InstructionArgumentCategory.DefinedFunction;

        protected override void Validate(SerializableFLProgram prog, SerializableFLFunction func,
            SerializableFLInstruction inst, SerializableFLInstructionArgument arg)
        {
            FLExecutableElementModifiers calledFunc =
                prog.Functions.FirstOrDefault(x => x.Name == arg.Identifier)?.Modifiers ??
                prog.ExternalFunctions.First(x => x.Name == arg.Identifier).Modifiers;
            if (calledFunc.NoCall)
            {
                throw new FLInvalidFLElementModifierUseException(func.Name, FLKeywords.NoCallKeyword,
                    $"Can not use instruction {inst.InstructionKey} with a Defined Function that is marked with the nocall modifier.");
            }
        }

        protected override bool AppliesOnInstruction(SerializableFLInstruction instr)
        {
            return !InvalidInstructions.Contains(instr.InstructionKey);
        }
    }
}