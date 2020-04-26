using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.Utilities.Exceptions;

namespace Byt3.Engine.Debug
{
    public static class EngineDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.Engine", LogType.All, Verbosity.Level4,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
        

        internal static void Crash(this ADLLogger<DebugChannel> logger, Byt3Exception exception, bool throwEx)
        {
            throw exception;
        }
    }
}