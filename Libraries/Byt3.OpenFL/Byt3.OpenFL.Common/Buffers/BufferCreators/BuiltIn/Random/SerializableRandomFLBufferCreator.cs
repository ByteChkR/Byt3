using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random
{
    public class SerializableRandomFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args)
        {
            return new SerializableRandomFLBuffer(name);
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "rnd";
        }
    }
}