using System;

namespace Byt3.ObjectPipeline
{
    public class PipelineNotValidException : ApplicationException
    {
        public readonly InternalPipelineStage Pipeline;

        public PipelineNotValidException(InternalPipelineStage pipeline, string message) : base(message)
        {
            Pipeline = pipeline;
        }
    }
}