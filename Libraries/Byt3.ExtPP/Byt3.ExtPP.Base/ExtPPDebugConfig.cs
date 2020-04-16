using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.ExtPP.Base
{
    public class ExtPPDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.ExtPP", LogType.All, Verbosity.Level4,
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Onlyoneprefix);
    }
}