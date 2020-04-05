using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Byt3.BuildSystem
{
    [Serializable]
    public class ModuleDefinition
    {
        public string Name;
        public string RootDirectory;
        public string[] ScriptFiles => Directory.GetFiles(RootDirectory, "*.cs", SearchOption.AllDirectories).Where(x => !x.Contains("\\bin\\") && !x.Contains("\\obj\\")).ToArray();
        public string[] EmbedFiles = new string[0];

        public ModuleDefinition() { }

        public ModuleDefinition(string name, string rootDirectory)
        {
            Name = name;
            RootDirectory = rootDirectory;
        }

        public static ModuleDefinition Load(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ModuleDefinition));
            Stream s = File.OpenRead(path);
            ModuleDefinition ret = (ModuleDefinition)xs.Deserialize(s);
            s.Close();
            return ret;
        }

        public static void Save(string path, ModuleDefinition definition)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ModuleDefinition));
            Stream s = File.Create(path);
            xs.Serialize(s, definition);
            s.Close();
        }

    }
}