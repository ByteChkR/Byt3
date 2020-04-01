using System;

namespace Byt3.PackageHandling
{
    [Flags]
    public enum HandlerLookupType
    {
        None = 0,
        TraverseUp = 1,
        UseFallback = 2,
    }
}