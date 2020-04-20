using Byt3.Utilities.Exceptions;

namespace Byt3.ExtPP.Plugins
{
    public class ErrorException : Byt3Exception
    {
        public ErrorException(string message) : base(message)
        {
        }
    }
}