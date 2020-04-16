using System;
using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.Utilities.Exceptions
{
    /// <summary>
    /// Exception that the SuperClass of all exceptions that get thrown with logger.crash
    /// </summary>
    public class Byt3Exception : ApplicationException
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.Utilities.Exceptions", LogType.All, Verbosity.Level4,
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Onlyoneprefix);

        protected readonly ADLLogger<LogType> Logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">The message why this exception occurred</param>
        /// <param name="inner">Inner exeption</param>
        public Byt3Exception(string errorMessage, Exception inner) : base(errorMessage, inner)
        {
            Logger = new ADLLogger<LogType>(Settings,GetType().Name);
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