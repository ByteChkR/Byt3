using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class UnifiedRandomFLBufferSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            int name = s.ReadInt();
            bool isArray = s.ReadBool();
            return new SerializableUnifiedRandomFLBuffer(ResolveId(name), isArray, isArray ? s.ReadInt() : 0);
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName(((SerializableUnifiedRandomFLBuffer)obj).Name));
            s.Write(((SerializableUnifiedRandomFLBuffer)obj).IsArray);
            if (((SerializableUnifiedRandomFLBuffer)obj).IsArray)
            {
                s.Write(((SerializableUnifiedRandomFLBuffer)obj).Size);
            }
        }
    }
}