using System;

namespace Byt3.OpenFL.Serialization.FileFormat
{
    public class FLHeader
    {
        public Version HeaderVersion { get; }
        public Version SerializerVersion { get; }
        public Version CommonVersion { get; }
        public string[] ExtraSerializationSteps { get; }

        public FLHeader(Version headerVersion, Version serializerVersion, Version commonVersion,
            string[] extraSerializationSteps)
        {
            HeaderVersion = headerVersion;
            SerializerVersion = serializerVersion;
            CommonVersion = commonVersion;
            ExtraSerializationSteps = extraSerializationSteps;
        }
    }
}