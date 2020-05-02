using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class UnifiedRandomFLBufferSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            int name = s.ReadInt();
            return new SerializableUnifiedRandomFLBuffer(ResolveId(name));
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName(((SerializableUnifiedRandomFLBuffer)obj).Name));
        }
    }
}