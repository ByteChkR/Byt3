using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.CommandRunner
{
    public static class CommandRunnerDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.CommandRunner", LogType.All, Verbosity.Level4,
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Onlyoneprefix);
    }
}