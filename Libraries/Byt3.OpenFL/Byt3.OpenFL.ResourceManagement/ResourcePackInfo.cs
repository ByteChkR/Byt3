using System.Xml.Serialization;

namespace Byt3.OpenFL.ResourceManagement
{
    public class ResourcePackInfo
    {
        public string Name;
        public string UnpackerConfig;
        [XmlIgnore]
        public string ResourceData;

        public override string ToString()
        {
            return $"Name: {Name}, Config:{UnpackerConfig}";
        }
    }
}
