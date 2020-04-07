using Byt3.Threading;

namespace Byt3.ObjectPipeline.Threading
{
    public class PipelineThreadWorker<TIn, TOut> : ThreadWorker<TIn, TOut>
    {
        private readonly PipelineStage<TIn, TOut> stage;

        public PipelineThreadWorker(PipelineStage<TIn, TOut> stage)
        {
            this.stage = stage;
        }

        protected override TOut DoWork(TIn input)
        {
            return stage.Process(input);
        }
    }
}