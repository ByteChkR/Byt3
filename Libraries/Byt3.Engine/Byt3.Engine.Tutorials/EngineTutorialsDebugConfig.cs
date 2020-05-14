using System;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.Engine.Tutorials
{
    public static class EngineTutorialsDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.Engine.Tutorials", LogType.All, Verbosity.Level1,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}