using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.Utilities.DotNet
{
    public static class UtilitiesDotNetDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.Utilities.DotNet", LogType.All, Verbosity.Level1,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}