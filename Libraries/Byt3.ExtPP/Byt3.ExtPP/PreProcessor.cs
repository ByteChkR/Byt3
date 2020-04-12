using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;
using Utils = Byt3.ExtPP.Base.Utils;

namespace Byt3.ExtPP
{
    /// <summary>
    /// 
    /// </summary>
    public class PreProcessor : ALoggable<PPLogType>
    {
        /// <summary>
        /// List of loaded plugins
        /// </summary>
        private List<AbstractPlugin> plugins = new List<AbstractPlugin>();


        /// <summary>
        /// 
        /// </summary>
        private readonly string sep = " ";

        /// <summary>
        /// Returns the List of statements from all the plugins that are remaining in the file and need to be removed as a last step
        /// </summary>
        private List<string> CleanUpList
        {
            get
            {
                List<string> ret = new List<string>();

                foreach (AbstractPlugin plugin in plugins)
                {
                    ret.AddRange(plugin.Cleanup);
                }
                return ret;

            }
        }

        /// <summary>
        /// Sets the File Processing Chain
        /// 0 => First Plugin that gets executed
        /// </summary>
        /// <param name="fileProcessors">The List of Pluugins that will be used when processing files</param>
        public void SetFileProcessingChain(List<AbstractPlugin> fileProcessors)
        {
            plugins = fileProcessors;
        }


        public ISourceScript[] ProcessFiles(IFileContent[] files, Settings settings, IDefinitions definitions)
        {
            return Process(files, settings, definitions);
        }

        public string[] CompileFileList(ISourceScript[] files)
        {
            return Compile(files, true);
        }

        /// <summary>
        /// Compiles a File with the definitions and settings provided
        /// </summary>
        /// <param name="files">FilePaths of the files.</param>
        /// <param name="settings">The settings used in this compilation</param>
        /// <param name="defs">Definitions</param>
        /// <returns>Array of Compiled Lines</returns>
        public string[] Run(IFileContent[] files, Settings settings, IDefinitions defs)
        {

            Logger.Log(PPLogType.Log, Verbosity.Level1, "Starting Pre Processor...");
            ISourceScript[] src = Process(files, settings, defs);
            string[] ret = Compile(src, false);

            Logger.Log(PPLogType.Progress, Verbosity.Level1, "Finished in {0}ms.", Timer.MS);

            Timer.GlobalTimer.Reset();

            return ret;
        }


        /// <summary>
        /// Compiles a File with the definitions and settings provided
        /// </summary>
        /// <param name="files">FilePaths of the files.</param>
        /// <param name="defs">Definitions</param>
        /// <returns>Array of Compiled Lines</returns>
        public string[] Run(string[] files, IDefinitions defs)
        {
            return Run(files.Select(x => new FilePathContent(x)).OfType<IFileContent>().ToArray(), null, defs);
        }


        public string[] Run(IFileContent[] content, IDefinitions defs)
        {
            return Run(content, null, defs);
        }

        /// <summary>
        /// Compiles a File with the definitions and settings provided
        /// </summary>
        /// <param name="files">FilePaths of the files.</param>
        /// <param name="settings">The settings used in this compilation</param>
        /// <returns>Array of Compiled Lines</returns>
        public string[] Run(IFileContent[] files, Settings settings)
        {
            return Run(files, settings, null);
        }


        /// <summary>
        /// Initializing all Plugins with the settings, definitions and the source manager for this compilation
        /// </summary>
        /// <param name="settings">The settings used</param>
        /// <param name="def">Definitions used</param>
        /// <param name="sourceManager">Sourcemanager used</param>
        private void InitializePlugins(Settings settings, IDefinitions def, ISourceManager sourceManager)
        {
            Logger.Log(PPLogType.Log, Verbosity.Level1, "Initializing Plugins...");
            foreach (AbstractPlugin plugin in plugins)
            {
                Logger.Log(PPLogType.Log, Verbosity.Level2, "Initializing Plugin: {0}", plugin.GetType().Name);

                plugin.Initialize(settings.GetSettingsWithPrefix(plugin.Prefix, plugin.IncludeGlobal), sourceManager,
                    def);
            }
        }


