﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Crash;
using Byt3.ADL.Streams;
using Byt3.ExtPP;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;
using Byt3.Utilities.FastString;
using Byt3Console.ExtPP.PluginManager;
using Utils = Byt3.ExtPP.Base.Utils;

namespace Byt3Console.ExtPP
{
    /// <summary>
    /// Command Line Interface of the EXT_PP project.
    /// </summary>
    public class CLI : ALoggable<LogType>
    {
        /// <summary>
        /// A delegate used to handle all commands in the Command Line Interface
        /// </summary>
        /// <returns></returns>
        private delegate bool CommandHandler();

        /// <summary>
        /// Default helptext to show.
        /// </summary>
        public static string HelpText { get; } = "To list all available commands type: ext_pp_cli --help or -h";

        /// <summary>
        /// Simple header to assert dominance
        /// </summary>
        public static string CliHeader { get; } = "\n\next_pp_cli version: " +
                                                  Assembly.GetAssembly(typeof(CLI)).GetName().Version +
                                                  "\nCopyright by Tim Akermann\nGithub: https://github.com/ByteChkR/ext-pp\n\n";

        /// <summary>
        /// A version string.
        /// </summary>
        public static string Version { get; } = "\next_pp_cli: " + Assembly.GetAssembly(typeof(CLI)).GetName().Version +
                                                "\next_pp_base: " +
                                                Assembly.GetAssembly(typeof(Utils)).GetName().Version +
                                                "\next_pp: " + Assembly.GetAssembly(typeof(Definitions)).GetName()
                                                    .Version;


        /// <summary>
        /// Instance of the plugin manager.
        /// </summary>
        private readonly PluginManager.PluginManager pluginManager;


        #region Commands

        /// <summary>
        /// Sets the order in which the commands get checked/applied
        /// </summary>
        private List<CommandHandler> CommandApplyOrder => new List<CommandHandler>
        {
            LogToFileCommandHandler,
            ReadmeCommandHandler,
            ShowVersionCommandHandler,
            ListAllPluginsCommandHandler,
            AddPluginsCommandHandler,
            ListPluginDirsCommandHandler,
            ListPluginIncsCommandHandler,
            ListPluginManIncsCommandHandler,
            RefreshPluginsCommandHandler,
            Help_Command,
            HelpAll_Command,
            DefineParamsCommandHandler,
            ChainParamsCommandHanlder,
            InOutCommandHandler
        };


        #region CommandInfo

