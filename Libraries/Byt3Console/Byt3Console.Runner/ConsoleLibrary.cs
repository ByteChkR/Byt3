using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Byt3.Utilities.ConsoleInternals;

namespace Byt3Console.Runner
{
    public class ConsoleLibrary
    {
        public List<ConsoleItem> ConsoleEntries { get; set; } = new List<ConsoleItem>();

        public string InstalledConsoles
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Installed Consoles:");
                for (int i = 0; i < ConsoleEntries.Count; i++)
                {
                    sb.AppendLine($"\t{ConsoleEntries[i].ConsoleTitle} : {ConsoleEntries[i].ConsoleKey}");
                }

                return sb.ToString();
            }
        }

        public bool CheckConsoles()
        {
            bool changed = false;
            List<string> allFiles = Directory
                .GetFiles(ConsolePaths.AssemblyPath, "*Console.*", SearchOption.AllDirectories)
                .Select(x => x.Replace("\\", "/")).ToList();

            string[] removedFiles = FindRemovedFiles(allFiles.ToArray());
            for (int i = ConsoleEntries.Count - 1; i >= 0; i--)
            {
                if (removedFiles.Contains(ConsoleEntries[i].LibPath))
                {
                    changed = true;
                    Console.WriteLine("Removing Missing Console: " + ConsoleEntries[i].ConsoleTitle);
                    ConsoleEntries.RemoveAt(i);
                }
            }

            string[] changedFiles = FindChangedFiles(allFiles.ToArray());
            if (changedFiles.Length != 0)
            {
                changed = true;
                ConsoleEntries.Clear();
            }

            string[] addedFiles = FindAddedFiles(allFiles.ToArray());
            for (int i = 0; i < addedFiles.Length; i++)
            {
                string ext = Path.GetExtension(addedFiles[i]);
                if (!ConsoleRunner.Resolvers.ContainsKey(ext))
                {
                    Console.WriteLine("Can not find Resolver for Extension: " + ext);
                    continue;
                }

                string libPath = ConsoleRunner.Resolvers[ext]
                    .ResolveLibrary(addedFiles[i]);
                changed = true;
                AppDomainController adc = AppDomainController.Create("LoadingAssembly_" + libPath,
                    new[] {Path.GetDirectoryName(libPath)});
                try
                {
                    Type[] consoleTypes = adc.GetTypes(libPath, typeof(AConsole));
                    foreach (Type consoleType in consoleTypes)
                    {
                        ConsoleItem console = new ConsoleItem(adc, libPath, consoleType);
                        Console.WriteLine("Adding New Console: " + console.ConsoleTitle);
                        ConsoleEntries.Add(console);
                    }

                    adc.Dispose();
                }
                catch (Exception e)
                {
                    adc.Dispose();
                    throw e;
                }
            }


            return changed;
        }


        public ConsoleItem GetConsoleItem(string key)
        {
            return ConsoleEntries.FirstOrDefault(x => x.ConsoleKey == key);
        }

        public string[] FindAddedFiles(string[] files)
        {
            return files.Where(x => ConsoleRunner.Resolvers.ContainsKey(Path.GetExtension(x)) &&
                                    ConsoleEntries.All(
                                        y => y.LibPath != ConsoleRunner.Resolvers[Path.GetExtension(x)]
                                                 .ResolveLibrary(x)))
                .ToArray();
        }

        public string[] FindRemovedFiles(string[] files)
        {
            if (ConsoleEntries.Count == 0)
            {
                return new string[0]; //Can not remove files where there aren't any.
            }

            List<string> ret = new List<string>(files);
            for (int i = ret.Count - 1; i >= 0; i--)
            {
                foreach (ConsoleItem consoleEntry in ConsoleEntries)
                {
                    string entryfile = consoleEntry.LibPath;
                    if (ConsoleRunner.Resolvers.TryGetValue(Path.GetExtension(ret[i]), out ResolverWrapper resolver))
                    {
                        string other = resolver.ResolveLibrary(ret[i]);
                        if (other == entryfile)
                        {
                            ret.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            return ret.ToArray();
        }

        public string[] FindChangedFiles(string[] files)
        {
            return files.Where(x => ConsoleEntries.Any(y =>
                y.LibPath == x && y.FileHash !=
                ConsoleItem.GetHash(ConsoleRunner.Resolvers[Path.GetExtension(x)].ResolveLibrary(x)))).ToArray();
        }

        public void Save(string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ConsoleLibrary));
            Stream s = File.Create(filePath);
            xs.Serialize(s, this);
            s.Close();
        }

        public static ConsoleLibrary Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new ConsoleLibrary();
            }

            XmlSerializer xs = new XmlSerializer(typeof(ConsoleLibrary));
            Stream s = File.OpenRead(filePath);
            ConsoleLibrary lib = (ConsoleLibrary) xs.Deserialize(s);
            s.Close();
            return lib;
        }
    }
}