using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class EmptyFLBufferSerializer : ASerializer<SerializableEmptyFLBuffer>
    {
        public override SerializableEmptyFLBuffer DeserializePacket(PrimitiveValueWrapper s)
        {
            string name = s.ReadString();
            return new SerializableEmptyFLBuffer(name);
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableEmptyFLBuffer obj)
        {
            s.Write(obj.Name);
        }
    }
}