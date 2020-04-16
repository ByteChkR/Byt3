using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.CommandRunner
{
    public class CommandRunnerDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.CommandRunner", LogType.All, Verbosity.Level8,
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Bakeprefixes |
                PrefixLookupSettings.Deconstructmasktofind);
    }
}