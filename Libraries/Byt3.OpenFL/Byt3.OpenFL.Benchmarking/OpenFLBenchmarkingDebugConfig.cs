using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.OpenFL.Benchmarking
{
    public static class OpenFLBenchmarkingDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.OpenFL.Benchmarking", LogType.All, Verbosity.Level1,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}