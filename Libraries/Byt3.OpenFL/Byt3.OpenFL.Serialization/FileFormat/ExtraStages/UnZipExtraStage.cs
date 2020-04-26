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