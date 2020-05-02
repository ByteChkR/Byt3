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
        public static FLInstructionSet CreateWithBuiltInTypes(KernelDatabase db)
        {
            FLInstructionSet iset = new FLInstructionSet();
            iset.AddInstructionWithDefaultCreator<JumpFLInstruction>("jmp", "X");
            iset.AddInstructionWithDefaultCreator<SetActiveFLInstruction>("setactive",
                "E|EV|EVV|EVVV|EVVVV|VVVV|VVV|VV|V");
            iset.AddInstructionWithDefaultCreator<RandomFLInstruction>("rnd", "|B");
            iset.AddInstructionWithDefaultCreator<URandomFLInstruction>("urnd", "|B");
            iset.AddInstruction(new KernelFLInstructionCreator(db));
            return iset;
        }

        public static FLInstructionSet CreateWithBuiltInTypes(CLAPI instance, string clKernelPath)
        {
            KernelDatabase db =
                new KernelDatabase(instance, clKernelPath, DataVectorTypes.Uchar1);

            return CreateWithBuiltInTypes(db);
        }

        private readonly List<FLInstructionCreator> creators = new List<FLInstructionCreator>();


        public string[] GetInstructionNames()
        {
            List<string> keys = new List<string>();
            for (int i = 0; i < creators.Count; i++)
            {
                keys.AddRange(creators[i].InstructionKeys);
            }

            return keys.ToArray();
        }

        public bool HasInstruction(string key)
        {
            for (int i = 0; i < creators.Count; i++)
            {
                if (creators[i].IsInstruction(key))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddInstructionWithDefaultCreator<T>(string key, string signature = null) where T : FLInstruction
        {
            AddInstruction(new DefaultInstructionCreator<T>(key, signature));
        }

        public void AddInstruction(FLInstructionCreator creator)
        {
            if (creator == null)
            {
                throw new FLInstructionCreatorIsNullException("Trying to add an Instruction container that is null");
            }

            creators.Add(creator);
        }

        public FLInstructionCreator GetCreator(SerializableFLInstruction instruction)
        {
            for (int i = 0; i < creators.Count; i++)
            {
                if (creators[i].IsInstruction(instruction.InstructionKey))
                {
                    return creators[i];
                }
            }

            throw new FLInstructionCreatorNotFoundException("Could not find Instruction creator for Key: " +
                                                            instruction.InstructionKey);
        }

        public FLInstruction Create(FLProgram script, SerializableFLInstruction instruction)
        {
            return GetCreator(instruction).Create(script, instruction);
        }


        public void AddInstructionCreatorsFromAssembly(Assembly asm)
        {
            Type[] ts = asm.GetExportedTypes();
            Type target = typeof(FLInstructionCreator);
            for (int i = 0; i < ts.Length; i++)
            {
                if (target != ts[i] && target.IsAssignableFrom(ts[i]))
                {
                    FLInstructionCreator creator = (FLInstructionCreator) Activator.CreateInstance(ts[i]);
                    AddInstruction(creator);
                }
            }
        }
    }
}