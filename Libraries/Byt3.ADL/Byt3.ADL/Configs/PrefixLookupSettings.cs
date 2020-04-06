using System;

namespace Byt3.ADL.Configs
{
    [Flags]
    public enum PrefixLookupSettings
    {
        Noprefix = 0,
        Addprefixifavailable = 1,
        Deconstructmasktofind = 2,
        Onlyoneprefix = 4,
        Bakeprefixes = 8
    }
}