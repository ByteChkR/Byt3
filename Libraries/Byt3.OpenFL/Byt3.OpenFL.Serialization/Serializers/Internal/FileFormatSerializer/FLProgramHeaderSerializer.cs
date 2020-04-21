using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.FileFormatSerializer
{
    public class FLProgramHeaderSerializer : ASerializer<FLProgramHeader>
    {
        private VersionSerializer vs = new VersionSerializer();

        public override FLProgramHeader DeserializePacket(PrimitiveValueWrapper s)
        {
            return new FLProgramHeader(s.ReadString(), s.ReadString(), vs.DeserializePacket(s));
        }

        public override void SerializePacket(PrimitiveValueWrapper s, FLProgramHeader obj)
        {
            s.Write(obj.ProgramName);
            s.Write(obj.Author);
            vs.SerializePacket(s, obj.Version);
        }
    }
}