using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromFileFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args, bool isArray, int arraySize)
        {
            return new SerializableFromFileFLBuffer(name, args[0].Replace("\"", ""), isArray, arraySize);
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey.StartsWith("\"") && bufferKey.EndsWith("\"");
        }
    }
}