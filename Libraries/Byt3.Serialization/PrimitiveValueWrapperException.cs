using Byt3.Utilities.Exceptions;

namespace Byt3.Serialization
{
    public class PrimitiveValueWrapperException : Byt3Exception
    {
        public PrimitiveValueWrapperException(string message) : base(message)
        {
        }
    }
}