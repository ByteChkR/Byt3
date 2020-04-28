using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class RandomFLBufferSerializer :FLSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            int name = s.ReadInt();
            return new SerializableRandomFLBuffer(ResolveId(name));
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName(((SerializableRandomFLBuffer)obj).Name));
        }
    }
}