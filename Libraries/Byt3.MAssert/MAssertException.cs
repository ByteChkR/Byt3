using System;

namespace Byt3.MAssert
{
    public class MAssertException : Exception
    {

        public MAssertException(string message, Exception inner) : base(message, inner) { }
        public MAssertException(string message) : base(message) { }
        public MAssertException(object actual, object expected) : base($"Assertion Failed. Expected: {expected} but got {actual ?? "NULL"}.") { }

    }
}