using System;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.OpenFL.Common
{
    public static class OpenFLDebugConfig
    {
        public static Version CommonVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.OpenFL.Parsing", LogType.All, Verbosity.Level4,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}