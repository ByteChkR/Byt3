using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common
{
    public interface IDebugger
    {


        void Register(FLProgram program);
        void ProgramStart(FLProgram program);
        void ProcessEvent(FLParsedObject obj);
        void ProgramExit(FLProgram program);
        void SubProgramStarted(FLProgram program, ExternalFlFunction subProgram, FLProgram script);
        void SubProgramEnded(FLProgram program, FLProgram subProgram);
        void OnAddInternalBuffer(FLProgram program, FLBuffer buffer);

    }
}