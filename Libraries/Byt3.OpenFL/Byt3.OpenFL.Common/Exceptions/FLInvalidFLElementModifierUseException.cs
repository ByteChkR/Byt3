using System;
using Byt3.Utilities.Exceptions;

namespace Byt3.OpenFL.Common.Exceptions
{
    public class FLInvalidFLElementModifierUseException : Byt3Exception
    {
        public FLInvalidFLElementModifierUseException(string function, string modifier, string errorMessage,
            Exception inner) : base(
            $"The modifier {modifier} is used incorrectly on {function}: \n{errorMessage}", inner)
        {
        }

        public FLInvalidFLElementModifierUseException(string function, string modifer, string errorMessage) : this(
            function, modifer, errorMessage, null)
        {
        }
    }
}