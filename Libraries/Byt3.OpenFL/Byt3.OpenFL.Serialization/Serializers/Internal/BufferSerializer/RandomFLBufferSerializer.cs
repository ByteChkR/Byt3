using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class RandomFLBufferSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            int name = s.ReadInt();
            bool isArray = s.ReadBool();
            return new SerializableRandomFLBuffer(ResolveId(name), isArray, isArray ? s.ReadInt() : 0);
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName(((SerializableRandomFLBuffer)obj).Name));
            s.Write(((SerializableRandomFLBuffer)obj).IsArray);
            if (((SerializableRandomFLBuffer) obj).IsArray)
            {
                s.Write(((SerializableRandomFLBuffer)obj).Size);
            }
        }
    }
}