using System.Xml.Serialization;

namespace Byt3.BuildSystem.Settings
{
    [XmlType(IncludeInSchema = false, TypeName = "BuildSettings")]
    public class BuildSettings
    {
        [XmlArrayItem(ElementName = "StageConfig")]
        public BuildStageSettings[] StageSettings;
    }
}