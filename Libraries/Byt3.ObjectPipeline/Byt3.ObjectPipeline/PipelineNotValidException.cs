using Byt3.Utilities.Exceptions;

namespace Byt3.ObjectPipeline
{
    public class PipelineNotValidException : Byt3Exception
    {
        public PipelineNotValidException(PipelineStage pipeline, string message) : base(message)
        {
            Pipeline = pipeline;
        }

        public PipelineStage Pipeline { get; }
    }
}