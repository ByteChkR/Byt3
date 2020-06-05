using System.Collections.Generic;
using Byt3.OpenFL.Common.Arguments;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.ProgramChecks.Optimizations
{
    public class ExternalFLFunctionToCachedBufferOptimization : FLProgramCheck<SerializableFLProgram>
    {
        public override FLProgramCheckType CheckType => FLProgramCheckType.AggressiveOptimization;
        public override int Priority => 4;

        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;

            Dictionary<string, SerializableExternalFLFunction> convertedFuncs =
                new Dictionary<string, SerializableExternalFLFunction>();

            for (int i = input.ExternalFunctions.Count - 1; i >= 0; i--)
            {
                SerializableExternalFLFunction serializableExternalFlFunction = input.ExternalFunctions[i];
                if (serializableExternalFlFunction.Modifiers.ComputeOnce)
                {
                    string newName = "_cached_" + serializableExternalFlFunction.Name;
                    convertedFuncs.Add(newName, serializableExternalFlFunction);
                    input.ExternalFunctions.RemoveAt(i);


                    LazyLoadingFLBuffer buf = new LazyLoadingFLBuffer(root =>
                    {
                        FLProgram prog =
                            serializableExternalFlFunction.ExternalProgram.Initialize(root.Instance, InstructionSet);
                        FLBuffer b = new FLBuffer(root.Instance, root.Dimensions.x, root.Dimensions.y,
                            root.Dimensions.z,
                            "CachedBuffer" + serializableExternalFlFunction.Name);
                        prog.Run(b, true);

                        b = prog.GetActiveBuffer(true);
                        prog.FreeResources();
                        return b;
                    }, serializableExternalFlFunction.Modifiers.InitializeOnStart);

                    input.DefinedBuffers.Add(new SerializableScriptBuffer(newName,
                        new FLBufferModifiers(newName, new[] {FLKeywords.ReadOnlyBufferModifier}), buf));
                }
            }

            foreach (KeyValuePair<string, SerializableExternalFLFunction> staticFunction in convertedFuncs)
            {
                foreach (SerializableFLFunction serializableFlFunction in input.Functions)
                {
                    foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                    {
                        for (int i = 0; i < serializableFlInstruction.Arguments.Count; i++)
                        {
                            SerializableFLInstructionArgument arg = serializableFlInstruction.Arguments[i];
                            if (arg.Identifier == staticFunction.Value.Name)
                            {
                                serializableFlInstruction.Arguments[i] =
                                    new SerializeBufferArgument(staticFunction.Key);
                            }
                        }
                    }
                }
            }


            return input;
        }

        private class SerializableScriptBuffer : SerializableFLBuffer
        {
            private readonly FLBuffer buffer;

            public SerializableScriptBuffer(string name, FLBufferModifiers modifiers,
                FLBuffer buf) : base(name, modifiers)
            {
                buffer = buf;
            }

            public override FLBuffer GetBuffer()
            {
                return buffer;
            }
        }
    }
}