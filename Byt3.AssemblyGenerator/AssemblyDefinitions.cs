using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Byt3.BuildSystem
{
    [Serializable]
    public class AssemblyDefinitions
    {
        public BuildType Type = BuildType.Build;
        public List<ModuleDefinition> Definitions;
        public bool NoTargetRuntime = false;
        public string AssemblyName = "TestAssembly";
        public string BuildConfiguration = "Release";
        public string BuildTargetRuntime = "win-x64";

        public AssemblyDefinitions()
        {
            Definitions = new List<ModuleDefinition>();
        }

        public AssemblyDefinitions(BuildType buildType, string assemblyName, string buildConfiguration = "Release", bool noTargetRuntime = false, string buildTargetRuntime = "win-x64") : this()
        {
            Type = buildType;
            AssemblyName = assemblyName;
            BuildConfiguration = buildConfiguration;
            NoTargetRuntime = noTargetRuntime;
            BuildTargetRuntime = buildTargetRuntime;
        }

        public static AssemblyDefinitions Load(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(AssemblyDefinitions));
            Stream s = File.OpenRead(path);
            AssemblyDefinitions ret = (AssemblyDefinitions)xs.Deserialize(s);
            s.Close();
            return ret;
        }

        public static void Save(string path, AssemblyDefinitions definition)
        {
            XmlSerializer xs = new XmlSerializer(typeof(AssemblyDefinitions));
            Stream s = File.Create(path);
            xs.Serialize(s, definition);
            s.Close();
        }
    }
}