using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.PackageHandling
{
    public static class PackageHandlerDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.PackageHandling", LogType.All, Verbosity.Level4,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}