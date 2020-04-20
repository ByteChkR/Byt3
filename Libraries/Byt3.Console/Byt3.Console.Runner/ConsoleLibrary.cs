using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Byt3.Console.Runner
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
            string[] allFiles = Directory.GetFiles(ConsolePaths.AssemblyPath, "*.Console.dll", SearchOption.AllDirectories).Select(x => x.Replace("\\", "/")).ToArray();

            string[] removedFiles = FindRemovedFiles(allFiles);
            for (int i = ConsoleEntries.Count - 1; i >= 0; i--)
            {
                if (removedFiles.Contains(ConsoleEntries[i].LibPath))
                {
                    changed = true;
                    System.Console.WriteLine("Removing Missing Console: " + ConsoleEntries[i].ConsoleTitle);
                    ConsoleEntries.RemoveAt(i);
                }
            }

            string[] changedFiles = FindChangedFiles(allFiles);
            if (changedFiles.Length != 0)
            {
                changed = true;
                ConsoleEntries.Clear();
            }

            string[] addedFiles = FindAddedFiles(allFiles);
            for (int i = 0; i < addedFiles.Length; i++)
            {

                changed = true;
                AppDomainController adc = AppDomainController.Create("LoadingAssembly_" + Path.GetFileNameWithoutExtension(addedFiles[i]), new[] { Path.GetDirectoryName(addedFiles[i]) });
                try
                {
                    Type[] consoleTypes = adc.GetTypes(addedFiles[i], "ConsoleEntry");
                    foreach (Type consoleType in consoleTypes)
                    {
                        ConsoleItem console = new ConsoleItem(adc, addedFiles[i], consoleType);
                        System.Console.WriteLine("Adding New Console: " + console.ConsoleTitle);
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

        //public bool TryGetConsole(string key, out ConsoleItem console)
        //{
        //    ConsoleItem item = ConsoleEntries.FirstOrDefault(x => x.ConsoleKey == key);
        //    console = item;
        //    return item != null;
        //}


        //private bool TryGetConsoleTypes(string file, out Type[] consoles)
        //{
        //    try
        //    {
        //        Assembly asm = ConsoleItem.LoadAssembly(file);
        //        consoles = asm.GetExportedTypes().Where(x => x.Name == "ConsoleEntry").ToArray();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        consoles = new Type[0];
        //        return false;
        //    }
        //}


        public ConsoleItem GetConsoleItem(string key)
        {
            return ConsoleEntries.FirstOrDefault(x => x.ConsoleKey == key);
        }

        public string[] FindAddedFiles(string[] files)
        {
            return files.Where(x => ConsoleEntries.All(y => y.LibPath != x)).ToArray();
        }

        public string[] FindRemovedFiles(string[] files)
        {
            List<string> ret = new List<string>(files);
            for (int i = ret.Count - 1; i >= 0; i--)
            {
                foreach (ConsoleItem consoleEntry in ConsoleEntries)
                {
                    if (ret[i] == consoleEntry.LibPath)
                    {
                        ret.RemoveAt(i);
                        break;
                    }
                }
            }

            return ret.ToArray();
        }

        public string[] FindChangedFiles(string[] files)
        {
            return files.Where(x => ConsoleEntries.Any(y => y.LibPath == x && y.FileHash != ConsoleItem.GetHash(x))).ToArray();
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
            if (!File.Exists(filePath)) return new ConsoleLibrary();
            XmlSerializer xs = new XmlSerializer(typeof(ConsoleLibrary));
            Stream s = File.OpenRead(filePath);
            ConsoleLibrary lib = (ConsoleLibrary)xs.Deserialize(s);
            s.Close();
            return lib;
        }
    }
}