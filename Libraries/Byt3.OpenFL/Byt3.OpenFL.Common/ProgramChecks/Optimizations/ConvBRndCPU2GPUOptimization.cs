using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn;

namespace Byt3.OpenFL.Common.ProgramChecks.Optimizations
{
    public class ConvBRndCPU2GPUOptimization : FLProgramCheck<SerializableFLProgram>
    {
        public override int Priority => 2;
        public override FLProgramCheckType CheckType => FLProgramCheckType.Optimization;

        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            List<SerializableRandomFLBuffer> rndBuffers = new List<SerializableRandomFLBuffer>();
            List<SerializableUnifiedRandomFLBuffer> urndBuffers = new List<SerializableUnifiedRandomFLBuffer>();

            for (int i = 0; i < input.DefinedBuffers.Count; i++)
            {
                if (input.DefinedBuffers[i].IsArray)
                {
                    continue; //No support for arrays due to crashes that have not yet been fixed.
                }

                SerializableFLBuffer serializableFlBuffer = input.DefinedBuffers[i];
                if (serializableFlBuffer is SerializableRandomFLBuffer r)
                {
                    rndBuffers.Add(r);
                    if (r.IsArray)
                    {
                        input.DefinedBuffers[i] = new SerializableEmptyFLBuffer(r.Name, r.Size, r.Modifiers);
                    }
                    else
                    {
                        input.DefinedBuffers[i] = new SerializableEmptyFLBuffer(r.Name, r.Modifiers);
                    }
                }
                else if (serializableFlBuffer is SerializableUnifiedRandomFLBuffer u)
                {
                    urndBuffers.Add(u);
                    if (u.IsArray)
                    {
                        input.DefinedBuffers[i] = new SerializableEmptyFLBuffer(u.Name, u.Size, u.Modifiers);
                    }
                    else
                    {
                        input.DefinedBuffers[i] = new SerializableEmptyFLBuffer(u.Name, u.Modifiers);
                    }
                }
            }

            List<SerializableFLInstruction> weavedBufferInitializationCode = new List<SerializableFLInstruction>();
            for (int i = 0; i < rndBuffers.Count; i++)
            {
                weavedBufferInitializationCode.Add(new SerializableFLInstruction("setactive",
                    new List<SerializableFLInstructionArgument>
                    {
                        new SerializeBufferArgument(rndBuffers[i].Name),
                        new SerializeDecimalArgument(0),
                        new SerializeDecimalArgument(1),
                        new SerializeDecimalArgument(2),
                        new SerializeDecimalArgument(3)
                    }));
                weavedBufferInitializationCode.Add(new SerializableFLInstruction("rnd_gpu",
                    new List<SerializableFLInstructionArgument>()));
            }

            for (int i = 0; i < urndBuffers.Count; i++)
            {
                weavedBufferInitializationCode.Add(new SerializableFLInstruction("setactive",
                    new List<SerializableFLInstructionArgument>
                    {
                        new SerializeBufferArgument(urndBuffers[i].Name),
                        new SerializeDecimalArgument(0),
                        new SerializeDecimalArgument(1),
                        new SerializeDecimalArgument(2),
                        new SerializeDecimalArgument(3)
                    }));
                weavedBufferInitializationCode.Add(new SerializableFLInstruction("urnd_gpu",
                    new List<SerializableFLInstructionArgument>()));
            }

            weavedBufferInitializationCode.Add(new SerializableFLInstruction("setactive",
                new List<SerializableFLInstructionArgument>
                {
                    new SerializeBufferArgument(FLKeywords.InputBufferKey),
                    new SerializeDecimalArgument(0),
                    new SerializeDecimalArgument(1),
                    new SerializeDecimalArgument(2),
                    new SerializeDecimalArgument(3)
                }));


            if (urndBuffers.Count != 0 || rndBuffers.Count != 0)
            {
                string s = "Weaved Assembly:\n";
                weavedBufferInitializationCode.ForEach(x => s += "\t" + x + "\n");
                Logger.Log(LogType.Log, s, 2);
                input.Functions.First(x => x.Name == FLKeywords.EntryFunctionKey).Instructions
                    .InsertRange(0, weavedBufferInitializationCode);
            }

            return input;
        }
    }
}