using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace FLDebugger
{
    public interface IProgramDebugger
    {
        bool FollowScripts { get; }
        void ProgramStart();
        void ProcessEvent(FLParsedObject obj);
        void ProgramExit();
        void SubProgramStarted();
        void SubProgramEnded();

        void OnAddInternalBuffer(FLBuffer buffer);
    }
}