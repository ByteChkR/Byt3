using Byt3.Utilities.Exceptions;

namespace Byt3.OpenFL.Serialization.Exceptions
{
    public class FLDeserializationException : Byt3Exception
    {
        public FLDeserializationException(string errorMessage) : base(errorMessage) { }
    }
}