using System;
using Byt3.Utilities.Exceptions;

namespace Byt3.ObjectPipeline
{
    public class PipelineNotValidException : Byt3Exception
    {
        public PipelineStage Pipeline { get; }

        public PipelineNotValidException(PipelineStage pipeline, string message) : base(message)
        {
            Pipeline = pipeline;
        }
    }
}