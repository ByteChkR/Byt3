using System.Collections.Generic;
using System.Linq;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common
{
    public static class FLInitializationExtensions
    {
        public static FLProgram Initialize(this SerializableFLProgram program, FLInstructionSet instructionSet)
        {
            Dictionary<string, FLBuffer> buffers = new Dictionary<string, FLBuffer>();
            Dictionary<string, FLFunction> functions = new Dictionary<string, FLFunction>();
            Dictionary<string, ExternalFlFunction> externalFunctions = new Dictionary<string, ExternalFlFunction>();



            for (int i = 0; i < program.ExternalFunctions.Count; i++)
            {
                ExternalFlFunction extFunc = new ExternalFlFunction(program.ExternalFunctions[i].Name,
                    program.ExternalFunctions[i].ExternalProgram, instructionSet);
                externalFunctions.Add(program.ExternalFunctions[i].Name, extFunc);
            }

            for (int i = 0; i < program.DefinedBuffers.Count; i++)
            {
                FLBuffer extFunc = program.DefinedBuffers[i].GetBuffer();
                extFunc.SetKey(program.DefinedBuffers[i].Name);
                buffers.Add(extFunc.DefinedBufferName, extFunc);
            }



            for (int i = 0; i < program.Functions.Count; i++)
            {
                functions.Add(program.Functions[i].Name, null);
            }
            FLProgram p = new FLProgram(externalFunctions, buffers, functions);
            for (int i = 0; i < program.Functions.Count; i++)
            {
                functions[program.Functions[i].Name] = program.Functions[i].Initialize(p, instructionSet);
            }


            p.SetRoot();
            return p;
        }

        private static void SetRoot(this FLProgram program)
        {
            foreach (KeyValuePair<string, FLBuffer> programDefinedBuffer in program.DefinedBuffers)
            {
                programDefinedBuffer.Value.SetRoot(program);
            }

            foreach (KeyValuePair<string, ExternalFlFunction> programDefinedScript in program.DefinedScripts)
            {
                programDefinedScript.Value.SetRoot(program);
            }

            foreach (KeyValuePair<string, FLFunction> programFlFunction in program.FlFunctions)
            {
                programFlFunction.Value.SetRoot(program);
            }
        }

        public static FLFunction Initialize(this SerializableFLFunction function, FLProgram script,
            FLInstructionSet instructionSet)
        {
            FLFunction func = new FLFunction(function.Name,
                function.Instructions.Select(x => x.Initialize(script, instructionSet)).ToList());
            return func;
        }

        public static FLInstruction Initialize(this SerializableFLInstruction instruction, FLProgram script,
            FLInstructionSet instructionSet)
        {
            FLInstruction i = instructionSet.Create(script, instruction);
            return i;
        }
    }
}