        /// <summary>
        /// Compiles the Provided source array into a single file. And removes all remaining statements
        /// </summary>
        /// <param name="src">The Array of Sourcescripts that need to be compiled.</param>
        /// <returns>A compiled list out of the passed sourcescripts</returns>
        private string[] Compile(ISourceScript[] src, bool restartTimer)
        {
            if (restartTimer)
            {
                Timer.GlobalTimer.Restart();
            }

            long old = Timer.MS;
            Logger.Log(PPLogType.Log, Verbosity.Level2, "Starting Compilation of File Tree...");
            List<string> ret = new List<string>();
            for (int i = src.Length - 1; i >= 0; i--)
            {
                string[] sr = src[i].GetSource();
                if (sr != null && sr.Length != 0)
                {
                    ret.AddRange(sr);
                }
            }

            Logger.Log(PPLogType.Log, Verbosity.Level2, "Finished Compilation...");
            //this.Log(PPLogType.Log, Verbosity.LEVEL3, "Cleaning up: {0}", CleanUpList.Unpack(", "));

            string[] rrr = Utils.RemoveStatements(ret, CleanUpList.ToArray()).ToArray();

            Logger.Log(PPLogType.Progress, Verbosity.Level1, "Finished Compiling {1} Files({0}ms)", Timer.MS - old,
                src.Length);
            Logger.Log(PPLogType.Log, Verbosity.Level2, "Total Lines: {0}", rrr.Length);
            return rrr;
        }

        /// <summary>
        /// Processes the file with the settings, definitions and the source manager specified.
        /// </summary>
        /// <param name="files">the file paths to be processed</param>
        /// <param name="settings">the settings that are used</param>
        /// <param name="defs">the definitions that are used</param>
        /// <returns>Returns a list of files that can be compiled in reverse order</returns>
        private ISourceScript[] Process(IFileContent[] files, Settings settings, IDefinitions defs)
        {
            Timer.GlobalTimer.Restart();
            //string dir = Directory.GetCurrentDirectory();
            IDefinitions definitions = defs ?? new Definitions();
            settings = settings ?? new Settings();
            SourceManager sm = new SourceManager(plugins);

            long old = Timer.MS;
            InitializePlugins(settings, definitions, sm);
            Logger.Log(PPLogType.Progress, Verbosity.Level1, "Finished Initializing {1} Plugins({0}ms)", Timer.MS - old,
                plugins.Count);

            old = Timer.MS;
            foreach (IFileContent file in files)
            {

                sm.SetLock(false);
                sm.TryCreateScript(out ISourceScript sss, sep, file, new ImportResult());
                sm.SetLock(true);
                sm.AddToTodo(sss);
            }

            Logger.Log(PPLogType.Progress, Verbosity.Level1, "Loaded {1} Files in {0}ms", Timer.MS - old,
                sm.GetTodoCount());

            Logger.Log(PPLogType.Log, Verbosity.Level1, "Starting Processing of Files: {0}", files.Unpack(", "));

            old = Timer.MS;
            ISourceScript ss = sm.NextItem;

            do
            {

                if (!(ss as SourceScript).IsSourceLoaded)
                {
                    RunStages(this, ProcessStage.OnLoadStage, ss, sm, definitions);
                }

                Logger.Log(PPLogType.Progress, Verbosity.Level1, "Remaining Files: {0}", sm.GetTodoCount());
                Logger.Log(PPLogType.Log, Verbosity.Level2, "Selecting File: {0}",
                    Path.GetFileName(ss.GetFileInterface().GetKey()));
                //RUN MAIN
                sm.SetLock(false);
                RunStages(this, ProcessStage.OnMain, ss, sm, definitions);
                sm.SetLock(true);
                sm.SetState(ss, ProcessStage.OnFinishUp);
                ss = sm.NextItem;
            } while (ss != null);


            //Directory.SetCurrentDirectory(dir);
            ISourceScript[] ret = sm.GetList().ToArray();
            Logger.Log(PPLogType.Log, Verbosity.Level1, "Finishing Up...");
            foreach (ISourceScript finishedScript in ret)
            {
                Logger.Log(PPLogType.Log, Verbosity.Level2, "Selecting File: {0}",
                    Path.GetFileName(finishedScript.GetFileInterface().GetKey()));
                RunStages(this, ProcessStage.OnFinishUp, finishedScript, sm, definitions);
            }
            Logger.Log(PPLogType.Log, Verbosity.Level1, "Finished Processing Files.");

            Logger.Log(PPLogType.Progress, Verbosity.Level1, "Processed {1} Files into {2} scripts in {0}ms",
                Timer.MS - old, sm.GetList().Count, ret.Length);

            return ret;

        }


