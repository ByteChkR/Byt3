using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableDecimalArgumentSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeDecimalArgument((decimal) s.ReadFloat());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write((float) (obj as SerializeDecimalArgument).Value);
        }
    }
}