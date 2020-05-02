using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public class InstructionArgumentValidator : FLProgramCheck<SerializableFLProgram>
    {
        public override bool ChangesOutput => false;
        private bool ParseCreatorSig(string sig, out List<InstructionArgumentSignature> ret)
        {
            ret = new List<InstructionArgumentSignature>();
            if (sig == null)
            {
                return false;
            }

            string[] overloads = sig.Split('|');

            foreach (string overload in overloads)
            {
                List<InstructionArgumentCategory> signature = new List<InstructionArgumentCategory>();
                for (int i = 0; i < overload.Length; i++)
                {
                    signature.Add(SerializableFLInstructionArgument.Parse(overload[i]));
                }

                ret.Add(new InstructionArgumentSignature {Signature = signature});
            }

            return true;
        }


        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                {
                    FLInstructionCreator creator = InstructionSet.GetCreator(serializableFlInstruction);

                    bool hasDefinedSig = ParseCreatorSig(
                        creator.GetArgumentSignatureForInstruction(serializableFlInstruction),
                        out List<InstructionArgumentSignature> creatorSigs);
                    if (!hasDefinedSig)
                    {
                        Logger.Log(LogType.Warning,
                            "FL Creator: " + creator.GetType().Name + " has no Argument Signatures defined. Skipping.",
                            1);
                        continue;
                    }

                    InstructionArgumentSignature sig = new InstructionArgumentSignature
                    {
                        Signature = serializableFlInstruction.Arguments
                            .Select(x => x.ArgumentCategory).ToList()
                    };
                    bool matches = false;
                    foreach (InstructionArgumentSignature instructionArgumentSignature in creatorSigs)
                    {
                        matches |= instructionArgumentSignature.Compare(sig.Signature);
                    }

                    if (!matches)
                    {
                        throw new FLProgramCheckException(
                            $"The Script is using the instruction with key: " +
                            serializableFlInstruction.InstructionKey +
                            " with the Arguments: " + sig + " but no such argument overload is found.", this);
                    }
                }
            }

            foreach (SerializableExternalFLFunction serializableFlFunction in input.ExternalFunctions)
            {
                Process(serializableFlFunction.ExternalProgram);
            }


            return input;
        }
    }
}