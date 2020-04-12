using System;
using Byt3.ADL;

namespace Byt3.OpenCL.Common.Exceptions
{
    /// <summary>
    /// Exception that the SuperClass of all exceptions that get thrown with logger.crash
    /// </summary>
    public class Byt3Exception : ApplicationException
    {
        protected readonly ADLLogger<DebugChannel> Logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">The message why this exception occurred</param>
        /// <param name="inner">Inner exeption</param>
        public Byt3Exception(string errorMessage, Exception inner) : base(errorMessage, inner)
        {
            Logger = new ADLLogger<DebugChannel>(GetType().Name);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">The message why this exception occurred</param>
        public Byt3Exception(string errorMessage) : this(errorMessage, null)
        {
        }
    }
}