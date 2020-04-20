using Byt3.Utilities.Exceptions;

namespace Byt3.OpenFL.Common.Exceptions
{
    public class FLBufferCreatorNotFoundException : Byt3Exception
    {
        public FLBufferCreatorNotFoundException(string errorMessage) : base(errorMessage) { }
    }
}