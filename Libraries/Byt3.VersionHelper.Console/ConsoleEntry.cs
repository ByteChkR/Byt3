using System;
using System.IO;
using System.Xml;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.Versioning;
using Byt3.VersionHelper.Console.Commands;

namespace Byt3.VersionHelper.Console
{
    public class ConsoleEntry
    {

        public string ConsoleKey => "vh";
        public void Run(string[] args)
        {
            Debug.DefaultInitialization();
            VersionAccumulatorManager.SearchForAssemblies();
            Runner.AddCommand(new DefaultHelpCommand());
            Runner.AddCommand(new ChangeVersionCommand());
            Runner.RunCommands(args);
        }

        public static void ChangeVersionInFile(string file, Version newVersion)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode[] nodes = FindVersionTags(doc);
            Version v = Version.Parse(nodes[0].InnerText);
            nodes[0].InnerText = newVersion.ToString();
            nodes[1].InnerText = newVersion.ToString();

            File.Delete(file);
            doc.Save(file);
        }

        public static Version GetVersionFromFile(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            return FindVersion(doc);
        }

        public static Version ChangeVersion(Version version, string changeStr)
        {
            string[] subVersions = changeStr.Split('.');
            int[] versions = new[] { version.Major, version.Minor, version.Build, version.Revision };
            for (int i = 0; i < 4; i++)
            {
                string current = subVersions[i];
                if (current == "+")
                {
                    versions[i]++;
                }
                else if (current == "-" && versions[i] != 0)
                {
                    versions[i]--;
                }
                else if (current == "X")
                {
                    continue;
                }
                else if (current.StartsWith("{") && current.EndsWith("}"))
                {
                    string format = current.Remove(current.Length - 1, 1).Remove(0, 1);

                    string value = DateTime.Now.ToString(format);

                    if (long.TryParse(value, out long newValue))
                    {
                        versions[i] = (int)(newValue % ushort.MaxValue);
                    }
                    else
                    {
                        System.Console.WriteLine("Can not Parse: " + value + " to INT");
                    }

                }
                else
                {
                    versions[i] = int.Parse(current);
                }
            }
            return new Version(versions[0], versions[1], versions[2], versions[3]);
        }

        private static Version FindVersion(XmlDocument doc)
        {
            if (Version.TryParse(FindVersionTags(doc)[0].InnerText, out Version v)) return v;
            return new Version(0, 0, 1, 0);
        }

        private static XmlNode[] FindVersionTags(XmlDocument doc)
        {
            string s1 = doc.Name;

            XmlNode s = null;

            for (int i = 0; i < doc.ChildNodes.Count; i++)
            {
                if (doc.ChildNodes[i].Name == "Project") s = doc.ChildNodes[i];
            }

            XmlNode[] ret = new XmlNode[2];
            for (int i = 0; i < s.ChildNodes.Count; i++)
            {
                if (s.ChildNodes[i].Name == "PropertyGroup")
                {
                    if (s.ChildNodes[i].HasChildNodes && s.ChildNodes[i].FirstChild.Name == "TargetFramework")
                    {
                        for (int j = 0; j < s.ChildNodes[i].ChildNodes.Count; j++)
                        {
                            XmlNode projTag = s.ChildNodes[i].ChildNodes[j];
                            if (projTag.Name == "AssemblyVersion")
                            {
                                ret[0] = projTag;
                            }
                            else if (projTag.Name == "FileVersion")
                            {
                                ret[1] = projTag;
                            }
                        }
                    }
                }
            }

            if (ret[0] == null || ret[1] == null)
            {
                for (int i = 0; i < s.ChildNodes.Count; i++)
                {
                    if (s.ChildNodes[i].Name == "PropertyGroup")
                    {
                        if (s.ChildNodes[i].HasChildNodes && s.ChildNodes[i].FirstChild.Name == "TargetFramework")
                        {
                            XmlNode assemblyVersion = s.ChildNodes[i].AppendChild(doc.CreateNode(XmlNodeType.Element, "AssemblyVersion", ""));
                            assemblyVersion.InnerText = "0.0.1.0";
                            XmlNode fileVersion = s.ChildNodes[i].AppendChild(doc.CreateNode(XmlNodeType.Element, "FileVersion", ""));
                            fileVersion.InnerText = "0.0.1.0";
                            ret[0] = assemblyVersion;
                            ret[1] = fileVersion;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
