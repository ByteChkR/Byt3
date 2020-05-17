using Byt3.ObjectPipeline;

namespace Byt3.OpenFL.Serialization.FileFormat.ExtraStages
{
    internal class ExtraStage
    {
        public ExtraStage(PipelineStage<byte[], byte[]> toFile, PipelineStage<byte[], byte[]> fromFile)
        {
            ToFile = toFile;
            FromFile = fromFile;
        }

        public PipelineStage<byte[], byte[]> ToFile { get; }
        public PipelineStage<byte[], byte[]> FromFile { get; }
    }
}