        /// <summary>
        /// List of Command infos that are directly used by the CLI
        /// They have no prefix
        /// </summary>
        private static List<CommandInfo> Info => new List<CommandInfo>
        {
            new CommandInfo("input", "i", PropertyHelper<CLI>.GetPropertyInfo(x => x.Input),
                "--input filepath,filepath filepath,filepath\r\n\t, separated = the same compilation\r\n\t[space] separated means gets queued after the compilation of the first one"),
            new CommandInfo("output", "o", PropertyHelper<CLI>.GetPropertyInfo(x => x.Output),
                "--output filepath filepath filepath\r\n\t[space] separated list of output files."),
            new CommandInfo("defines", "d", PropertyHelper<CLI>.GetPropertyInfo(x => x.DefinesParams),
                "--defines <vars>\r\n\t, [space] separated list of predefines values"),
            new CommandInfo("chain", "c", PropertyHelper<CLI>.GetPropertyInfo(x => x.ChainParams),
                "--chain [filepath]\r\n\t, separated list of plugins\r\n\t\t[filepath]:pluginname => loads a plugin by assembly name\r\n\t\t[filepath]:prefix => loads a plugin with prefix\r\n\t\t[filepath]:(collection) => loads a list(chain) of plugins from an IChainCollection with the specified name\r\n\t\tthe plugins in the /plugin folder can be directly accessed by using the prefix instead of the lines above"),
            new CommandInfo("log-to-file", "l2f", PropertyHelper<CLI>.GetPropertyInfo(x => x.LogToFileParams),
                "--log-to-file <file> <settings>\r\n\tCreates a log file with the settings\r\n\t\t<mask>:<timestamp>\r\n\t\tdefault: all:true"),
            new CommandInfo("write-to-console", "w2c", PropertyHelper<CLI>.GetPropertyInfo(x => x.OutputToConsole),
                "--write2console [bool]\r\n\tWrites the result into the cout stream\r\n\tSets the verbosity to silent if not specified otherwise"),
            new CommandInfo("verbosity", "v", PropertyHelper<CLI>.GetPropertyInfo(x => x.DebugLvl),
                "--verbosity <int>\r\n\tSets the debug output granularity"),
            new CommandInfo("version", "vv", PropertyHelper<CLI>.GetPropertyInfo(x => x.ShowVersion),
                "--version\r\n\tdisplays the current version"),
            new CommandInfo("no-chain-collection", "nc", PropertyHelper<CLI>.GetPropertyInfo(x => x.NoCollections),
                "The CLI will not search for a ChainCollection in the specified assembly"),
            new CommandInfo("help", "h", PropertyHelper<CLI>.GetPropertyInfo(x => x.HelpParams),
                "	\t--help <chainstr>\r\n\t\tlists the commands of the CLI or with supplied chain, it will display the help info of each plugin."),
            new CommandInfo("help-all", "hh", PropertyHelper<CLI>.GetPropertyInfo(x => x.HelpAllParams),
                "	\t--help-all <chainstr>\r\n\t\tlists the commands of the CLI or with supplied chain, it will display the help info of each plugin."),
            new CommandInfo("pm-refresh", "pm-r", PropertyHelper<CLI>.GetPropertyInfo(x => x.PluginRefresh),
                "--pm-refresh\r\n\t\tRefreshes the Plugin Manager."),
            new CommandInfo("pm-add", "pm-a", PropertyHelper<CLI>.GetPropertyInfo(x => x.PluginAdd),
                "--pm-add <folder>\r\n\t\tAdds a folder with plugins to the Plugin Manager. All pluins in that folder can be referenced by their prefixes when specifies in --chain or --help"),
            new CommandInfo("pm-list-dir", "pm-ld", PropertyHelper<CLI>.GetPropertyInfo(x => x.PluginListDirs),
                "--pm-list-dir\r\n\t\tLists all Included dictionaries in Plugin Manager"),
            new CommandInfo("pm-list-file", "pm-lf", PropertyHelper<CLI>.GetPropertyInfo(x => x.PluginListIncs),
                "--pm-list-file\r\n\t\tLists all Included and Cached Files in Plugin Manager"),
            new CommandInfo("pm-list-manual-files", "pm-lmf",
                PropertyHelper<CLI>.GetPropertyInfo(x => x.PluginListManIncs),
                "--pm-list-manual-files\r\n\t\tLists all Manually Included and Cached Files in Plugin Manager"),
            new CommandInfo("pm-list-all", "pm-la",
                PropertyHelper<CLI>.GetPropertyInfo(x => x.PluginListAllCommandHandler),
                "--pm-list-all\r\n\t\tLists all Cached data."),
            new CommandInfo("throw-on-warning", "tow", PropertyHelper<CLI>.GetPropertyInfo(x => x.ThrowOnWarning),
                "--throw-on-warning <true|false>\r\n\t\tCrashes the programm if any warnings are occuring."),
            new CommandInfo("throw-on-error", "toe", PropertyHelper<CLI>.GetPropertyInfo(x => x.ThrowOnError),
                "--throw-on-error <true|false>\r\n\t\tCrashes the programm if any errors are occuring."),
            new CommandInfo("generate-readme", "gen-r", PropertyHelper<CLI>.GetPropertyInfo(x => x.ReadmeArgs),
                "--generate-readme <self|pathToPluginLibrary> <outputfile>\r\n\t\tGenerates a readme file in markdown syntax."),
        };

        #endregion

        #region CommandFields

        /// <summary>
        /// Contains the Parameters for the --throw-on-warning command.
        /// </summary>
        public bool ThrowOnWarning { get; set; }

