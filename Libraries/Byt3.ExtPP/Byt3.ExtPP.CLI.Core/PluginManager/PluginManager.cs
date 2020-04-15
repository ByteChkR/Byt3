using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Byt3.ADL;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;

namespace Byt3.ExtPP.CLI.Core.PluginManager
{
    /// <summary>
    /// Manages often used plugins and makes them available by prefix when added to the watched dirs or files.
    /// </summary>
    public class PluginManager : ALoggable<PPLogType>
    {
        /// <summary>
        /// Directory of the ext_pp_cli.dll library
        /// </summary>
        private readonly string rootDir =
            Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        /// <summary>
        /// Config path for the cache
        /// </summary>
        private string ConfigPath => Path.Combine(rootDir, "plugin_manager.xml");
        /// <summary>
        /// Default folder for plugins.
        /// </summary>
        private string DefaultPluginFolder => Path.Combine(rootDir, "plugins");
        /// <summary>
        /// The Database of all plugins.
        /// </summary>
        private PluginManagerDatabase info;
        /// <summary>
        /// Serializer for loading/saving the cache to disk.
        /// </summary>
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(PluginManagerDatabase));


        /// <summary>
        /// Constructor
        /// </summary>
        public PluginManager()
        {
            if (!Directory.Exists(DefaultPluginFolder))
            {
                Directory.CreateDirectory(DefaultPluginFolder);
            }
            if (File.Exists(ConfigPath))
            {
                Initialize();
            }
            else
            {
                FirstStart();
            }
        }

        /// <summary>
        /// Initializes the PluginManager by loading the cache from file.
        /// </summary>
        private void Initialize()
        {
            FileStream fs = new FileStream(ConfigPath, FileMode.Open);
            info = (PluginManagerDatabase) Serializer.Deserialize(fs);
            fs.Close();

        }

        /// <summary>
        /// Lists all data that the cache contains.
        /// </summary>
        public void ListAllCachedData()
        {
            Logger.Log(PPLogType.Log, Verbosity.Level1, "Listing all Cached Data:");
            ListCachedFolders();
            ListCachedPlugins(false);
        }

        /// <summary>
        /// Lists the helpinfo of each plugin.(not used)
        /// </summary>
        public void ListCachedHelpInfo()
        {
            Logger.Log(PPLogType.Log, Verbosity.Level1, "Plugins [prefixes]:path");
            for (int i = 0; i < info.Cache.Count; i++)
            {
                Logger.Log(PPLogType.Log, Verbosity.Level1, $"\n\n{info.Cache[i].Name}" );
                for (int j = 0; j < info.Cache[i].Data.Length; j++)
                {
                    Logger.Log(PPLogType.Log, Verbosity.Level1, $"\t{info.Cache[i].Data[j].ToString()}");
                }
            }
        }

        /// <summary>
        /// Lists all cached plugins
        /// </summary>
        /// <param name="shortDesc">should skip the commands/help for commands</param>
        public void ListCachedPlugins(bool shortDesc)
        {
            for (int i = 0; i < info.Cache.Count; i++)
            {
                Logger.Log(PPLogType.Log, Verbosity.Level1, info.Cache[i].GetDescription(shortDesc));
            }
        }

        /// <summary>
        /// Returns all folders that are currently beeing watched.
        /// </summary>
        public void ListCachedFolders()
        {

            Logger.Log(PPLogType.Log, Verbosity.Level1, "Directories:");
            for (int i = 0; i < info.IncludedDirectories.Count; i++)
            {
                Logger.Log(PPLogType.Log, Verbosity.Level1, info.IncludedDirectories[i]);
            }
        }

        /// <summary>
        /// returns all the manually included files.
        /// </summary>
        public void ListManuallyCachedFiles()
        {

            Logger.Log(PPLogType.Log, Verbosity.Level1, "Manually Included Files:");
            for (int i = 0; i < info.IncludedFiles.Count; i++)
            {
                Logger.Log(PPLogType.Log, Verbosity.Level1, info.IncludedFiles[i]);
            }
        }

