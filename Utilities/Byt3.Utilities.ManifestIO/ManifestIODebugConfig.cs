﻿using System;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;

namespace Byt3.Utilities.ManifestIO
{
    public static class ManifestIODebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.Utilities.ManifestIO", LogType.All, Verbosity.Level1,
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix);
    }
}