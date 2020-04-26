using System;
using System.Threading;

namespace Byt3.Engine.Core
{
    /// <summary>
    /// A reference to a specific task item
    /// </summary>
    /// <typeparam name="T">Type of endresult</typeparam>
    public class TaskReference<T>
    {
        /// <summary>
        /// Delegate used by the TaskReference implementation
        /// </summary>
        /// <returns></returns>
        public delegate T DelTask();

        private T ret;
        private readonly Action<T> onFinish;
        private readonly Thread t;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="task">Task to complete</param>
        /// <param name="onFinish">On Finish Event</param>
        internal TaskReference(DelTask task, Action<T> onFinish)
        {
            this.onFinish = onFinish;
            t = new Thread(() => ThreadRun(task));
        }

        /// <summary>
        /// A flag that is true if the task has been finished
        /// </summary>
        public bool IsDone => !t.IsAlive;


        /// <summary>
        /// Internal Starts the Thread for the Task
        /// </summary>
        internal void RunTask()
        {
            t.Start();
        }

        private void ThreadRun(DelTask task)
        {
            ret = task.Invoke();
        }

        internal bool CheckState()
        {
            if (IsDone)
            {
                onFinish?.Invoke(ret);
            }

            return IsDone;
        }
    }
}