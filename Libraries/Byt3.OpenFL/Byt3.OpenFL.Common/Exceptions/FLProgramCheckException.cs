using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.Utilities.Exceptions;

namespace Byt3.OpenFL.Common.Exceptions
{
    public class FLProgramCheckException : Byt3Exception
    {
        public FLProgramCheckException(string errorMessage,
            FLProgramCheck stage) : base(stage.GetType().Name + ": " + errorMessage)
        {
        }
    }
}