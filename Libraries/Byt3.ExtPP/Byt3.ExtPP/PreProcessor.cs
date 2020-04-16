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
    public class PreProcessor : ALoggable<LogType>
    {
        public PreProcessor() : base(ExtPPDebugConfig.Settings) { }

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

            //Logger.Log(LogType.Log, "Starting Pre Processor...", 1);
            ISourceScript[] src = Process(files, settings, defs);
            string[] ret = Compile(src, false);

            //Logger.Log(LogType.Progress, $"Finished in {Timer.MS}ms.", 1);

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
            Logger.Log(LogType.Log, "Initializing Plugins...", 2);
            foreach (AbstractPlugin plugin in plugins)
            {
                Logger.Log(LogType.Log, $"Initializing Plugin: {plugin.GetType().Name}", 3);

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
            Logger.Log(LogType.Log, "Starting Compilation of File Tree...", 4);
            List<string> ret = new List<string>();
            for (int i = src.Length - 1; i >= 0; i--)
            {
                string[] sr = src[i].GetSource();
                if (sr != null && sr.Length != 0)
                {
                    ret.AddRange(sr);
                }
            }

            //Logger.Log(LogType.Log, "Finished Compilation...", 2);
            //this.Log(PPLogType.Log,  "Cleaning up: {0}", CleanUpList.Unpack(", "));

            string[] rrr = Utils.RemoveStatements(ret, CleanUpList.ToArray()).ToArray();

            //Logger.Log(LogType.Progress, $"Finished Compiling {Timer.MS - old} Files({Timer.MS - old}ms)", 1);
            //Logger.Log(LogType.Log, $"Total Lines: {rrr.Length}", 1);
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
            //Logger.Log(LogType.Progress, $"Finished Initializing {plugins.Count} Plugins({Timer.MS - old}ms)", 1);

            old = Timer.MS;
            foreach (IFileContent file in files)
            {

                sm.SetLock(false);
                sm.TryCreateScript(out ISourceScript sss, sep, file, new ImportResult());
                sm.SetLock(true);
                sm.AddToTodo(sss);
            }

            //Logger.Log(LogType.Progress, $"Loaded {sm.GetTodoCount()} Files in {Timer.MS - old}ms", 1);

            //Logger.Log(LogType.Log, $"Starting Processing of Files: {files.Unpack(", ")}", 1);

            old = Timer.MS;
            ISourceScript ss = sm.NextItem;

            do
            {

                if (!(ss as SourceScript).IsSourceLoaded)
                {
                    RunStages(this, ProcessStage.OnLoadStage, ss, sm, definitions);
                }

                //Logger.Log(LogType.Progress, $"Remaining Files: {sm.GetTodoCount()}", 1);
                Logger.Log(LogType.Log, $"Selecting File: {Path.GetFileName(ss.GetFileInterface().GetKey())}", 3);
                //RUN MAIN
                sm.SetLock(false);
                RunStages(this, ProcessStage.OnMain, ss, sm, definitions);
                sm.SetLock(true);
                sm.SetState(ss, ProcessStage.OnFinishUp);
                ss = sm.NextItem;
            } while (ss != null);


            //Directory.SetCurrentDirectory(dir);
            ISourceScript[] ret = sm.GetList().ToArray();

            foreach (ISourceScript finishedScript in ret)
            {
                Logger.Log(LogType.Log, $"Selecting File: {Path.GetFileName(finishedScript.GetFileInterface().GetKey())}", 3);
                RunStages(this, ProcessStage.OnFinishUp, finishedScript, sm, definitions);
            }
            Logger.Log(LogType.Log, "Finished Processing Files.", 2);

            //Logger.Log(LogType.Progress,$"Processed {sm.GetList().Count} Files into {ret.Length} scripts in {Timer.MS - old}ms", 1);

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


            Logger.Log(LogType.Log, "Running Stage: " + stage + "|" + type, 3);

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
                Logger.Log(LogType.Log, $"Running Plugin: {abstractPlugin}: {stage} on file {Path.GetFileName(script.GetFileInterface().GetKey())}", 4);
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
                    Logger.Log(LogType.Error, $"Processing was aborted by Plugin: {abstractPlugin}", 1);
                    return false;
                }
            }

            return true;
        }
    }
}