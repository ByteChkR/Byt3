using System;

namespace Byt3.Utilities.ProgressFeedback
{
    public interface IProgressIndicator : IDisposable
    {
        IProgressIndicator CreateSubTask(bool asTask = true);
        void SetProgress(string status, int currentProgress, int maxProgress);
    }
}