        /// <summary>
        /// Adds a folder to the PluginManager
        /// </summary>
        /// <param name="folder">The folder to be added</param>
        public void AddFolder(string folder)
        {
            if (Directory.Exists(folder))
            {

                Logger.Log(PPLogType.Log, Verbosity.Level1, $"Adding Directory: {folder}");
                info.IncludedDirectories.Add(Path.GetFullPath(folder));
            }
            else
            {
                Logger.Log(PPLogType.Error, Verbosity.Level1, $"Folder does not exist: {folder}");
            }

            Save();
        }

        public void AddAssemblies(Assembly[] assemblies, string fallbackPath, bool save)
        {
            Logger.Log(PPLogType.Log, $"Adding {assemblies.Length} Assemblies.");
            foreach (Assembly assembly in assemblies)
            {
                string assemblyPath = fallbackPath;
                if (!assembly.IsDynamic)
                {
                    assemblyPath = new Uri(assembly.CodeBase).AbsolutePath;
                }
                AddAssembly(assembly, assemblyPath, false);
            }

            if (save)
            {
                Save();
            }
        }

        public void AddAssembly(Assembly asm, string assemblyPath, bool save)
        {
            string fullpath = assemblyPath;
            if (info.Cache.Count(x => x.Path == fullpath) != 0)
            {
                return;
            }

            List<AbstractPlugin> plugins = FromAssembly(asm);

            List<PluginInformation> val = new List<PluginInformation>();

            if (plugins.Count != 0)
            {
                Logger.Log(PPLogType.Log, Verbosity.Level1, $"Adding {plugins.Count} plugins from {asm.FullName}");
            }

            for (int i = 0; i < plugins.Count; i++)
            {
                val.Add(new PluginInformation(plugins[i].Prefix, plugins[i].GetType().Name, fullpath,
                    plugins[i].Info.Select(x => x.Meta).ToArray()));
            }
            info.Cache.AddRange(val);

            if (save)
            {
                Save();
            }
        }

        /// <summary>
        /// Adds a single file to the PluginManager
        /// </summary>
        /// <param name="file">the file to be added</param>
        /// <param name="save">flag to optionally save after adding</param>
        private void AddFile(string file, bool save)
        {
            if (!File.Exists(file))
            {

                Logger.Log(PPLogType.Error, Verbosity.Level1, $"File does not exist: {file}");

            }
            else
            {
                string fullpath = Path.GetFullPath(file);
                if (info.Cache.Count(x => x.Path == fullpath) != 0)
                {
                    return;
                }

                info.IncludedFiles.Add(fullpath);

                List<AbstractPlugin> plugins = FromFile(fullpath);

                List<PluginInformation> val = new List<PluginInformation>();

                if (plugins.Count != 0)
                {
                    Logger.Log(PPLogType.Log, Verbosity.Level1, $"Adding {plugins.Count} plugins from {fullpath}");
                }

                for (int i = 0; i < plugins.Count; i++)
                {
                    val.Add(new PluginInformation(plugins[i].Prefix, plugins[i].GetType().Name, fullpath,
                        plugins[i].Info.Select(x => x.Meta).ToArray()));
                }
                info.Cache.AddRange(val);
            }

            if (save)
            {
                Save();
            }
        }

        /// <summary>
        /// Returns the plugin info by name. returns false if not containing
        /// </summary>
        /// <param name="pmd">The database that is cached</param>
        /// <param name="name">the name to be searched for</param>
        /// <param name="val">the out variable containing the plugin information(null if not found)</param>
        /// <returns>the success state of the operation</returns>
        public static bool TryGetPluginInfoByName(PluginManagerDatabase pmd, string name, out PluginInformation val)
        {
            for (int i = 0; i < pmd.Cache.Count; i++)
            {
                if (pmd.Cache[i].Name == name)
                {
                    val = pmd.Cache[i];
                    return true;
                }
            }


            val = new PluginInformation();
            return false;
        }

        /// <summary>
        /// Returns the plugin info by prefix. returns false if not containing
        /// </summary>
        /// <param name="pmd">The database that is cached</param>
        /// <param name="prefix">The prefix to be searched for</param>
        /// <param name="val">the out variable containing the plugin information(null if not found)</param>
        /// <returns>the success state of the operation</returns>
        public static bool TryGetPluginInfoByPrefix(PluginManagerDatabase pmd, string prefix, out PluginInformation val)
        {
            for (int i = 0; i < pmd.Cache.Count; i++)
            {
                if (pmd.Cache[i].Prefixes.Contains(prefix))
                {
                    val = pmd.Cache[i];
                    return true;
                }
            }


            val = new PluginInformation();
            return false;

        }

