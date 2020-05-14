using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random
{
    public class SerializableUnifiedRandomFLBuffer : SerializableFLBuffer
    {
        public readonly int Size;
        public SerializableUnifiedRandomFLBuffer(string name, bool isArray, int size) : base(name, isArray)
        {
            Size = size;
        }

        public override FLBuffer GetBuffer()
        {
            return URandomFLInstruction.ComputeUrnd(IsArray, Size);
        }
        public override string ToString()
        {
            return $"--define texture {Name}: urnd";
        }
    }
}