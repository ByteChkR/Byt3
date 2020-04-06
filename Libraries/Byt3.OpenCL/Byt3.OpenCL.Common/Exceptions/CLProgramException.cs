using System;

namespace Byt3.OpenCL.Common.Exceptions
{
    /// <summary>
    /// This Exception occurs when the CL Implementation is not able to compile a CLProgram
    /// </summary>
    public class CLProgramException : Byt3Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">The message why this exception occurred</param>
        /// <param name="inner">Inner exeption</param>
        public CLProgramException(string errorMessage, Exception inner) : base(errorMessage, inner)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">The message why this exception occurred</param>
        public CLProgramException(string errorMessage) : base(errorMessage)
        {
        }
    }
}