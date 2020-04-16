using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Byt3.ADL;
using Byt3.CommandRunner;

namespace Byt3.OpenFL.CLI.Commands
{
    public class SetSettingsCommand : AbstractCommand
    {
        private static Dictionary<string, FieldInformations> RootNodes;

        public List<string> AllPaths
        {
            get
            {
                List<string> ret = new List<string>();
                RootNodes.Select(x => x.Value.Fields.Select(y => y.Key)).ToList().ForEach(x => ret.AddRange(x));
                return ret;
            }
        }

        


        public SetSettingsCommand(Dictionary<string, FieldInformations> rootNodes) :
            base( new[] {"--set-settings", "-ss" }, "Sets the Settings")
        {
            CommandAction = (info, strings) => ReflectData(strings);
            RootNodes = rootNodes;
            Logger.Log(LogType.Log,"Root Nodes: " + RootNodes.Count, 1);
        }

        private void ReflectData(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string[] parts = args[i].Split(':');
                string fullpath = parts[0];
                Logger.Log(LogType.Log, "Trying to Find Field: " + fullpath,3);
                if (parts.Length != 2 || fullpath.IndexOf('.') == -1)
                {
                    continue;
                }
                string root = fullpath.Substring(0, fullpath.IndexOf('.'));
                string data = parts[1];
                if (RootNodes.ContainsKey(root))
                {
                    FieldInformations fis = RootNodes[root];
                    if (fis.Fields.ContainsKey(fullpath))
                    {
                        Logger.Log(LogType.Log, "Setting " + fullpath + " to " + data, 1);
                        FieldInformation fi = fis.Fields[fullpath];
                        if (fi.info.FieldType == typeof(float))
                        {
                            fi.info.SetValue(fi.reference, float.Parse(data));
                        }
                        else if (fi.info.FieldType == typeof(int))
                        {
                            fi.info.SetValue(fi.reference, int.Parse(data));
                        }
                        else if (fi.info.FieldType == typeof(string))
                        {
                            fi.info.SetValue(fi.reference, data);
                        }
                        else if (fi.info.FieldType == typeof(bool))
                        {
                            fi.info.SetValue(fi.reference, bool.Parse(data));
                        }
                    }
                    else
                    {
                        Logger.Log(LogType.Error, "Can not Find a Field with Path: " + fullpath, 1);
                    }
                }
                else
                {
                    Logger.Log(LogType.Error,"Can not Find a Field with Path: " + fullpath, 1);
                }
            }
        }

        private static void RecursiveAddFields(string parentPath, object parentRef, FieldInfo current, Dictionary<string, FieldInformation> infos)
        {
            string thisPath = parentPath + "." + current.Name;
            if (current.FieldType == typeof(float) || current.FieldType == typeof(string) ||
                current.FieldType == typeof(int) || current.FieldType == typeof(bool))
            {
                infos.Add(thisPath, new FieldInformation() { reference = parentRef, info = current, path = thisPath });
            }
            else
            {
                object newref = current.GetValue(parentRef);
                FieldInfo[] subs = GetFieldsOfType(newref.GetType());
                foreach (FieldInfo fieldInfo in subs)
                {
                    RecursiveAddFields(thisPath, newref, fieldInfo, infos);
                }
            }
        }

        private static FieldInfo[] GetFieldsOfType(Type t)
        {
            return t.GetFields();
        }


        private static Dictionary<string, FieldInformation> GenerateFieldInformation(string rootNode, object o)
        {
            Dictionary<string, FieldInformation> ret = new Dictionary<string, FieldInformation>();
            FieldInfo[] fis = GetFieldsOfType(o.GetType());
            foreach (FieldInfo fieldInfo in fis)
            {
                RecursiveAddFields(rootNode, o, fieldInfo, ret);
            }

            return ret;
        }


        public static Dictionary<string, FieldInformations> Create(params ObjectFieldContainer[] objs)
        {
            Dictionary<string, FieldInformations> ret = new Dictionary<string, FieldInformations>();
            for (int i = 0; i < objs.Length; i++)
            {
                ret.Add(objs[i].RootName, new FieldInformations(objs[i].Fields));
            }

            return ret;
        }

        public struct ObjectFieldContainer
        {
            public string RootName;
            public Dictionary<string, FieldInformation> Fields;

            public ObjectFieldContainer(string rootName, Dictionary<string, FieldInformation> fields)
            {
                Fields = fields;
                RootName = rootName;
            }
        }

        public static ObjectFieldContainer Create(string rootName, params object[] o)
        {
            Dictionary<string, FieldInformation> ret = new Dictionary<string, FieldInformation>();
            for (int i = 0; i < o.Length; i++)
            {
                Dictionary<string, FieldInformation> sub = GenerateFieldInformation(rootName, o[i]);
                foreach (KeyValuePair<string, FieldInformation> fieldInformation in sub)
                {
                    ret.Add(fieldInformation.Key, fieldInformation.Value);
                }
            }

            return new ObjectFieldContainer(rootName, ret);
        }

        //private static KeyValuePair<string, FieldInformations> Create(string RootName, object o)
        //{
        //	return new KeyValuePair<string, FieldInformations>(RootName, new FieldInformations(GenerateFieldInformation(RootName, o)));
        //}
    }
}