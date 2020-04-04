using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Byt3.BuildSystem.Settings;
using Byt3.BuildSystem.Stages;

namespace Byt3.BuildSystem
{
    public class Builder
    {

        private readonly List<BuilderStage> Stages = new List<BuilderStage>();

        public void AddBuilderStage(BuilderStage stage)
        {
            Stages.Add(stage);
        }

        public void LoadBuildSettings(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(BuildSettings), FindBuildSettingsTypes());
            Stream s = File.OpenRead(file);
            BuildSettings obj = (BuildSettings)xs.Deserialize(s);

            BuilderStage[] stages = obj.StageSettings.Select(x => (BuilderStage)Activator.CreateInstance(x.BuilderStageType))
                .ToArray();
            Stages.Clear();
            Stages.AddRange(stages);

            s.Close();
        }

        private Type[] FindBuildSettingsTypes()
        {
            List<Type> types = new List<Type>();
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < asms.Length; i++)
            {
                List<Type> ts = asms[i].GetTypes().ToList();
                for (int j = ts.Count - 1; j >= 0; j--)
                {
                    Type x = ts[j];
                    bool subclass = x.IsSubclassOf(typeof(BuildStageSettings));
                    if (x.IsAbstract || x.ContainsGenericParameters || !subclass)
                    {
                        ts.RemoveAt(j);
                        continue;
                    }
                }
                types.AddRange(ts);
            }
            return types.ToArray();
        }



        public void GenerateBuildSettings(string file)
        {
            Type[] buildSettingsTypes = FindBuildSettingsTypes();
            BuildSettings settings = new BuildSettings() { StageSettings = Stages.Select(x => x.SettingsObj).ToArray() };
            
            string sContent = Xml.Serialize(settings, buildSettingsTypes);

            

            Stream s = File.Create(file);
            TextWriter tw = new StreamWriter(s);
            tw.Write(sContent);
            tw.Close();
            s.Close();

        }


    }
}