using Byt3.Utilities.Exceptions;

namespace Byt3.PackageHandling
{
    public class HandlerNotFoundException : Byt3Exception
    {
        public HandlerNotFoundException(string message) : base(message) { }
    }
}