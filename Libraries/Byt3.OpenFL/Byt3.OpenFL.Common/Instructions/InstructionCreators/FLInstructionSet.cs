using System;
using System.Collections.Generic;
using System.Reflection;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public class FLInstructionSet
    {

        public static FLInstructionSet CreateWithBuiltInTypes(string clKernelPath)
        {
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);

            FLInstructionSet iset = new FLInstructionSet();
            iset.AddInstructionWithDefaultCreator<JumpFLInstruction>("jmp");
            iset.AddInstructionWithDefaultCreator<SetActiveFLInstruction>("setactive");
            iset.AddInstructionWithDefaultCreator<RandomFLInstruction>("rnd");
            iset.AddInstructionWithDefaultCreator<URandomFLInstruction>("urnd");
            iset.AddInstruction(new KernelFLInstructionCreator(db));
            return iset;
        }

        private readonly List<FLInstructionCreator> creators = new List<FLInstructionCreator>();

        public bool HasInstruction(string key)
        {
            for (int i = 0; i < creators.Count; i++)
            {
                if (creators[i].IsInstruction(key)) return true;
            }

            return false;
        }

        public void AddInstructionWithDefaultCreator<T>(string key) where T : FLInstruction
        {
            AddInstruction(new DefaultInstructionCreator<T>(key));
        }

        public void AddInstruction(FLInstructionCreator creator)
        {
            if (creator == null)
            {
                throw new FLInstructionCreatorIsNullException("Trying to add an Instruction container that is null");
            }
            creators.Add(creator);
        }

        public FLInstruction Create(FLProgram script, SerializableFLInstruction instruction)
        {
            for (int i = 0; i < creators.Count; i++)
            {
                if (creators[i].IsInstruction(instruction.InstructionKey))
                {
                    return creators[i].Create(script, instruction);
                }
            }
            throw new FLInstructionCreatorNotFoundException("Could not find Instruction creator for Key: "+ instruction.InstructionKey);
        }



        public void AddInstructionCreatorsFromAssembly(Assembly asm)
        {
            Type[] ts = asm.GetExportedTypes();
            Type target = typeof(FLInstructionCreator);
            for (int i = 0; i < ts.Length; i++)
            {
                if (target != ts[i] && target.IsAssignableFrom(ts[i]))
                {
                    FLInstructionCreator creator = (FLInstructionCreator)Activator.CreateInstance(ts[i]);
                    AddInstruction(creator);
                }
            }
        }
    }
}