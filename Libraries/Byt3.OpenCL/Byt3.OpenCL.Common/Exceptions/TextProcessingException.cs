using System;

namespace Byt3.OpenCL.Common.Exceptions
{
    /// <summary>
    /// Exception that occurs when the Text Processor encounters an error.
    /// </summary>
    public class TextProcessingException : Byt3Exception
    {
        public TextProcessingException(string errorMessage, ApplicationException inner) : base(errorMessage, inner)
        {
        }
    }
}