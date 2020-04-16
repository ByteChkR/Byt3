using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.Utilities.Threading
{
    public class UtilitiesDotNetDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.Utilities.DotNet", LogType.All, Verbosity.Level8,
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Bakeprefixes |
                PrefixLookupSettings.Deconstructmasktofind);
    }
}