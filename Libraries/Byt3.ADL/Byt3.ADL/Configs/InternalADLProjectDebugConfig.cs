namespace Byt3.ADL.Configs
{
    public class InternalADLProjectDebugConfig
    {
        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>("Byt3.ADL", LogType.All, Verbosity.Level8,
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Bakeprefixes |
                PrefixLookupSettings.Deconstructmasktofind);
    }
}