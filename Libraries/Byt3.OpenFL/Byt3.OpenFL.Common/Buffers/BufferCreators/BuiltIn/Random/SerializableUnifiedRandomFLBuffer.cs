using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;
using Byt3.OpenFL.Common.Instructions;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random
{
    public class SerializableUnifiedRandomFLBuffer : SerializableFLBuffer
    {
        public readonly int Size;

        public SerializableUnifiedRandomFLBuffer(string name, FLBufferModifiers modifiers, int size) : base(name,
            modifiers)
        {
            Size = size;
        }

        public override FLBuffer GetBuffer()
        {
            return URandomFLInstruction.ComputeUrnd(IsArray, Size, Modifiers.InitializeOnStart);
        }

        public override string ToString()
        {
            return base.ToString() + $"urnd {Size}";
        }
    }
}