        /// <summary>
        /// Contains the Parameters for the --throw-on-error command.
        /// </summary>
        public bool ThrowOnError { get; set; }

        /// <summary>
        /// Contains the Parameters for the -l2f and --logToFile commands.
        /// </summary>
        public string[] LogToFileParams { get; set; }

        /// <summary>
        /// A flag that is used to determine if the log2file flag was set.
        /// </summary>
        private bool LogToFile => LogToFileParams != null && LogToFileParams.Length != 0;

        /// <summary>
        /// Flag to output the result to the console.
        /// </summary>
        public bool OutputToConsole { get; set; }

        /// <summary>
        /// The input files
        /// </summary>
        public string[] Input { get; set; } = new string[0];

        /// <summary>
        /// The input files
        /// </summary>
        public string[] ReadmeArgs { get; set; } = new string[0];

        /// <summary>
        /// The output files.
        /// </summary>
        public string[] Output { get; set; } = new string[0];

        /// <summary>
        /// Predefined definitions from --defines and -defs
        /// </summary>
        public string[] DefinesParams { get; set; }

        /// <summary>
        /// Forces the cli to ignore collections.
        /// </summary>
        public bool NoCollections { get; set; }

        /// <summary>
        /// Contains the parameters for the plugin chain
        /// </summary>
        public string[] ChainParams { get; set; }

        /// <summary>
        /// Contains the parameters for the help parameter
        /// </summary>
        public string[] HelpParams { get; set; }

        /// <summary>
        /// Contains the parameters for the help all parameter
        /// </summary>
        public string[] HelpAllParams { get; set; }

        /// <summary>
        /// Debug level of the process.
        /// </summary>
        public Verbosity DebugLvl { get; set; } = Verbosity.Level1;

        /// <summary>
        /// Show the version at the start and exit.
        /// </summary>
        public bool ShowVersion { get; set; }

        /// <summary>
        /// parameter for the --pm-add and -pm-a commands.
        /// </summary>
        public string[] PluginAdd { get; set; }

        /// <summary>
        /// Flag if the settings contain the -pm-r/--pm-refresh command
        /// </summary>
        public bool PluginRefresh { get; set; }

        /// <summary>
        /// Flag if the settings contain the -pm-ld/--pm-list-dirr command
        /// </summary>
        public bool PluginListDirs { get; set; }

        /// <summary>
        /// Flag if the settings contain the -pm-lf/--pm-list-file command
        /// </summary>
        public bool PluginListIncs { get; set; }

        /// <summary>
        /// Flag if the settings contain the -pm-lmf/--pm-list-manual-files command
        /// </summary>
        public bool PluginListManIncs { get; set; }

        /// <summary>
        /// Flag if the settings contain the -pm-a/--pm-all command
        /// </summary>
        public bool PluginListAllCommandHandler { get; set; }

        /// <summary>
        /// Definitions.
        /// </summary>
        private Definitions defs;

        /// <summary>
        /// the chain used for it.
        /// </summary>
        private List<AbstractPlugin> chain;

        #endregion

        #region Help

        /// <summary>
        /// The command handler that is used for the command --help
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool Help_Command()
        {
            return Help_Command_Unfied(HelpParams, false);
        }

        /// <summary>
        /// The command handler that is used for the command --help-all
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool HelpAll_Command()
        {
            return Help_Command_Unfied(HelpAllParams, true);
        }

