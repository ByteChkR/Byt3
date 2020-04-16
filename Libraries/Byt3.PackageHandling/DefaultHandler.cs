using Byt3.ADL;

namespace Byt3.PackageHandling
{
    internal class DefaultHandler : AHandler
    {
        internal override void Handle(object objectToHandle, object context)
        {
            Logger.Log(LogType.Warning,
                "You are using the Byt3Handler with the UseFallback Flag Set but no custom fallback handler attached. Consider not setting the flag or specifying a fallback handler in the constructor",
                1);
        }
    }
}