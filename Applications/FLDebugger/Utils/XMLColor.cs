using System.Drawing;

namespace FLDebugger.Utils
{
    public struct XMLColor
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public XMLColor(byte a, byte r, byte g, byte b)
        {
            R = r;
            A = a;
            G = g;
            B = b;
        }

        public static implicit operator Color(XMLColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static implicit operator XMLColor(Color color)
        {
            return new XMLColor(color.A, color.R, color.G, color.B);
        }
    }
}