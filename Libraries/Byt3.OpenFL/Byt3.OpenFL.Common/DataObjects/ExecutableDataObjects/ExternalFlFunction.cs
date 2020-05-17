using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class ExternalFlFunction : FLParsedObject, IFunction
    {
        private readonly SerializableFLProgram ExternalFunctionBlueprint;
        private readonly FLInstructionSet InstructionSet;
        private readonly FLExecutableElementModifiers Modifiers;

        public ExternalFlFunction(string name, SerializableFLProgram external, FLInstructionSet iset,
            FLExecutableElementModifiers modifiers)
        {
            Modifiers = modifiers;
            ExternalFunctionBlueprint = external;
            InstructionSet = iset;
        }

        public string Name { get; }

        public void Process()
        {
            FLBuffer input = Root.ActiveBuffer;
            FLProgram externalFunction = ExternalFunctionBlueprint.Initialize(Root.Instance, InstructionSet);

            input.SetRoot(externalFunction);

            externalFunction.ActiveChannels = Root.ActiveChannels;
            externalFunction.SetCLVariables(input, false);
            //Not making it internal to the subscript because that would dispose the buffer later in the method
            FLProgram.Debugger?.SubProgramStarted(Root, this, externalFunction);

            externalFunction.EntryPoint.Process();

            FLProgram.Debugger?.SubProgramEnded(Root, externalFunction);

            FLBuffer buf = externalFunction.ActiveBuffer;

            buf.SetRoot(Root);
            input.SetRoot(Root);
            externalFunction.RemoveFromSystem(buf);
            externalFunction.RemoveFromSystem(input);

            Root.ActiveChannels = externalFunction.ActiveChannels;
            Root.ActiveBuffer = buf;


            externalFunction.FreeResources();
        }


        public override string ToString()
        {
            return $"{FLKeywords.DefineScriptKey} {Name}: [UNKNOWN]";
        }
    }
}