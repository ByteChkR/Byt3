using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class UnifiedRandomFLBufferSerializer : ASerializer<SerializableUnifiedRandomFLBuffer>
    {
        public override SerializableUnifiedRandomFLBuffer DeserializePacket(PrimitiveValueWrapper s)
        {
            string name = s.ReadString();
            return new SerializableUnifiedRandomFLBuffer(name);
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableUnifiedRandomFLBuffer obj)
        {
            s.Write(obj.Name);
        }
    }
}