using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class FromImageFLBufferSerializer : ASerializer<SerializableFLBuffer>
    {
        private readonly bool StoreRaw;

        public FromImageFLBufferSerializer(bool storeBitmapData)
        {
            StoreRaw = storeBitmapData;
        }

        public override SerializableFLBuffer DeserializePacket(PrimitiveValueWrapper s)
        {
            string name = s.ReadString();
            bool raw = s.ReadBool();
            if (raw)
            {
                MemoryStream ms = new MemoryStream(s.ReadBytes());

                Bitmap bmp = (Bitmap) Image.FromStream(ms);

                return new SerializableFromBitmapFLBuffer(name, bmp);
            }
            else
            {
                string file = s.ReadString();
                return new SerializableFromFileFLBuffer(name, file);
            }
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableFLBuffer obj)
        {
            if (!(obj is SerializableFromFileFLBuffer buffer))
            {
                throw new InvalidOperationException("Invalid type for Serializer.");
            }

            s.Write(buffer.Name);
            s.Write(StoreRaw);
            if (StoreRaw)
            {
                Bitmap bmp = buffer.GetBitmap();

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                s.Write(ms.GetBuffer(), (int) ms.Position);
                bmp.Dispose();
            }
            else
            {
                s.Write(buffer.File);
            }
        }
    }
}