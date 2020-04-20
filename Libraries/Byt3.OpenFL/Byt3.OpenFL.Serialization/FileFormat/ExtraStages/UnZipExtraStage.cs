using System.IO;
using System.IO.Compression;
using Byt3.ObjectPipeline;

namespace Byt3.OpenFL.Serialization.FileFormat.ExtraStages
{
    internal class UnZipExtraStage : PipelineStage<byte[], byte[]>
    {
        public override byte[] Process(byte[] input)
        {
            return Decompress(input);
            //MemoryStream uncompressed = new MemoryStream();
            //GZipStream zStream = new GZipStream(uncompressed, CompressionMode.Decompress);
            //zStream.Write(input, 0, input.Length);

            ////zStream.CopyTo(uncompressed);


            //byte[] r = new byte[uncompressed.Position];
            //uncompressed.Position = 0;
            //uncompressed.Read(r, 0, r.Length);


            //zStream.Close();
            //uncompressed.Close();
            //return r;
        }

        internal static byte[] Decompress(byte[] bytes)
        {
            byte[] ret;

            using (MemoryStream inStream = new MemoryStream(bytes))
            using (GZipStream bigStream = new GZipStream(inStream, CompressionMode.Decompress))
            using (MemoryStream bigStreamOut = new MemoryStream())
            {
                bigStream.CopyTo(bigStreamOut);
                ret = bigStreamOut.ToArray();
            }

            return ret;
        }
    }
}