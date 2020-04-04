using System;

namespace Byt3.ObjectPipeline
{
    public class PipelineNotValidException : ApplicationException
    {
        public readonly PipelineStage Pipeline;

        public PipelineNotValidException(PipelineStage pipeline, string message) : base(message)
        {
            Pipeline = pipeline;
        }
    }
}