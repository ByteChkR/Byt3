
using System;

namespace Byt3.Utilities.ProgressFeedback
{
    public class DummyProgressIndicator : IProgressIndicator
    {
        public IProgressIndicator CreateSubTask(bool asTask = true)
        {
            return this;
        }
        public void SetProgress(string status, int currentProgress, int maxProgress) { }

        public void Dispose() { }
    }
}
