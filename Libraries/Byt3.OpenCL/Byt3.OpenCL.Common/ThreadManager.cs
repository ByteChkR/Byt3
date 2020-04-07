using System;
using System.Collections.Generic;
using System.Linq;

namespace Byt3.OpenCL.Common
{
    internal class ThreadManager<T> : IThreadManager
    {
        /// <summary>
        /// A List of all Running Tasks
        /// </summary>
        public List<TaskReference<T>> RunningTasks = new List<TaskReference<T>>();
        /// <summary>
        /// The Type of the TheadManager
        /// to comply with the IThreadManager Interface
        /// </summary>
        public Type Type => typeof(T);


        /// <summary>
        /// Checks the States of the currently running tasks
        /// </summary>
        /// <returns>True if all tasks finished</returns>
        public bool CheckStates()
        {
            for (int i = RunningTasks.Count - 1; i >= 0; i--)
            {
                if (RunningTasks[i].CheckState())
                {
                    RunningTasks.RemoveAt(i);
                }
            }

            return RunningTasks.Count == 0;
        }

        /// <summary>
        /// Enqueues the Task and runs it
        /// </summary>
        /// <param name="task"></param>
        public void RunTask(TaskReference<T> task)
        {
            RunningTasks.Add(task);
            task.RunTask();
        }
    }

    /// <summary>
    /// Static Implementation of the Thread Manager
    /// </summary>
    public static class ThreadManager
    {
        private static readonly List<IThreadManager> Managers = new List<IThreadManager>();

        /// <summary>
        /// Runs a Task on a different thread
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="onFinish"></param>
        public static void RunTask<T>(TaskReference<T>.DelTask task, Action<T> onFinish)
        {
            ThreadManager<T> manager = GetManager<T>();
            manager.RunTask(CreateTask(task, onFinish));
        }
        /// <summary>
        /// Runs a Task on a different thread
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        public static void RunTask<T>(TaskReference<T> task)
        {
            ThreadManager<T> manager = GetManager<T>();
            manager.RunTask(task);
        }

        /// <summary>
        /// Checks all ThreadManager Completion States and removes them when they have finished
        /// </summary>
        internal static void CheckManagerStates()
        {
            for (int i = Managers.Count - 1; i >= 0; i--)
            {
                if (Managers[i].CheckStates())
                {
                    Managers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Returns a ThreadManager of Type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static ThreadManager<T> GetManager<T>()
        {
            List<IThreadManager> mgrs = Managers.Where(x => x.Type == typeof(T)).ToList();
            ThreadManager<T> manager;
            if (mgrs.Count == 0)
            {
                manager = new ThreadManager<T>();
                Managers.Add(manager);
            }
            else
            {
                manager = mgrs[0] as ThreadManager<T>;
            }

            return manager;
        }

        /// <summary>
        /// Removes a manager from the manager list
        /// </summary>
        /// <param name="manager"></param>
        internal static void RemoveManager(IThreadManager manager)
        {
            Managers.Remove(manager);
        }

        /// <summary>
        /// Creates a Task of a specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="onFinish"></param>
        /// <returns></returns>
        public static TaskReference<T> CreateTask<T>(TaskReference<T>.DelTask task, Action<T> onFinish)
        {
            return new TaskReference<T>(task, onFinish);
        }
    }
}