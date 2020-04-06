using System;

namespace Byt3.OpenCL.Common.Exceptions
{
    /// <summary>
    /// This exception gets thrown when the specified file was not found.
    /// </summary>
    public class InvalidFilePathException : Byt3Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">The File that was not found</param>
        /// <param name="inner">Inner exeption</param>
        public InvalidFilePathException(string filePath, Exception inner) : base(
            "The file " + filePath + " could not be found.", inner)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">The File that was not found</param>
        public InvalidFilePathException(string filePath) : this(filePath, null)
        {
            CLLogger.Log("Files:", DebugChannel.Error, 10);
        }
    }
}