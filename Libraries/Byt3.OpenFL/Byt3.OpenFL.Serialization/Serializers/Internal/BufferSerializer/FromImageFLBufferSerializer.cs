using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile;
using Byt3.OpenFL.Common.ElementModifiers;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class FromImageFLBufferSerializer : FLBaseSerializer
    {
        private readonly bool StoreRaw;

        public FromImageFLBufferSerializer(bool storeBitmapData)
        {
            StoreRaw = storeBitmapData;
        }

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            string name = ResolveId(s.ReadInt());
            bool raw = s.ReadBool();
            FLBufferModifiers bmod = new FLBufferModifiers(name, s.ReadArray<string>());
            if (raw)
            {
                MemoryStream ms = new MemoryStream(s.ReadBytes());

                Bitmap bmp = (Bitmap) Image.FromStream(ms);

                return new SerializableFromBitmapFLBuffer(name, bmp, bmod, bmod.IsArray ? s.ReadInt() : 0);
            }

            string file = s.ReadString();
            return new SerializableFromFileFLBuffer(name, file, bmod, bmod.IsArray ? s.ReadInt() : 0);
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            if (!(obj is SerializableFromFileFLBuffer buffer))
            {
                throw new InvalidOperationException("Invalid type for Serializer.");
            }

            s.Write(ResolveName(buffer.Name));
            s.Write(StoreRaw);
            s.WriteArray(buffer.Modifiers.GetModifiers().ToArray());
            //s.Write(buffer.IsArray);
            if (StoreRaw)
            {
                Bitmap bmp = buffer.Bitmap;

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                s.Write(ms.GetBuffer(), (int) ms.Position);
                bmp.Dispose();
            }
            else
            {
                s.Write(buffer.File);
            }

            if (buffer.IsArray)
            {
                s.Write(buffer.Size);
            }
        }
    }
}