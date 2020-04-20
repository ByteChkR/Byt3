using System;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.FileFormatSerializer
{
    public class FLProgramHeader
    {
        public string ProgramName { get; }
        public string Author { get; }
        public Version Version { get; }

        public FLProgramHeader(string programName, string author, Version version)
        {
            ProgramName = programName;
            Author = author;
            Version = version;
        }
    }
}