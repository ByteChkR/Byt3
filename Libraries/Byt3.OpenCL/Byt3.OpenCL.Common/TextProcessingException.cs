﻿using System;

namespace Byt3.OpenCL.Common
{
    /// <summary>
    /// Exception that occurs when the Text Processor encounters an error.
    /// </summary>
    public class TextProcessingException : ApplicationException
    {
        public TextProcessingException(string errorMessage, ApplicationException inner) : base(errorMessage, inner)
        {
        }
    }
}