        /// <summary>
        /// A underlying function to handle the --help and the --help-all command
        /// </summary>
        /// <param name="helpParams">Either HelpParams or HelpAllParams</param>
        /// <param name="extendedDescription">Indicator if the extended description should be used.</param>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool Help_Command_Unfied(string[] helpParams, bool extendedDescription)
        {
            if (helpParams == null)
            {
                return false;
            }

            if (helpParams.Length == 0)
            {
                Logger.Log(LogType.Log, $"\n{Info.ListAllCommands(new[] {""}).Unpack("\n")}", 0);
                return true;
            }

            foreach (string file in helpParams)
            {
                if (file != "self")
                {
                    ParseChainSyntax(file, out string path, out string[] names);
                    if (pluginManager.DisplayHelp(path, names, !extendedDescription))
                    {
                        continue;
                    }

                    List<AbstractPlugin> plugins = CreatePluginChain(new[] {file}, true).ToList();
                    Logger.Log(LogType.Log, "Listing Plugins: ", 0);
                    foreach (AbstractPlugin plugin in plugins)
                    {
                        Logger.Log(LogType.Log, $"\n{PluginExtensions.ListInfo(plugin, true).Unpack("\n")}", 0);
                    }
                }
                else
                {
                    Logger.Log(LogType.Log, $"\n{Info.ListAllCommands(new[] {""}).Unpack("\n")}", 0);
                }
            }

            return true;
        }

        #endregion

        #region Input/Output

        /// <summary>
        /// The command handler that is used for the commands --input --output
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool InOutCommandHandler()
        {
            //No Other code since the input and output arrays are populated with reflection
            //Error Checking
            if (Output.Length == 0 && !OutputToConsole || Input.Length == 0)
            {
                Logger.Log(LogType.Log, HelpText, 1);
                Logger.Log(LogType.Error, "Not enough arguments specified. Aborting..", 1);
                return true;
            }

            if (Input.Length > Output.Length)
            {
                Logger.Log(LogType.Error, "Not enough outputs specified. Aborting..", 1);
                return true;
            }

            return false;
        }

        #endregion

        #region Readme

