using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;
using Utils = Byt3.ExtPP.Base.Utils;

namespace Byt3.ExtPP.Plugins
{
    public class IncludePlugin : AbstractFullScriptPlugin
    {
        public override string[] Cleanup => new[] {IncludeKeyword};
        public override ProcessStage ProcessStages => ProcessStage.OnMain;
        public override string[] Prefix => new[] {"inc", "Include"};
        public string IncludeKeyword { get; set; } = "#include";
        public string IncludeInlineKeyword { get; set; } = "#includeinl";
        public string Separator { get; set; } = " ";

        public override List<CommandInfo> Info { get; } = new List<CommandInfo>
        {
            new CommandInfo("set-include", "i",
                PropertyHelper.GetPropertyInfo(typeof(IncludePlugin), nameof(IncludeKeyword)),
                "Sets the keyword that is used to include other files into the build process."),
            new CommandInfo("set-include-inline", "ii",
                PropertyHelper.GetPropertyInfo(typeof(IncludePlugin), nameof(IncludeInlineKeyword)),
                "Sets the keyword that is used to insert other files directly into the current file"),
            new CommandInfo("set-separator", "s",
                PropertyHelper.GetPropertyInfo(typeof(IncludePlugin), nameof(Separator)),
                "Sets the separator that is used to separate the include statement from the filepath"),
        };

        public override void Initialize(Settings settings, ISourceManager sourceManager, IDefinitions defTable)
        {

            settings.ApplySettings(Info, this);
        }


        public override bool FullScriptStage(ISourceScript script, ISourceManager sourceManager, IDefinitions defs)
        {

            Logger.Log(PPLogType.Log, Verbosity.Level5, "Disovering Include Statments...");
            List<string> source = script.GetSource().ToList();
            string currentPath = Path.GetDirectoryName(script.GetFileInterface().GetFilePath());
            bool hasIncludedInline;
            do
            {
                hasIncludedInline = false;
                for (int i = source.Count - 1; i >= 0; i--)
                {
                    if (Utils.IsStatement(source[i], IncludeInlineKeyword))
                    {
                        Logger.Log(PPLogType.Log, Verbosity.Level6, "Found Inline Include Statement...");
                        string[] args = Utils.SplitAndRemoveFirst(source[i], Separator);
                        if (args.Length == 0)
                        {

                            Logger.Log(PPLogType.Error, Verbosity.Level1, "No File Specified");
                            continue;
                        }

                        if (Utils.FileExistsRelativeTo(currentPath, args[0]))
                        {
                            Logger.Log(PPLogType.Log, Verbosity.Level6, "Replacing Inline Keyword with file content");
                            source.RemoveAt(i);

                            source.InsertRange(i, IOManager.ReadAllLines(Path.Combine(currentPath, args[0])));
                            hasIncludedInline = true;
                        }
                        else
                        {
                            Logger.Log(PPLogType.Error, Verbosity.Level1, $"File does not exist: {args[0]}");
                        }
                    }
                }
                script.SetSource(source.ToArray());
            } while (hasIncludedInline);


            string[] incs = Utils.FindStatements(source.ToArray(), IncludeKeyword);

            foreach (string includes in incs)
            {
                Logger.Log(PPLogType.Log, Verbosity.Level5, $"Processing Statement: {includes}");
                bool tmp = GetISourceScript(sourceManager, includes, currentPath, out List<ISourceScript> sources);
                if (tmp)
                {
                    foreach (ISourceScript sourceScript in sources)
                    {
                        Logger.Log(PPLogType.Log, Verbosity.Level6, $"Processing Include: {Path.GetFileName(sourceScript.GetFileInterface().GetKey())}");

                        if (!sourceManager.IsIncluded(sourceScript))
                        {
                            sourceManager.AddToTodo(sourceScript);
                        }
                        else
                        {
                            sourceManager.FixOrder(sourceScript);
                        }

                    }

                }
                else
                {
                    return
                        false; //We crash if we didnt find the file. but if the user forgets to specify the path we will just log the error
                }

            }

            Logger.Log(PPLogType.Log, Verbosity.Level5, "Inclusion of Files Finished");
            return true;

        }


        private bool GetISourceScript(ISourceManager manager, string statement, string currentPath,
            out List<ISourceScript> scripts)
        {
            string[] vars = Utils.SplitAndRemoveFirst(statement, Separator);

            scripts = new List<ISourceScript>();
            if (vars.Length != 0)
            {
                ImportResult importInfo = manager.GetComputingScheme()(vars, currentPath);
                if (!importInfo)
                {
                    Logger.Log(PPLogType.Error, Verbosity.Level1, "Invalid Include Statement");
                    return false;

                }

                string filepath = importInfo.GetString("filename");
                importInfo.RemoveEntry("filename");
                string key = importInfo.GetString("key");
                importInfo.RemoveEntry("key");


                if (filepath.EndsWith("\\*") || filepath.EndsWith("/*"))
                {
                    string[] files = IOManager.GetFiles(filepath.Substring(0, filepath.Length - 2));
                    foreach (string file in files)
                    {
                        IFileContent cont = new FilePathContent(file);
                        cont.SetKey(key);
                        if (manager.TryCreateScript(out ISourceScript iss, Separator, cont, importInfo))
                        {
                            scripts.Add(iss);
                        }
                    }
                }
                else
                {
                    IFileContent cont = new FilePathContent(filepath);
                    cont.SetKey(key);
                    if (manager.TryCreateScript(out ISourceScript iss, Separator, cont, importInfo))
                    {
                        scripts.Add(iss);
                    }
                }


                for (int index = scripts.Count - 1; index >= 0; index--)
                {
                    ISourceScript sourceScript = scripts[index];
                    if (sourceScript.GetFileInterface().HasValidFilepath &&
                        !Utils.FileExistsRelativeTo(currentPath, sourceScript.GetFileInterface()))
                    {
                        Logger.Log(PPLogType.Error, Verbosity.Level1, $"Could not find File: {sourceScript.GetFileInterface()}");
                        scripts.RemoveAt(index);
                    }
                }


                return true;

            }


            return false;
        }
    }
}