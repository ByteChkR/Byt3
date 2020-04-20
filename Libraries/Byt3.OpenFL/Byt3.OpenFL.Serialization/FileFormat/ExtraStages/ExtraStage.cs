using Byt3.ObjectPipeline;

namespace Byt3.OpenFL.Serialization.FileFormat.ExtraStages
{
    internal class ExtraStage
    {
        public PipelineStage<byte[], byte[]> ToFile { get; }
        public PipelineStage<byte[], byte[]> FromFile { get; }

        public ExtraStage(PipelineStage<byte[], byte[]> toFile, PipelineStage<byte[], byte[]> fromFile)
        {
            ToFile = toFile;
            FromFile = fromFile;
        }
    }
}