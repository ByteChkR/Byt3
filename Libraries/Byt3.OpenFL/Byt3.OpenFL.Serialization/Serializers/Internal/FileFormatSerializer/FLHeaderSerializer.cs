using System;
using Byt3.OpenFL.Serialization.Exceptions;
using Byt3.OpenFL.Serialization.FileFormat;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.FileFormatSerializer
{
    public class FLHeaderSerializer : ASerializer<FLHeader>
    {
        private VersionSerializer vs = new VersionSerializer();

        public override FLHeader DeserializePacket(PrimitiveValueWrapper s)
        {
            Version headerVersion = vs.DeserializePacket(s);
            if (FLVersions.HeaderVersion != headerVersion)
            {
                throw new FLDeserializationException(
                    $"The Header version can not be parsed. Supported Version: {FLVersions.HeaderVersion} Required Version: {headerVersion}");
            }

            Version commonVersion = vs.DeserializePacket(s);
            Version serializerVersion = vs.DeserializePacket(s);

            int len = s.ReadInt();
            string[] extraInitializationSteps = new string[len];
            for (int i = 0; i < len; i++)
            {
                extraInitializationSteps[i] = s.ReadString();
            }

            return new FLHeader(headerVersion, serializerVersion, commonVersion, extraInitializationSteps);
        }

        public override void SerializePacket(PrimitiveValueWrapper s, FLHeader obj)
        {
            vs.SerializePacket(s, obj.HeaderVersion);
            vs.SerializePacket(s, obj.CommonVersion);
            vs.SerializePacket(s, obj.SerializerVersion);

            s.Write(obj.ExtraSerializationSteps.Length);
            for (int i = 0; i < obj.ExtraSerializationSteps.Length; i++)
            {
                s.Write(obj.ExtraSerializationSteps[i]);
            }
        }
    }
}