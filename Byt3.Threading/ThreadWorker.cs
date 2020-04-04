using System.Collections.Concurrent;

namespace Byt3.Threading
{

    public abstract class ThreadWorker : ThreadLoop
    {
        private readonly ConcurrentQueue<ThreadWorkerItem> Queue = new ConcurrentQueue<ThreadWorkerItem>();

        public virtual void EnqueueItem(object workItem, OnThreadItemFinish onFinishEvent = null)
        {
            Queue.Enqueue(new ThreadWorkerItem(workItem, onFinishEvent));
        }

        protected abstract object DoWork(object input);

        protected override void Update()
        {
            while (Queue.TryDequeue(out ThreadWorkerItem result))
            {
                object ret = DoWork(result.WorkItem);
                result.OnFinishEvent?.Invoke(ret);
            }
        }
    }

    public abstract class ThreadWorker<TIn, TOut> : ThreadWorker
    {
        protected abstract TOut DoWork(TIn input);
        protected override object DoWork(object input)
        {
            return DoWork((TIn)input);
        }
    }
}