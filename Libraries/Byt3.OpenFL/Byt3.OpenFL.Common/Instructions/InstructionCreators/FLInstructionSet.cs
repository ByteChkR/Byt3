using System;
using System.Collections.Generic;
using System.Reflection;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.OpenFL.Common.Instructions.Variables;

namespace Byt3.OpenFL.Common.Instructions.InstructionCreators
{
    public class FLInstructionSet : IDisposable
    {
        private readonly List<FLInstructionCreator> creators = new List<FLInstructionCreator>();
        public int CreatorCount => creators.Count;

        public void Dispose()
        {
            for (int i = 0; i < creators.Count; i++)
            {
                creators[i].Dispose();
            }

            creators.Clear();
        }

        public static FLInstructionSet CreateWithBuiltInTypes(KernelDatabase db)
        {
            FLInstructionSet iset = new FLInstructionSet();
            iset.AddInstructionWithDefaultCreator<JumpFLInstruction>("jmp", "X",
                "Jumps to a Script or Function and returns. The active channels and buffer will not be cleared");
            iset.AddInstructionWithDefaultCreator<SetActiveFLInstruction>("setactive",
                "E|EV|EVV|EVVV|EVVVV|VVVV|VVV|VV|V", "Sets the active buffer and active channel states.");
            iset.AddInstructionWithDefaultCreator<RandomFLInstruction>("rnd", "|B",
                "Writes random values to all active channels of the active buffer");
            iset.AddInstructionWithDefaultCreator<URandomFLInstruction>("urnd", "|B",
                "Writes random values to all active channels of the active buffer, the channels of a pixel will have the same color(grayscale)");
            iset.AddInstructionWithDefaultCreator<DefineVarFLInstruction>("def", "DV",
                "Defines a variable in the local scope and assigns a value to it");
            iset.AddInstructionWithDefaultCreator<DefineGlobalVarFLInstruction>("gdef", "DV",
                "Defines a variable in global scope and assigns a value to it");
            iset.AddInstructionWithDefaultCreator<DecrementVarFLInstruction>("dec", "D|DV|DD",
                "Decrements a variable by 1 if no arguments specified.");
            iset.AddInstructionWithDefaultCreator<IncrementVarFLInstruction>("inc", "D|DV|DD",
                "Increments a variable by 1 if no arguments specified.");
            iset.AddInstructionWithDefaultCreator<MultiplyVarFLInstruction>("multiply", "DV|DD",
                "Multiplies a variable by the arguments Specified.");
            iset.AddInstructionWithDefaultCreator<DivideVarFLInstruction>("divide", "DV|DD",
                "Divides a variable by the arguments Specified.");
            iset.AddInstructionWithDefaultCreator<BranchLessOrEqualFLInstruction>("ble", "DVX|VVX|DDX",
                "Branches to the Specified function or script when firstparameter <= secondparameter");
            iset.AddInstructionWithDefaultCreator<BranchGreaterOrEqualFLInstruction>("bge", "DVX|VVX|DDX",
                "Branches to the Specified function or script when firstparameter >= secondparameter");
            iset.AddInstructionWithDefaultCreator<BranchLessThanFLInstruction>("blt", "DVX|VVX|DDX",
                "Branches to the Specified function or script when firstparameter < secondparameter");
            iset.AddInstructionWithDefaultCreator<BranchGreaterThanFLInstruction>("bgt", "DVX|VVX|DDX",
                "Branches to the Specified function or script when firstparameter > secondparameter");
            iset.AddInstructionWithDefaultCreator<PrintLineFLInstruction>("print",
                "A|AA|AAA|AAAA|AAAAA|AAAAAA|AAAAAAA|AAAAAAAA|AAAAAAAAA|AAAAAAAAAA|AAAAAAAAAAA|AAAAAAAAAAAA",
                "Prints text or all kinds of variables to the console.");
            iset.AddInstructionWithDefaultCreator<CPUArrangeFLInstruction>("arrange", "V|VV|VVV|VVVV",
                "Swaps the channels based on the arguments provided");
            iset.AddInstructionWithDefaultCreator<ArraySetFLInstruction>("arrset", "CVV", "sets the specified value at the specified index.");

            iset.AddInstruction(new KernelFLInstructionCreator(db));

            if (db.TryGetClKernel("_arrange", out CLKernel arrangeKernel))
            {
                iset.AddInstruction(new GPUArrangeInstructionCreator(arrangeKernel));
            }

            return iset;
        }

        public static FLInstructionSet CreateWithBuiltInTypes(CLAPI instance, string clKernelPath)
        {
            KernelDatabase db =
                new KernelDatabase(instance, clKernelPath, DataVectorTypes.Uchar1);

            return CreateWithBuiltInTypes(db);
        }

        public FLInstructionCreator GetCreatorAt(int idx)
        {
            if (idx >= 0 && idx < creators.Count)
            {
                return creators[idx];
            }

            throw new IndexOutOfRangeException();
        }

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

        public void AddInstructionWithDefaultCreator<T>(string key, string signature = null, string description = null,
            bool allowStaticUse = true) where T : FLInstruction
        {
            AddInstruction(new DefaultInstructionCreator<T>(key, signature, description, allowStaticUse));
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

        public FLInstruction Create(FLProgram script, FLFunction func, SerializableFLInstruction instruction)
        {
            return GetCreator(instruction).Create(script, func, instruction);
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