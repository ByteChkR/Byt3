using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.OpenCL.Wrapper
{
    public class OpenCLDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.OpenCL", LogType.All, Verbosity.Level4,
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Onlyoneprefix);
    }
}