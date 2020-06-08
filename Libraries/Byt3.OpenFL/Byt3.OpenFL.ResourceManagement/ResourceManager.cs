using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;
using Byt3.Callbacks;
using Byt3.Utilities.ProgressFeedback;

namespace Byt3.OpenFL.ResourceManagement
{
    public static class ResourceManager
    {

        private static string ResourceDirectory = Path.Combine(Directory.GetCurrentDirectory());

        private static Dictionary<string, ResourcePackInfo> LoadedPacks = new Dictionary<string, ResourcePackInfo>();
        private static List<ResourceTypeUnpacker> Unpacker = new List<ResourceTypeUnpacker>() { new DefaultUnpacker()};

        public static void AddUnpacker(ResourceTypeUnpacker unpacker)
        {
            Unpacker.Add(unpacker);
        }

        public static string Load(string packPath)
        {
            if (IOManager.FileExists(packPath))
            {
                try
                {
                    Stream archStream = IOManager.GetStream(packPath);
                    ZipArchive arch = new ZipArchive(archStream);
                    XmlSerializer xs = new XmlSerializer(typeof(ResourcePackInfo));
                    Stream s = arch.GetEntry("Info.xml").Open();
                    ResourcePackInfo info = (ResourcePackInfo)xs.Deserialize(s);
                    info.ResourceData = packPath;
                    LoadedPacks[info.Name] = info;
                    s.Dispose();
                    arch.Dispose();
                    archStream.Dispose();
                    return info.Name;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return "";
        }

        public static void Activate(string name, IProgressIndicator ProgressInfo, string targetDir = null)
        {
            if (targetDir == null) targetDir = ResourceDirectory;
            if (LoadedPacks.ContainsKey(name))
            {
                ProgressInfo.SetProgress("Loading Package...", 1, 3);
                Stream archStream = IOManager.GetStream(LoadedPacks[name].ResourceData);
                ZipArchive arch = new ZipArchive(archStream);

                ProgressInfo.SetProgress("Preparing Unpackers...", 2, 3);
                Dictionary<string, string[]> unpackers = new Dictionary<string, string[]>(LoadedPacks[name].UnpackerConfig
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x =>
                    {
                        KeyValuePair<string, string[]> ret = new KeyValuePair<string, string[]>(x.Split('|')[0], x.Split('|'));
                        List<string> temp = ret.Value.ToList();
                        temp.RemoveAt(0);
                        ret = new KeyValuePair<string, string[]>(ret.Key, temp.ToArray());
                        return ret;
                    }).ToDictionary(x => x.Key, y => y.Value));

                ProgressInfo.SetProgress("Unpacking...", 3, 3);
                IProgressIndicator perFileProgress = ProgressInfo.CreateSubTask();
                int fileCount = 0;
                for (int i = 0; i < arch.Entries.Count; i++)
                {
                    if (arch.Entries[i].Name == "Info.xml") continue;
                    bool unpacked = false;
                    foreach (KeyValuePair<string, string[]> keyValuePair in unpackers)
                    {
                        if (keyValuePair.Value.Contains(Path.GetExtension(arch.Entries[i].Name)))
                        {
                            ResourceTypeUnpacker u =
                                Unpacker.FirstOrDefault(x => x.UnpackerName == keyValuePair.Key);
                            if (u == null)
                            {
                                continue;
                            }
                            perFileProgress.SetProgress("Unpacking:" + arch.Entries[i].FullName, fileCount, arch.Entries.Count - 1);
                            fileCount++;
                            u.Unpack(targetDir, arch.Entries[i].FullName, arch.Entries[i].Open(), perFileProgress.CreateSubTask());
                            unpacked = true;
                            break;
                        }
                    }

                    if (!unpacked)
                    {
                        perFileProgress.SetProgress("Unpacking:" + arch.Entries[i].FullName, fileCount, arch.Entries.Count - 1);
                        fileCount++;
                        string key = unpackers.Where(x => x.Value.Length == 0).Select(x => x.Key).FirstOrDefault();
                        if (key == null) continue;
                        ResourceTypeUnpacker u =
                            Unpacker.FirstOrDefault(x => x.UnpackerName == key);
                        if (u == null) continue;
                        u.Unpack(targetDir, arch.Entries[i].FullName, arch.Entries[i].Open(), perFileProgress.CreateSubTask());
                    }
                }
                perFileProgress.Dispose();
                ProgressInfo.Dispose();
            }
        }

        public static void Create(string folder, string targetFile, string name, string unpackerConfig, IProgressIndicator ProgressInfo)
        {
            ResourcePackInfo info = new ResourcePackInfo() { Name = name, UnpackerConfig = unpackerConfig };
            ProgressInfo.SetProgress("Writing Package Info: " + info, 1, 2);
            XmlSerializer xs = new XmlSerializer(typeof(ResourcePackInfo));
            Stream s = File.OpenWrite(Path.Combine(folder, "Info.xml"));
            xs.Serialize(s, info);
            s.Dispose();

            ProgressInfo.SetProgress("Creating Package from Folder: " + folder, 1, 2);

            ZipArchive arch = new ZipArchive(File.OpenWrite(targetFile), ZipArchiveMode.Create);

            string[] files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);

            IProgressIndicator perFileProgressIndicator = ProgressInfo.CreateSubTask();

            for (int i = 0; i < files.Length; i++)
            {
                perFileProgressIndicator.SetProgress("Packing File: " + files[i], i, files.Length - 1);
                try
                {

                    string zipLocation = files[i].Replace(folder + "\\", "");
                    ZipArchiveEntry entry = arch.CreateEntry(zipLocation);
                    Stream filestr = File.OpenRead(files[i]);
                    Stream zipstr = entry.Open();
                    filestr.CopyTo(zipstr);
                    filestr.Close();
                    zipstr.Close();
                }
                catch (Exception e)
                {
                }
            }

            arch.Dispose();

            //ZipFile.CreateFromDirectory(folder, targetFile);
            File.Delete(Path.Combine(folder, "Info.xml"));
        }



    }
}