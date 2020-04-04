using System;

namespace Byt3.ObjectPipeline
{
    public class DelegatePipelineStage<TIn, TOut> : PipelineStage<TIn, TOut>
    {
        public delegate TOut ProcessDel(TIn input);

        private readonly ProcessDel StageDel;
        public DelegatePipelineStage(ProcessDel stageDel)
        {
            StageDel = stageDel ?? throw new ArgumentNullException("stageDel", "The Stage Delegate can not be null.");
        }

        public override TOut Process(TIn input)
        {
            return StageDel(input);
        }
    }
}