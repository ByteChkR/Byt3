using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.WFC;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class WFCFLBufferSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            string name = ResolveId(s.ReadInt());
            bool force = s.ReadBool();
            int n = s.ReadInt();
            int width = s.ReadInt();
            int height = s.ReadInt();
            int symmetry = s.ReadInt();
            int ground = s.ReadInt();
            int limit = s.ReadInt();
            bool pIn = s.ReadBool();
            bool pOut = s.ReadBool();


            MemoryStream ms = new MemoryStream(s.ReadBytes());

            Bitmap bmp = (Bitmap) Image.FromStream(ms);

            WFCParameterObject obj = new WFCParameterObject(new SerializableFromBitmapFLBuffer("wfc-bin", bmp), n,
                width, height, symmetry, ground, limit, pIn, pOut, force);
            return new SerializableWaveFunctionCollapseFLBuffer(name, obj);
        }


        public override void Serialize(PrimitiveValueWrapper s, object input)
        {
            SerializableWaveFunctionCollapseFLBuffer obj = (SerializableWaveFunctionCollapseFLBuffer) input;
            s.Write(ResolveName(obj.Name));
            s.Write(obj.Parameter.Force);
            s.Write(obj.Parameter.N);
            s.Write(obj.Parameter.Width);
            s.Write(obj.Parameter.Height);
            s.Write(obj.Parameter.Symmetry);
            s.Write(obj.Parameter.Ground);
            s.Write(obj.Parameter.Limit);
            s.Write(obj.Parameter.PeriodicInput);
            s.Write(obj.Parameter.PeriodicOutput);

            MemoryStream ms = new MemoryStream();

            Bitmap bmp = obj.Parameter.SourceImage.Bitmap;

            bmp.Save(ms, ImageFormat.Png);

            s.Write(ms.GetBuffer(), (int) ms.Position);

            bmp.Dispose();
        }
    }
}