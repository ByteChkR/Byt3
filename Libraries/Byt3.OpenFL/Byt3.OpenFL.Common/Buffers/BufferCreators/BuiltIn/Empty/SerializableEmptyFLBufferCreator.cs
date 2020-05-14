using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty
{
    public class SerializableEmptyFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args, bool isArray, int arraySize)
        {
            if (isArray)
            {
                return new SerializableEmptyFLBuffer(name, arraySize);
            }
            return new SerializableEmptyFLBuffer(name);
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "empty";
        }
    }
}