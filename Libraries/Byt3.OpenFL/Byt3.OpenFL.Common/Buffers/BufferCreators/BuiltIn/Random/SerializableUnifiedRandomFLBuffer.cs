using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random
{
    public class SerializableUnifiedRandomFLBuffer : SerializableFLBuffer
    {
        public SerializableUnifiedRandomFLBuffer(string name) : base(name)
        {
        }

        public override FLBuffer GetBuffer()
        {
            return URandomFLInstruction.ComputeUrnd();
        }
        public override string ToString()
        {
            return $"--define texture {Name}: urnd";
        }
    }
}