﻿using System;

namespace Byt3.OpenFL.Common
{
    public class WorkItemRunnerSettings
    {
        public static WorkItemRunnerSettings Default => new WorkItemRunnerSettings(false, 2);

        public readonly bool UseMultithread;
        public readonly int WorksizeMultiplier;

        public WorkItemRunnerSettings(bool useMultithread, int worksizeMultiplier)
        {
            UseMultithread = useMultithread;
            WorksizeMultiplier = worksizeMultiplier;
        }

        public int GetOptimalWorkSize(int itemCount)
        {
            return 1 + itemCount / (Environment.ProcessorCount * 2);
        }
    }
}