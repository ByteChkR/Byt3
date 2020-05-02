using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common
{
    public interface IDebugger
    {


        void Register(FLProgram program);
        void ProcessEvent(FLParsedObject obj);
        void ProgramExit(FLProgram program);



    }
}