using Byt3.Utilities.Exceptions;

namespace Byt3.OpenFL.Serialization.Exceptions
{
    public class FLSerializationException : Byt3Exception
    {
        public FLSerializationException(string errorMessage) : base(errorMessage)
        {
        }
    }
}