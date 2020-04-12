using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Byt3.Utilities.DotNet
{
    [Serializable]
    public class AssemblyDefinition
    {
        public List<ModuleDefinition> IncludedModules;
        public bool NoTargetRuntime = false;
        public string AssemblyName = "TestAssembly";
        public string BuildConfiguration = "Release";
        public string BuildTargetRuntime = "win-x64";

        public AssemblyDefinition()
        {
            IncludedModules = new List<ModuleDefinition>();
        }

        public AssemblyDefinition(string assemblyName, string buildConfiguration = "Release",
            bool noTargetRuntime = false, string buildTargetRuntime = "win-x64") : this()
        {
            AssemblyName = assemblyName;
            BuildConfiguration = buildConfiguration;
            NoTargetRuntime = noTargetRuntime;
            BuildTargetRuntime = buildTargetRuntime;
        }

        public static AssemblyDefinition Load(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(AssemblyDefinition));
            Stream s = File.OpenRead(path);
            AssemblyDefinition ret = (AssemblyDefinition) xs.Deserialize(s);
            s.Close();
            return ret;
        }

        public static void Save(string path, AssemblyDefinition definition)
        {
            XmlSerializer xs = new XmlSerializer(typeof(AssemblyDefinition));
            Stream s = File.Create(path);
            xs.Serialize(s, definition);
            s.Close();
        }
    }
}