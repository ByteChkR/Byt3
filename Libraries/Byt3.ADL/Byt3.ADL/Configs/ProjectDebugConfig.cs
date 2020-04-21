using System;

namespace Byt3.ADL.Configs
{
    public class ProjectDebugConfig : IProjectDebugConfig
    {
        public string ProjectName { get; set; }
        public int AcceptMask { get; set; }
        public int MinSeverity { get; set; }
        public PrefixLookupSettings PrefixLookupSettings { get; set; }

        public virtual string GetProjectName()
        {
            return ProjectName;
        }

        public virtual int GetMinSeverity()
        {
            return MinSeverity;
        }

        public virtual int GetAcceptMask()
        {
            return AcceptMask;
        }

        public virtual PrefixLookupSettings GetPrefixLookupSettings()
        {
            return PrefixLookupSettings;
        }

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

        public virtual string GetProjectName()
        {
            return ProjectName;
        }

        public virtual int GetMinSeverity()
        {
            return Convert.ToInt32(MinSeverity);
        }

        public virtual int GetAcceptMask()
        {
            return Convert.ToInt32(AcceptMask);
        }

        public virtual PrefixLookupSettings GetPrefixLookupSettings()
        {
            return PrefixLookupSettings;
        }

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