        /// <summary>
        /// Returns the plugin info by Path and Prefix. returns false if not containing
        /// </summary>
        /// <param name="pmd">The database that is cached</param>
        /// <param name="file">the file to be searched for</param>
        /// <param name="prefix">The prefix to be searched for</param>
        /// <param name="val">the out variable containing the plugin information(null if not found)</param>
        /// <returns>the success state of the operation</returns>
        public static bool TryGetPluginInfoByPathAndPrefix(PluginManagerDatabase pmd, string file, string prefix,
            out PluginInformation val)
        {

            for (int i = 0; i < pmd.Cache.Count; i++)
            {
                if (pmd.Cache[i].Path == file && pmd.Cache[i].Prefixes.Contains(prefix))
                {
                    val = pmd.Cache[i];
                    return true;
                }
            }


            val = new PluginInformation();
            return false;
        }

        /// <summary>
        /// Returns all cached information about this file.
        /// </summary>
        /// <param name="pathToLib">the file path to the compiled library</param>
        /// <returns>the plugin information of each plugin found</returns>
        private PluginInformation[] GetAllInLib(string pathToLib)
        {
            if (!File.Exists(pathToLib))
            {
                return new PluginInformation[0];
            }
            List<PluginInformation> ret = new List<PluginInformation>();
            foreach (PluginInformation inf in info.Cache)
            {
                if (inf.Path == pathToLib)
                {
                    ret.Add(inf);
                }
            }

            return ret.ToArray();
        }


        /// <summary>
        /// Displays help for a specific file/plugin
        /// </summary>
        /// <param name="path">the path of the library containing the plugins</param>
        /// <param name="names">the names of the plugins to display help for</param>
        /// <param name="shortDesc">flag to optionally display a short description</param>
        /// <returns>the success state of the operation</returns>
        public bool DisplayHelp(string path, string[] names, bool shortDesc)
        {
            if (names == null)
            {
                PluginInformation[] inf = GetAllInLib(path);
                if (inf.Length == 0)
                {
                    return false;
                }
                foreach (PluginInformation name in inf)
                {

                    Logger.Log(PPLogType.Log, Verbosity.Level1, $"\n{name.GetDescription(shortDesc)}");

                }

                return true;

            }

            foreach (string name in names)
            {
                if (TryGetPluginInfoByPathAndPrefix(info, path, name, out PluginInformation val))
                {
                    Logger.Log(PPLogType.Log, Verbosity.Level1, $"\n{val.GetDescription(shortDesc)}");
                }
            }

            return true;
        }


        /// <summary>
        /// Adds a file to the plugin manager
        /// </summary>
        /// <param name="file">The file to be added</param>
        public void AddFile(string file)
        {
            AddFile(file, true);

        }


        /// <summary>
        /// Refreshes the cache.
        /// </summary>
        public void Refresh()
        {

            info.Cache.Clear();


            Assembly asm = Assembly.GetEntryAssembly(); //Add own Modules
            AddAssemblies(AppDomain.CurrentDomain.GetAssemblies(), new Uri(asm.CodeBase).AbsolutePath, false);


            for (int i = info.IncludedDirectories.Count - 1; i >= 0; i--)
            {
                if (!Directory.Exists(info.IncludedDirectories[i]))
                {
                    Logger.Log(PPLogType.Error, Verbosity.Level1, $"Folder does not exist: {info.IncludedDirectories[i]} Removing..");
                    info.IncludedDirectories.RemoveAt(i);

                }
                else
                {
                    Logger.Log(PPLogType.Log, Verbosity.Level1, $"Discovering Files in {info.IncludedDirectories[i]}");
                    string[] files = Directory.GetFiles(info.IncludedDirectories[i], "*.dll");
                    foreach (string file in files)
                    {
                        List<AbstractPlugin> plugins = FromFile(file);
                        List<string> prefixes = new List<string>();
                        plugins.ForEach(x => prefixes.AddRange(x.Prefix));
                        Logger.Log(PPLogType.Log, Verbosity.Level1, $"Adding {plugins.Count} plugins from {file}" );
                        for (int j = 0; j < plugins.Count; j++)
                        {
                            info.Cache.Add(new PluginInformation(plugins[i].Prefix, plugins[i].GetType().Name, file,
                                plugins[i].Info.Select(x => x.Meta).ToArray()));
                        }
                    }
                }
            }

            List<string> manuallyIncluded = new List<string>(info.IncludedFiles);
            info.IncludedFiles.Clear();


            foreach (string inc in manuallyIncluded)
            {
                AddFile(inc, false);
            }

            Save();
        }

