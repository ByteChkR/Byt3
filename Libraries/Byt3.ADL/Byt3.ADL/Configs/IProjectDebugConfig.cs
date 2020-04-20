namespace Byt3.ADL.Configs
{
    public interface IProjectDebugConfig
    {
        string GetProjectName();
        int GetMinSeverity();
        int GetAcceptMask();
        PrefixLookupSettings GetPrefixLookupSettings();
    }
}