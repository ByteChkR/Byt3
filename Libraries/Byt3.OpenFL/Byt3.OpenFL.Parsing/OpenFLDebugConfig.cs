using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.OpenFL.Parsing
{
    public static class OpenFLDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.OpenFL.Parsing", LogType.All, Verbosity.Level4,
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Onlyoneprefix);
    }
}