        /// <summary>
        /// The command handler that is used for the command --generate-readme
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool ReadmeCommandHandler()
        {
            if (ReadmeArgs.Length == 2)
            {
                if (ReadmeArgs[0] == "self")
                {
                    Logger.Log(LogType.Log, "Generating Readme for self.", 1);
                    List<string> ret = PluginExtensions.ToMarkdown(Info).ToList();
                    Logger.Log(LogType.Log, $"Writing Readme to file: {ReadmeArgs[1]}", 1);
                    File.WriteAllLines(ReadmeArgs[1], ret.ToArray());
                    return true;
                }

                Logger.Log(LogType.Log, $"Generating Readme for file: {ReadmeArgs[0]}", 1);
                PluginManager.PluginManager pm = new PluginManager.PluginManager();
                List<string> ht = GenerateReadme(pm.FromFile(ReadmeArgs[0]));

                Logger.Log(LogType.Log, $"Writing Readme to file: {ReadmeArgs[1]}", 1);
                File.WriteAllLines(ReadmeArgs[1], ht.ToArray());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Appends the Plugin Help Texts to be used in the ReadmeCommandHandler
        /// </summary>
        /// <param name="plugins">The plugins used to generate the readme</param>
        /// <returns>The readme</returns>
        private List<string> GenerateReadme(List<AbstractPlugin> plugins)
        {
            List<string> ret = new List<string>();

            foreach (AbstractPlugin abstractPlugin in plugins)
            {
                Logger.Log(LogType.Log, $"Generating Readme for plugin: {abstractPlugin.GetType().Name}", 1);
                ret.AddRange(PluginExtensions.ToMarkdown(abstractPlugin));
            }

            return ret;
        }

        #endregion

        #region ShowVersion

        /// <summary>
        /// The command handler that is used for the command --version
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool ShowVersionCommandHandler()
        {
            if (ShowVersion)
            {
                Logger.Log(LogType.Log, Version, 1);
                return true;
            }

            return false;
        }

        #endregion

        #region PluginManager

        #region PluginList

        /// <summary>
        /// The command handler that is used for the command --pm-list-all
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool ListAllPluginsCommandHandler()
        {
            if (PluginListAllCommandHandler)
            {
                pluginManager.ListAllCachedData();
                return true;
            }

            return false;
        }

        /// <summary>
        /// The command handler that is used for the command --pm-list-dir
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool ListPluginDirsCommandHandler()
        {
            if (PluginListDirs)
            {
                pluginManager.ListCachedFolders();
                if (!PluginRefresh && !PluginListIncs && !PluginListManIncs)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The command handler that is used for the command --pm-list-file
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool ListPluginIncsCommandHandler()
        {
            if (PluginListIncs)
            {
                pluginManager.ListCachedPlugins(false);
                if (!PluginRefresh && !PluginListManIncs)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The command handler that is used for the command --pm-list-manual-files
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool ListPluginManIncsCommandHandler()
        {
            if (PluginListManIncs)
            {
                pluginManager.ListManuallyCachedFiles();
                if (!PluginRefresh)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region PluginAddRemove

        /// <summary>
        /// The command handler that is used for the command --pm-refresh
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool RefreshPluginsCommandHandler()
        {
            if (PluginRefresh)
            {
                pluginManager.Refresh();
                return true;
            }

            return false;
        }

        /// <summary>
        /// The command handler that is used for the command --pm-add
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool AddPluginsCommandHandler()
        {
            if (PluginAdd != null && PluginAdd.Length != 0)
            {
                foreach (string s in PluginAdd)
                {
                    Logger.Log(LogType.Log, $"Adding: {s}", 1);
                    FileAttributes attr = File.GetAttributes(s);
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        pluginManager.AddFolder(s);
                    }
                    else
                    {
                        pluginManager.AddFile(s);
                    }
                }

                if (!PluginRefresh && !PluginListDirs && !PluginListIncs && !PluginListManIncs)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #endregion

        #region LogToFile

        /// <summary>
        /// The command handler that is used for the command --log-to-file
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool LogToFileCommandHandler()
        {
            if (LogToFile)
            {
                string[] arguments = LogToFileParams.Length > 1
                    ? LogToFileParams.SubArray(1, LogToFileParams.Length - 1).ToArray()
                    : new string[0];
                KeyValuePair<int, bool> ts = ParseLogParams(arguments.Length != 0 ? arguments[0] : "");

                AddLogOutput(LogToFileParams[0], ts.Value);
            }

            return false;
        }

        /// <summary>
        /// Adds a log output to the ADL system that is writing to a file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="mask"></param>
        /// <param name="timestamp"></param>
        private static void AddLogOutput(string file, bool timestamp)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            LogTextStream lll = new LogTextStream(File.OpenWrite(file), timestamp);
            Debug.AddOutputStream(lll);
        }

        /// <summary>
        /// Implements the log params syntax
        /// int:bool
        /// default: -1:true
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static KeyValuePair<int, bool> ParseLogParams(string input)
        {
            int mask = -1;
            bool timestamp = true;
            string[] vars = input.Split(new[] {':'});
            if (vars.Length > 0)
            {
                if (vars[0] != "all")
                {
                    mask = (int) Utils.Parse(typeof(LogType), vars[0], -1);
                }

                if (vars.Length > 1)
                {
                    bool.TryParse(vars[1], out timestamp);
                }
            }

            return new KeyValuePair<int, bool>(mask, timestamp);
        }

        #endregion

        #region Defines

        /// <summary>
        /// The command handler that is used for the command --defines
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool DefineParamsCommandHandler()
        {
            if (DefinesParams == null)
            {
                defs = new Definitions();
            }
            else
            {
                defs = new Definitions(DefinesParams.Select(x => new KeyValuePair<string, bool>(x, true))
                    .ToDictionary(x => x.Key, x => x.Value));
            }

            return false;
        }

        #endregion

        #region Chain

        /// <summary>
        /// The command handler that is used for the command --chain
        /// </summary>
        /// <returns>Returns true if the CLI should exit after this command</returns>
        private bool ChainParamsCommandHanlder()
        {
            if (ChainParams != null)
            {
                chain = CreatePluginChain(ChainParams, NoCollections).ToList();
                Logger.Log(LogType.Log, $"{chain.Count} Plugins Loaded..", 2);
            }
            else
            {
                Logger.Log(LogType.Error, "Not plugin chain specified. 0 Plugins Loaded..", 1);
                chain = new List<AbstractPlugin>();
            }

            return false;
        }

        /// <summary>
        /// Implements the Chain Syntax
        /// path:plugin:plugin:plugin
        /// path:collection:plugin
        /// prefix:prefix:prefix
        /// prefix prefix prefix
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="path"></param>
        /// <param name="names"></param>
        private void ParseChainSyntax(string arg, out string path, out string[] names)
        {
            if (arg.Contains(':'))
            {
                List<string> tmp = arg.Split(':').ToList();
                names = tmp.SubArray(1, tmp.Count - 1).ToArray();
                path = tmp[0];
            }
            else
            {
                path = arg; //Set the path
                names = new[] {path};
                if (!pluginManager.TryGetPathByPrefix(arg, out path) && !pluginManager.TryGetPathByName(arg, out path))
                {
                    names = null; //Will change path if it matches prefix
                }
            }
        }

        /// <summary>
        /// Creates a List of Plugins based on a chain collection in the specified assembly
        /// </summary>
        /// <param name="asm">The containing assembly</param>
        /// <param name="name">The name of the collection(or "*" for any collection)</param>
        /// <returns></returns>
        private List<AbstractPlugin> CreateChainCollection(Assembly asm, string name)
        {
            if (TryCreateChainCollection(asm, name, out IChainCollection collection))
            {
                List<AbstractPlugin> r = collection.Chain
                    .Select(x => (AbstractPlugin) Activator.CreateInstance(x)).ToList();
                Logger.Log(LogType.Log,
                    $"Creating Chain Collection with Plugins: {r.Select(x => x.GetType().Name).Unpack(", ")}", 2);
                return r;
            }
            else
            {
                Logger.Log(LogType.Log, "Could not find a Chain collection in the specified file.", 2);
            }

            return new List<AbstractPlugin>();
        }

        /// <summary>
        /// Tries to create a chain collection from a name
        /// </summary>
        /// <param name="asm">The containing assembly</param>
        /// <param name="name">The name of the collection(or "*" for any collection)</param>
        /// <param name="collection">The out parameter containing the created collection</param>
        /// <returns>The success state</returns>
        private static bool TryCreateChainCollection(Assembly asm, string name, out IChainCollection collection)
        {
            Type[] types = asm.GetTypes();
            if (name == "*")
            {
                Type tt = types.FirstOrDefault(x => x.GetInterfaces().Contains(typeof(IChainCollection)));
                if (tt != null)
                {
                    collection = (IChainCollection) Activator.CreateInstance(tt);
                }

                collection = null;
            }
            else
            {
                collection = types.Where(x => x.GetInterfaces().Contains(typeof(IChainCollection)))
                    .Select(x => (IChainCollection) Activator.CreateInstance(x)).FirstOrDefault(x => x.Name == name);
            }

            return collection != null;
        }

        /// <summary>
        /// Creates a List of Plugins based on a manually created chain.
        /// </summary>
        /// <param name="names">The names of the plugins that are specified in the chain</param>
        /// <param name="asm">The containing assembly</param>
        /// <returns>The chain from this Assembly</returns>
        private List<AbstractPlugin> ProcessChainCollection(string[] names, Assembly asm)
        {
            List<AbstractPlugin> ret = new List<AbstractPlugin>();

            if (names == null)
            {
                ret.AddRange(CreateChainCollection(asm, "*"));
            }
            else
            {
                names[0] = names[0].Trim('(', ')');
                Logger.Log(LogType.Log, $"Searching Chain Collection: {names[0]}", 2);

                ret.AddRange(CreateChainCollection(asm, names[0]));
            }

            return ret;
        }

        /// <summary>
        /// Creates a List of plugins from a manually created name chain.
        /// </summary>
        /// <param name="names">The names of the plugins that are specified in the chain</param>
        /// <param name="path">File Path of the compiled assembly</param>
        /// <returns></returns>
        private List<AbstractPlugin> ProcessPluginChain(string[] names, string path)
        {
            List<AbstractPlugin> ret = new List<AbstractPlugin>();
            Logger.Log(LogType.Log, $"Loading {(names == null ? "all plugins" : names.Unpack(", "))} in file {path}",
                4);

            if (names == null)
            {
                ret.AddRange(pluginManager.FromFile(path));
            }
            else
            {
                List<AbstractPlugin> plugins = pluginManager.FromFile(path);
                for (int i = 0; i < names.Length; i++)
                {
                    for (int j = 0; j < plugins.Count; j++)
                    {
                        if (plugins[j].Prefix.Contains(names[i]))
                        {
                            Logger.Log(LogType.Log, $"Creating instance of: {plugins[j].GetType().Name}", 5);
                            ret.Add(plugins[j]);
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Returns all Plugins from the specified compiled assembly.
        /// </summary>
        /// <param name="path">Path to the compiled assembly</param>
        /// <param name="names">The names of the plugins or chains</param>
        /// <param name="noCollection">opt out flag to ignore collections</param>
        /// <returns></returns>
        private List<AbstractPlugin> GetPluginsFromPath(string path, string[] names, bool noCollection)
        {
            List<AbstractPlugin> ret = new List<AbstractPlugin>();

            if (File.Exists(path))
            {
                bool isCollection = names != null && names.Length == 1 && names[0].StartsWith("(") &&
                                    names[0].EndsWith(")");
                if (names == null && !noCollection || isCollection)
                {
                    Assembly asm = Assembly.LoadFile(Path.GetFullPath(path));
                    ret.AddRange(ProcessChainCollection(names, asm));
                }
                else
                {
                    ret.AddRange(ProcessPluginChain(names, path));
                }
            }
            else
            {
                Logger.Log(LogType.Error, $"Could not load file: {path}", 1);
            }

            return ret;
        }

        /// <summary>
        /// Creates the Plugin chain based on a argument in ChainSyntax.
        /// </summary>
        /// <param name="arg">The chain</param>
        /// <param name="noCollection"></param>
        /// <returns></returns>
        private IEnumerable<AbstractPlugin> CreatePluginChain(string[] arg, bool noCollection)
        {
            Logger.Log(LogType.Log, "Creating Plugin Chain...", 3);
            List<AbstractPlugin> ret = new List<AbstractPlugin>();

            string[] names = null;
            string path = "";
            foreach (string plugin in arg)
            {
                ParseChainSyntax(plugin, out path, out names);

                ret.AddRange(GetPluginsFromPath(path, names, noCollection));
            }

            return ret;
        }

        #endregion

        #endregion

        #region CLIArgumentProcessing

        /// <summary>
        /// Implements the @[filename] syntax
        /// All paths preceeded by an @ will be opened and their content pasted as argument.
        /// </summary>
        /// <param name="args">The raw arguments from the Command Line</param>
        /// <returns>A complete list of arguments with all @ usings beeing resolved.</returns>
        private List<string> ComputeFileReferences(List<string> args)
        {
            List<string> ret = args;
            for (int i = 0; i < args.Count; i++)
            {
                if (args[i].StartsWith("@"))
                {
                    string path = args[i].TrimStart('@');
                    args.RemoveAt(i);
                    if (File.Exists(path))
                    {
                        args.InsertRange(i, File.ReadAllLines(path).Unpack(" ").Pack(" "));
                    }
                    else
                    {
                        Logger.Log(LogType.Error, "Can not find: " + path, 1);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Turns the args into a dictionary that contains keys and n string values.
        /// </summary>
        /// <param name="args">The arguments of the CLI</param>
        /// <returns>The dictionary containing all the arguments</returns>
        public static Dictionary<string, string[]> AnalyzeArgs(string[] args)
        {
            Dictionary<string, string[]> ret = new Dictionary<string, string[]>();
            if (args.Length == 0)
            {
                return ret;
            }

            int cmdIdx = FindNextCommand(args, -1);
            if (cmdIdx == args.Length)
            {
                return ret;
            }

            do
            {
                int tmpidx = FindNextCommand(args, cmdIdx);
                ret.Add(args[cmdIdx], args.SubArray(cmdIdx + 1, tmpidx - cmdIdx - 1).ToArray());
            } while ((cmdIdx = FindNextCommand(args, cmdIdx)) != args.Length);

            return ret;
        }

        /// <summary>
        /// Returns the index of the next occurrence of "-..."
        /// </summary>
        /// <param name="args">the arguments to search</param>
        /// <param name="start">the current index</param>
        /// <returns>the index of the next command.</returns>
        private static int FindNextCommand(string[] args, int start)
        {
            for (int i = start + 1; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    return i;
                }
            }

            return args.Length;
        }

        #endregion

        /// <summary>
        /// Processes all the queued files with the PreProcessor
        /// </summary>
        /// <param name="pp">The PreProcessor</param>
        /// <param name="settings">The settings used in the computation.</param>
        private void Process(PreProcessor pp, Settings settings)
        {
            //Run/Execute PreProcessor
            for (int index = 0; index < Input.Length; index++)
            {
                string input = Input[index];
                string[] src =
                    pp.Run(input.Split(',').Select(x => new FilePathContent(x, x)).OfType<IFileContent>().ToArray(),
                        settings, defs);

                if (OutputToConsole)
                {
                    if (Output != null && Output.Length > index)
                    {
                        string outp = Path.GetFullPath(Output[index]);
                        string sr = src.Unpack("\n");
                        File.WriteAllText(outp, sr);
                    }
                }
                else
                {
                    string outp = Path.GetFullPath(Output[index]);
                    string sr = src.Unpack("\n");
                    File.WriteAllText(outp, sr);
                }
            }
        }


        /// <summary>
        /// Applies/Brings the configuration of the CLI up and running so it can start the PreProcessing.
        /// </summary>
        public bool Apply()
        {
            List<CommandHandler> loadOrder = CommandApplyOrder;

            foreach (CommandHandler commandHandler in loadOrder)
            {
                if (commandHandler()) //Command wants us to exit the program(work was finished)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Constructor that does the parameter analysis.
        /// </summary>
        /// <param name="args"></param>
        public CLI(string[] args) : base(ExtPPDebugConfig.Settings)
        {
            pluginManager = new PluginManager.PluginManager();

            DoExecution(args);
        }

        private void DoExecution(string[] args)
        {
            List<string> arf = ComputeFileReferences(args.ToList());

            string[] arguments = arf.ToArray();

            Settings settings = new Settings(AnalyzeArgs(arguments));

            settings.ApplySettings(Info, this);


            if (Apply())
            {
                PreProcessor pp = new PreProcessor();
                pp.SetFileProcessingChain(chain);
                Process(pp, settings);
            }
        }

        private static string[][] SplitExecutions(string[] args)
        {
            string argstr = args.Unpack(" ");
            List<string[]> ret = new List<string[]>();
            string[] execs = argstr.Split(new[] {"__"}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < execs.Length; i++)
            {
                ret.Add(execs[i].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
            }

            return ret.ToArray();
        }

        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args"></param>
        public static void StartConsole(string[] args)
        {
            Debug.DefaultInitialization();
            CrashHandler.Initialize();


            float start = Timer.MS; // Load assembly
            Console.WriteLine(CliHeader, start);


            if (args.Length != 0)
            {
                string[][] execs = SplitExecutions(args);
                foreach (string[] execution in execs)
                {
                    new CLI(execution);
                }
            }
            else
            {
                //Not empty... Just special.
#if DEBUG

                CLI c;
                string[] arf;
                bool exit = false;
                do
                {
                    arf = Console.ReadLine().Pack(" ").ToArray();
                    if (arf.Contains("exit"))
                    {
                        exit = true;
                    }

                    c = new CLI(arf);
                    Debug.RemoveAllOutputStreams();
                    c = null;
                } while (!exit);
#endif
            }
        }
    }
}