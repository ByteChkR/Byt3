using System;

namespace Byt3.PackageHandling
{
    [Flags]
    public enum Byt3HandlerLookupType
    {
        None = 0,
        TraverseUp = 1,
        UseFallback = 2,
        IncludeInterfaces = 4,
    }
}