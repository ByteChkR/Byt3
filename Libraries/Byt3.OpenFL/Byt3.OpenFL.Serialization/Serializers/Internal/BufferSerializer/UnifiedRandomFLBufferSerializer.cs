using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.OpenFL.Common.ElementModifiers;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class UnifiedRandomFLBufferSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            string name = ResolveId(s.ReadInt());
            string[] mods = s.ReadArray<string>();
            FLBufferModifiers bmod = new FLBufferModifiers(name, mods);

            return new SerializableUnifiedRandomFLBuffer(name, bmod, bmod.IsArray ? s.ReadInt() : 0);
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            SerializableUnifiedRandomFLBuffer input = (SerializableUnifiedRandomFLBuffer) obj;
            s.Write(ResolveName(input.Name));
            s.WriteArray(input.Modifiers.GetModifiers().ToArray());
            if (input.IsArray)
            {
                s.Write(input.Size);
            }
        }
    }
}