using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.Engine.Demos
{
    public static class EngineDemoDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.Engine.Demos", LogType.All, Verbosity.Level1,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}