using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random
{
    public class SerializableRandomFLBuffer : SerializableFLBuffer
    {
        public SerializableRandomFLBuffer(string name) : base(name)
        {
        }

        public override FLBuffer GetBuffer()
        {
            return RandomFLInstruction.ComputeRnd();
        }

        public override string ToString()
        {
            return $"--define texture {Name}: rnd";
        }
    }
}