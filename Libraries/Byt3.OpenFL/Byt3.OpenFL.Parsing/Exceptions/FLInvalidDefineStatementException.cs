using System;
using Byt3.Utilities.Exceptions;

namespace Byt3.OpenFL.Parsing.Exceptions
{
    public class FLInvalidDefineStatementException : Byt3Exception
    {
        public FLInvalidDefineStatementException(string errorMessage) : base(errorMessage)
        {
        }

        public FLInvalidDefineStatementException(string errorMessage, Exception inner) : base(errorMessage, inner)
        {
        }
    }
}