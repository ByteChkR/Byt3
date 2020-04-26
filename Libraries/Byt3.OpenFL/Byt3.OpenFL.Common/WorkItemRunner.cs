using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Byt3.ADL;

namespace Byt3.OpenFL.Common
{
    public static class WorkItemRunner
    {
        public delegate void RunWorkItemDel<In>(List<In> input, int start, int count);

        public delegate List<Out> RunWorkItemDel<In, Out>(List<In> input, int start, int count);

        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "WorkItemRunner");

        public static List<Out> RunInWorkItems<In, Out>(List<In> input, RunWorkItemDel<In, Out> action,
            WorkItemRunnerSettings settings)
        {
            List<Task<List<Out>>> taskList = new List<Task<List<Out>>>();
            int workSize = settings.GetOptimalWorkSize(input.Count);
            int currentID = 0;
            int taskNr = 0;
            int maxTasks = input.Count / workSize;
            Logger.Log(LogType.Log, $"Starting {maxTasks} Tasks...", 2);
            while (currentID != input.Count)
            {
                int len = Math.Min(workSize, input.Count - currentID);
                int id = currentID;
                int nr = taskNr;
                Task<List<Out>> task = new Task<List<Out>>(() => action(input, id, len));
                task.ContinueWith(t => Logger.Log(LogType.Log, "Task " + nr + " of " + maxTasks + " completed.", 2));

                taskList.Add(task);
                if (settings.UseMultithread)
                {
                    task.Start();
                }
                else
                {
                    task.RunSynchronously();
                }

                currentID += len;
                taskNr++;
            }

            Logger.Log(LogType.Log, $"Waiting for Tasks..", 2);
            Task.WaitAll(taskList.ToArray());

            List<Out> ret = new List<Out>();
            for (int i = 0; i < taskList.Count; i++)
            {
                ret.AddRange(taskList[i].Result);
            }

            return ret;
        }

        public static void RunInWorkItems<In>(List<In> input, RunWorkItemDel<In> action,
            WorkItemRunnerSettings settings)
        {
            List<Task> taskList = new List<Task>();
            int workSize = settings.GetOptimalWorkSize(input.Count);
            int currentID = 0;
            int taskNr = 0;
            Logger.Log(LogType.Log, $"Starting Tasks...", 2);
            while (currentID != input.Count)
            {
                int len = Math.Min(workSize, input.Count - currentID);
                int id = currentID;
                int nr = taskNr;
                Task task = new Task(() => action(input, id, len));
                task.ContinueWith(t => Logger.Log(LogType.Log, "Task: " + nr + " completed.", 2));

                taskList.Add(task);
                if (settings.UseMultithread)
                {
                    task.Start();
                }
                else
                {
                    task.RunSynchronously();
                }

                currentID += len;
                taskNr++;
            }

            Logger.Log(LogType.Log, $"Waiting for Tasks..", 2);
            Task.WaitAll(taskList.ToArray());
        }
    }
}