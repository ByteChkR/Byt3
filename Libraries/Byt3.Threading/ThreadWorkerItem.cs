namespace Byt3.Threading
{
    internal class ThreadWorkerItem
    {
        public object WorkItem;
        public OnThreadWorkerItemFinish OnFinishEvent;

        public ThreadWorkerItem(object workItem, OnThreadWorkerItemFinish onFinishEvent = null)
        {
            WorkItem = workItem;
            OnFinishEvent = onFinishEvent;
        }
    }

    internal class ThreadWorkerItem<TIn, TOut> 
    {
        public TIn WorkItem;
        public OnThreadWorkerItemFinish<TOut> OnFinishEvent;

        public ThreadWorkerItem(TIn workItem, OnThreadWorkerItemFinish<TOut> onFinishEvent = null) 
        {
            WorkItem = workItem;
            OnFinishEvent = onFinishEvent;
        }
    }
}
