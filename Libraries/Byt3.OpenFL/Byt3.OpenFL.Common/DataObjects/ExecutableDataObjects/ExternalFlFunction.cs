using System.Collections.Generic;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class ExternalFlFunction : FLFunction
    {
        private readonly SerializableFLProgram ExternalFunctionBlueprint;
        private readonly FLInstructionSet InstructionSet;

        public ExternalFlFunction(string name, SerializableFLProgram external, FLInstructionSet iset) : base(name, new List<FLInstruction>())
        {
            ExternalFunctionBlueprint = external;
            InstructionSet = iset;
        }

        public override void Process()
        {
            FLBuffer input = Root.ActiveBuffer;
            FLProgram externalFunction = ExternalFunctionBlueprint.Initialize(InstructionSet);

            input.SetRoot(externalFunction);

            externalFunction.ActiveChannels = Root.ActiveChannels;
            externalFunction.SetCLVariables(Root.Instance, input);

            externalFunction.EntryPoint.Process();
            
            FLBuffer buf = externalFunction.ActiveBuffer;

            buf.SetRoot(Root);
            input.SetRoot(Root);
            externalFunction.RemoveFromSystem(buf);
            externalFunction.RemoveFromSystem(input);

            Root.ActiveChannels = externalFunction.ActiveChannels;
            Root.ActiveBuffer = buf;


            externalFunction.FreeResources();
        }
    }
}