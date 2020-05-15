using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random
{
    public class SerializableRandomFLBuffer : SerializableFLBuffer
    {
        public readonly int Size;
        public SerializableRandomFLBuffer(string name, bool isArray, int size) : base(name, isArray)
        {
            Size = size;
        }

        public override FLBuffer GetBuffer()
        {
            return RandomFLInstruction.ComputeRnd(IsArray, Size);
        }

        public override string ToString()
        {
            return $"{(IsArray? FLKeywords.DefineArrayKey:FLKeywords.DefineTextureKey)} {Name}: rnd {Size}";
        }
    }
}