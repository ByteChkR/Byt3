using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.ExtPP.Base
{
    public static class ExtPPDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.ExtPP", LogType.All, Verbosity.Level1,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}