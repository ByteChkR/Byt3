using System;
using System.Globalization;
using System.Xml;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Utilities.Console.Internals;
using Byt3.Utilities.Versioning;
using Byt3.VersionHelper.Console.Commands;

namespace Byt3.VersionHelper.Console
{
    public class ConsoleEntry : AConsole
    {
        public override string ConsoleKey => "vh";
        public override string ConsoleTitle => "CSProj Verison Helper";

        public override bool Run(string[] args)
        {
            Debug.DefaultInitialization();
            VersionAccumulatorManager.SearchForAssemblies();
            Runner.AddCommand(new DefaultHelpCommand());
            Runner.AddCommand(new NoWrapFlagCommand());
            Runner.AddCommand(new ChangeVersionCommand());
            return Runner.RunCommands(args);
        }

        public static void ChangeVersionInFile(string file, Version newVersion)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode[] nodes = FindVersionTags(doc);

            nodes[0].InnerText = newVersion.ToString();
            nodes[1].InnerText = newVersion.ToString();
            try
            {
                doc.Save(file);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
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
            int[] wrapValues = new[] {ushort.MaxValue, 9, 99, ushort.MaxValue};
            int[] versions = new[] {version.Major, version.Minor, version.Build, version.Revision};
            for (int i = 4 - 1; i >= 0; i--)
            {
                string current = subVersions[i];
                if (current.StartsWith("("))
                {
                    if (i == 0)
                    {
                        continue; //Can not wrap the last digit
                    }

                    int j = 0;
                    for (; j < current.Length; j++)
                    {
                        if (current[j] == ')')
                        {
                            break;
                        }
                    }

                    if (j == current.Length)
                    {
                        System.Console.WriteLine($"Can not parse version ID: {i}({current})");
                        continue; //Broken. No number left. better ignore
                    }

                    string max = current.Substring(1, j - 1);
                    if (int.TryParse(max, out int newMax))
                    {
                        wrapValues[i] = newMax;
                    }

                    current = current.Remove(0, j + 1);
                }

                if (!NoWrapFlagCommand.NoWrap && i != 0) //Check if we wrapped
                {
                    if (versions[i] >= wrapValues[i])
                    {
                        versions[i] = 0;
                        versions[i - 1]++;
                    }
                }

                if (current == "+")
                {
                    versions[i]++;
                }
                else if (current == "-" && versions[i] != 0)
                {
                    versions[i]--;
                }
                else if (current.ToLower(CultureInfo.InvariantCulture) == "x")
                {
                    continue;
                }
                else if (current.StartsWith("{") && current.EndsWith("}"))
                {
                    string format = current.Remove(current.Length - 1, 1).Remove(0, 1);

                    string value = DateTime.Now.ToString(format);

                    if (long.TryParse(value, out long newValue))
                    {
                        versions[i] = (int) (newValue % ushort.MaxValue);
                    }
                    else
                    {
                        System.Console.WriteLine("Can not Parse: " + value + " to INT");
                    }
                }
                else if (int.TryParse(current, out int v))
                {
                    versions[i] = v;
                }
            }

            return new Version(versions[0], versions[1], versions[2], versions[3]);
        }

        private static Version FindVersion(XmlDocument doc)
        {
            if (Version.TryParse(FindVersionTags(doc)[0].InnerText, out Version v))
            {
                return v;
            }

            return new Version(0, 0, 1, 0);
        }

        private static XmlNode[] FindVersionTags(XmlDocument doc)
        {
            string s1 = doc.Name;

            XmlNode s = null;

            for (int i = 0; i < doc.ChildNodes.Count; i++)
            {
                if (doc.ChildNodes[i].Name == "Project")
                {
                    s = doc.ChildNodes[i];
                }
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
                            XmlNode assemblyVersion = s.ChildNodes[i]
                                .AppendChild(doc.CreateNode(XmlNodeType.Element, "AssemblyVersion", ""));
                            assemblyVersion.InnerText = "0.0.1.0";
                            XmlNode fileVersion = s.ChildNodes[i]
                                .AppendChild(doc.CreateNode(XmlNodeType.Element, "FileVersion", ""));
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