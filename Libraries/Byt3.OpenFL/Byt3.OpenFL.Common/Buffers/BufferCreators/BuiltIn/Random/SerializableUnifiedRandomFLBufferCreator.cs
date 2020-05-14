using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random
{
    public class SerializableUnifiedRandomFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args, bool isArray, int arraySize)
        {
            return new SerializableUnifiedRandomFLBuffer(name,isArray, arraySize);
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "urnd";
        }
    }
}