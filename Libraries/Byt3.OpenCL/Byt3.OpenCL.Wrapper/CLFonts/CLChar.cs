using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Memory;

namespace Byt3.OpenCL.Wrapper.CLFonts
{
    public class CLChar
    {
        public readonly MemoryBuffer Buffer;
        public readonly char Character;
        public readonly int2 Size;

        internal CLChar(char character, MemoryBuffer buf, int width, int height)
        {
            Character = character;
            Buffer = buf;
            Size = new int2(width, height);
        }
    }
}