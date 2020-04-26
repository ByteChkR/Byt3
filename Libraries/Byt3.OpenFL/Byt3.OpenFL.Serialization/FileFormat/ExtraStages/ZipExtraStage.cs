using System.IO;
using System.IO.Compression;
using Byt3.ObjectPipeline;

namespace Byt3.OpenFL.Serialization.FileFormat.ExtraStages
{
    internal class ZipExtraStage : PipelineStage<byte[], byte[]>
    {
        public override byte[] Process(byte[] input)
        {
            return Compress(input);
        }

        internal static byte[] Compress(byte[] bytes)
        {
            byte[] ret;
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipStream tinyStream = new GZipStream(outStream, CompressionMode.Compress))
                using (MemoryStream mStream = new MemoryStream(bytes))
                {
                    mStream.CopyTo(tinyStream);
                }

                ret = outStream.ToArray();
            }

            return ret;
        }
    }
}