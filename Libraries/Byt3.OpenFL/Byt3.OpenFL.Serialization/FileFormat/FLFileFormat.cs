using Byt3.OpenFL.Serialization.Serializers.Internal.FileFormatSerializer;

namespace Byt3.OpenFL.Serialization.FileFormat
{
    public class FLFileFormat
    {
        public FLFileFormat(FLHeader compilerHeader, FLProgramHeader programHeader, byte[] program)
        {
            CompilerHeader = compilerHeader;
            Program = program;
            ProgramHeader = programHeader;
        }

        public FLHeader CompilerHeader { get; }
        public FLProgramHeader ProgramHeader { get; }
        public byte[] Program { get; }
    }
}