        /// <summary>
        /// Runs the specified stage on the passed script
        /// </summary>
        /// <param name="stage">The stage of the current processing</param>
        /// <param name="script">the script to be processed</param>
        /// <param name="sourceManager"></param>
        /// <param name="defTable">the definitions that are used</param>
        /// <returns></returns>
        private static bool RunStages(PreProcessor pp, ProcessStage stage, ISourceScript script,
            ISourceManager sourceManager,
            IDefinitions defTable)
        {
            if (!pp.RunPluginStage(PluginType.LinePluginBefore, stage, script, sourceManager, defTable))
            {
                return false;
            }
            if (stage != ProcessStage.OnFinishUp &&
                !pp.RunPluginStage(PluginType.FullScriptPlugin, stage, script, sourceManager, defTable))
            {
                return false;
            }
            if (!pp.RunPluginStage(PluginType.LinePluginAfter, stage, script, sourceManager, defTable))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Runs the plugin stage with the specififed type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stage">The stage of the current processing</param>
        /// <param name="script">the script to be processed</param>
        /// <param name="sourceManager"></param>
        /// <param name="defTable">the definitions that are used</param>
        /// <returns>True if the operation completed successfully</returns>
        private bool RunPluginStage(PluginType type, ProcessStage stage, ISourceScript script,
            ISourceManager sourceManager, IDefinitions defTable)
        {
            List<AbstractPlugin> chain = AbstractPlugin.GetPluginsForStage(plugins, type, stage);


            bool ret = true;

            if (type == PluginType.FullScriptPlugin)
            {
                ret = RunFullScriptStage(chain, stage, script, sourceManager, defTable);
            }
            else if (type == PluginType.LinePluginBefore || type == PluginType.LinePluginAfter)
            {
                string[] src = script.GetSource();
                RunLineStage(chain, stage, src);
                script.SetSource(src);
            }
            if (!ret)
            {
                return false;
            }


            return true;
        }


        /// <summary>
        /// Wrapper that runs a list of line plugins based on the stage that is beeing run.
        /// </summary>
        /// <param name="lineStage">The chain for this stage</param>
        /// <param name="stage">The stage of the current processing</param>
        /// <param name="source">The source to operate on</param>
        private static void RunLineStage(List<AbstractPlugin> lineStage, ProcessStage stage, string[] source)
        {
            foreach (AbstractPlugin abstractPlugin in lineStage)
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (stage == ProcessStage.OnLoadStage)
                    {
                        source[i] = abstractPlugin.OnLoad_LineStage(source[i]);
                    }
                    else if (stage == ProcessStage.OnMain)
                    {
                        source[i] = abstractPlugin.OnMain_LineStage(source[i]);
                    }
                    else if (stage == ProcessStage.OnFinishUp)
                    {
                        source[i] = abstractPlugin.OnFinishUp_LineStage(source[i]);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullScriptStage">The chain for this stage</param>
        /// <param name="stage">The stage of the current processing</param>
        /// <param name="script">The script to operate on</param>
        /// <param name="sourceManager">The sourcemanager used.</param>
        /// <param name="defTable">The definitions used</param>
        /// <returns></returns>
        private bool RunFullScriptStage(List<AbstractPlugin> fullScriptStage, ProcessStage stage, ISourceScript script,
            ISourceManager sourceManager, IDefinitions defTable)
        {
            foreach (AbstractPlugin abstractPlugin in fullScriptStage)
            {
                bool ret = true;
                Logger.Log(PPLogType.Log, Verbosity.Level3, "Running Plugin: {0}: {1} on file {2}", abstractPlugin,
                    stage, Path.GetFileName(script.GetFileInterface().GetKey()));
                if (stage == ProcessStage.OnLoadStage)
                {
                    ret = abstractPlugin.OnLoad_FullScriptStage(script, sourceManager, defTable);
                }
                else if (stage == ProcessStage.OnMain)
                {
                    ret = abstractPlugin.OnMain_FullScriptStage(script, sourceManager, defTable);
                }

                if (!ret)
                {
                    Logger.Log(PPLogType.Error, Verbosity.Level1, "Processing was aborted by Plugin: {0}",
                        abstractPlugin);
                    return false;
                }
            }

            return true;
        }
    }
}