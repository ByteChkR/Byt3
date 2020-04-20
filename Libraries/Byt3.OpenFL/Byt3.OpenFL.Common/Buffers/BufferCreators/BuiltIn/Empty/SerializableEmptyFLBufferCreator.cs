using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty
{
    public class SerializableEmptyFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args)
        {
            return new SerializableEmptyFLBuffer(name);
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "empty";
        }
    }
}