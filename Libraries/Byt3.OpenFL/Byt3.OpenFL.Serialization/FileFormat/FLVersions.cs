using System;
using System.Reflection;
using Byt3.OpenFL.Common;

namespace Byt3.OpenFL.Serialization.FileFormat
{
    internal static class FLVersions
    {
        public static Version SerializationVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public static Version CommonVersion => OpenFLDebugConfig.CommonVersion;
        public static Version HeaderVersion => new Version(0, 0, 0, 1);

        public static bool IsCompatible(this FLHeader header)
        {
            return SerializationVersion >= header.SerializerVersion && CommonVersion >= header.CommonVersion;
        }
    }
}