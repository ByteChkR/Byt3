using Byt3.Threading;

namespace Byt3.ObjectPipeline.Threading
{
    public class PipelineThreadWorker<TIn, TOut> : ThreadWorker<TIn, TOut>
    {
        private readonly PipelineStage<TIn, TOut> Stage;

        public PipelineThreadWorker(PipelineStage<TIn, TOut> stage)
        {
            Stage = stage;
        }

        protected override TOut DoWork(TIn input)
        {
            return Stage.Process(input);
        }
    }
}