        public List<AbstractPlugin> FromAssemblies(Assembly[] assemblies)
        {
            List<AbstractPlugin> plugins = new List<AbstractPlugin>();
            foreach (Assembly assembly in assemblies)
            {
                plugins.AddRange(FromAssembly(assembly));
            }

            return plugins;
        }

        public List<AbstractPlugin> FromAssembly(Assembly asm)
        {
            List<AbstractPlugin> ret = new List<AbstractPlugin>();
            Type[] types = asm.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(AbstractPlugin)))
                {
                    ret.Add((AbstractPlugin) Activator.CreateInstance(type));
                }
            }

            return ret;
        }

        /// <summary>
        /// returns all plugins contained in the file.
        /// </summary>
        /// <param name="path">The path to a compiled assembly containing Absract Plugins</param>
        /// <returns>A list of all abstract plugins in the file</returns>
        public List<AbstractPlugin> FromFile(string path)
        {
            List<AbstractPlugin> ret = new List<AbstractPlugin>();
            try
            {
                Assembly asm = Assembly.LoadFile(Path.GetFullPath(path));
                ret.AddRange(FromAssembly(asm));
            }
            catch (Exception)
            {
                Logger.Log(PPLogType.Error, Verbosity.Level1, $"Could not load file: {path}");
                // ignored
            }

            return ret;
        }


        /// <summary>
        /// Saves the cache to disk.
        /// </summary>
        private void Save()
        {
            if (File.Exists(ConfigPath))
            {
                File.Delete(ConfigPath);
            }
            FileStream fs = new FileStream(ConfigPath, FileMode.Create);
            Serializer.Serialize(fs, info);
            fs.Close();
        }

        /// <summary>
        /// Gets executed when its the first start.
        /// Will set up the cache and will perform a refresh.
        /// </summary>
        private void FirstStart()
        {

            Logger.Log(PPLogType.Log, Verbosity.Level1, "First start of Plugin Manager. Setting up...");
            FileStream fs = new FileStream(ConfigPath, FileMode.Create);
            info = new PluginManagerDatabase
            {
                IncludedFiles = new List<string>(),
                Cache = new List<PluginInformation>(),
                IncludedDirectories = new List<string>
                {
                    DefaultPluginFolder
                }
            };
            if (!Directory.Exists(DefaultPluginFolder))
            {
                Directory.CreateDirectory(DefaultPluginFolder);
            }
            Serializer.Serialize(fs, info);
            fs.Close();
            Refresh();

        }


        /// <summary>
        /// Returns the plugin path by name. returns false if not containing
        /// </summary>
        /// <param name="name">name to be searched for</param>
        /// <param name="path">the path of the assembly containing the specified plugin</param>
        /// <returns>the success state of the operation</returns>
        public bool TryGetPathByName(string name, out string path)
        {
            if (TryGetPluginInfoByName(info, name, out PluginInformation pli))
            {
                path = pli.Path;
                return true;
            }

            path = name;
            return false;
        }

        /// <summary>
        /// Returns the plugin path by prefix. returns false if not containing
        /// </summary>
        /// <param name="prefix">prefix to be searched for</param>
        /// <param name="path">the path of the assembly containing the specified plugin</param>
        /// <returns>the success state of the operation</returns>
        public bool TryGetPathByPrefix(string prefix, out string path)
        {
            if (TryGetPluginInfoByPrefix(info, prefix, out PluginInformation pli))
            {
                path = pli.Path;
                return true;
            }

            path = prefix;
            return false;
        }
    }
}