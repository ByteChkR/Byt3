using System;

namespace Byt3.OpenCL.Common.Exceptions
{

    /// <summary>
    /// This Exception occurs when the FLInterpreter is not able to find the argument type through deduction
    /// </summary>
    public class FLInvalidArgumentType : Byt3Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="varname">Variable name that is affected</param>
        /// <param name="expected">The expected value for the variable</param>
        /// <param name="inner">Inner exeption</param>
        public FLInvalidArgumentType(string varname, string expected, Exception inner) : base(
            $"Argument {varname} has the wrong type or is Null. Expected: {expected}", inner)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="varname">Variable name that is affected</param>
        /// <param name="expected">The expected value for the variable</param>
        public FLInvalidArgumentType(string varname, string expected) : this(varname, expected, null)
        {
        }
    }
}