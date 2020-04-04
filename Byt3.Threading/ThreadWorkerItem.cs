using System;

namespace Byt3.Threading
{
    internal class ThreadWorkerItem
    {
        public object WorkItem;
        public OnThreadItemFinish OnFinishEvent;

        public ThreadWorkerItem(object workItem, OnThreadItemFinish onFinishEvent = null)
        {
            WorkItem = workItem;
            OnFinishEvent = onFinishEvent;
        }
    }

    internal class ThreadWorkerItem<TIn, TOut> 
    {
        public TIn WorkItem;
        public OnThreadItemFinish<TOut> OnFinishEvent;

        public ThreadWorkerItem(TIn workItem, OnThreadItemFinish<TOut> onFinishEvent = null) 
        {
            WorkItem = workItem;
            OnFinishEvent = onFinishEvent;
        }
    }
}
