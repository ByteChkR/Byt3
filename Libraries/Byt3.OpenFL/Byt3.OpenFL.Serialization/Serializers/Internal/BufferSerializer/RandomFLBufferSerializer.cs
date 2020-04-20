using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class RandomFLBufferSerializer : ASerializer<SerializableRandomFLBuffer>
    {
        public override SerializableRandomFLBuffer DeserializePacket(PrimitiveValueWrapper s)
        {
            string name = s.ReadString();
            return new SerializableRandomFLBuffer(name);
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableRandomFLBuffer obj)
        {
            s.Write(obj.Name);
        }
    }
}