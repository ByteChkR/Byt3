using System;

namespace Byt3.ADL.Configs
{
    public interface IProjectDebugConfig
    {
        string GetProjectName();
        int GetMinSeverity();
        int GetAcceptMask();
        PrefixLookupSettings GetPrefixLookupSettings();
    }

    public class ProjectDebugConfig : IProjectDebugConfig
    {
        public string ProjectName { get; set; }
        public int AcceptMask { get; set; }
        public int MinSeverity { get; set; }
        public PrefixLookupSettings PrefixLookupSettings { get; set; }

        public virtual string GetProjectName() => ProjectName;
        public virtual int GetMinSeverity() => MinSeverity;
        public virtual int GetAcceptMask() => AcceptMask;
        public virtual PrefixLookupSettings GetPrefixLookupSettings() => PrefixLookupSettings;

        public ProjectDebugConfig(string projectName, int acceptMask, int minSeverity,
            PrefixLookupSettings lookupSettings)
        {
            ProjectName = projectName;
            AcceptMask = acceptMask;
            MinSeverity = minSeverity;
            PrefixLookupSettings = lookupSettings;
        }
    }

    public class ProjectDebugConfig<MaskType, SeverityType> : IProjectDebugConfig
        where MaskType : struct
        where SeverityType : struct
    {
        public string ProjectName { get; set; }
        public MaskType AcceptMask { get; set; }
        public SeverityType MinSeverity { get; set; }
        public PrefixLookupSettings PrefixLookupSettings { get; set; }

        public virtual string GetProjectName() => ProjectName;
        public virtual int GetMinSeverity() => Convert.ToInt32(MinSeverity);
        public virtual int GetAcceptMask() => Convert.ToInt32(AcceptMask);
        public virtual PrefixLookupSettings GetPrefixLookupSettings() => PrefixLookupSettings;

        public ProjectDebugConfig(string projectName, MaskType acceptMask, SeverityType minSeverity,
            PrefixLookupSettings lookupSettings)
        {
            ProjectName = projectName;
            AcceptMask = acceptMask;
            MinSeverity = minSeverity;
            PrefixLookupSettings = lookupSettings;
        }
    }


}