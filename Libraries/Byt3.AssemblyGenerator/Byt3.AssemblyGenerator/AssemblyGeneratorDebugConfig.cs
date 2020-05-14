using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.AssemblyGenerator
{
    public static class AssemblyGeneratorDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.AssemblyGenerator", LogType.All, Verbosity.Level1,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}