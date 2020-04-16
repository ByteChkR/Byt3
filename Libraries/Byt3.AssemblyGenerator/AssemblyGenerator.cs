using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.ADL;
using Byt3.Utilities.DotNet;
using Byt3.Utilities.DotNet.ProjectParsing;
using Byt3.Utilities.Serialization;

namespace Byt3.AssemblyGenerator
{
    public static class AssemblyGenerator
    {
        public static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(AssemblyGeneratorDebugConfig.Settings, "AssemblyGenerator");

        #region API Calls

        public static void GenerateAssembly(string msBuildPath, AssemblyDefinition assemblyDefinitions,
            string outputFolder, AssemblyGeneratorBuildType buildType, bool lib = true)
        {
            if (msBuildPath == null)
            {
                throw new ArgumentNullException(nameof(msBuildPath));
            }
            if (assemblyDefinitions == null)
            {
                throw new ArgumentNullException(nameof(assemblyDefinitions));
            }
            if (outputFolder == null)
            {
                throw new ArgumentNullException(nameof(outputFolder));
            }


            Logger.Log(LogType.Log, "Generating Assembly...", 1);
            string tempFolder = GetTempFolder();

            Tuple<string, List<ModuleDefinition>> project =
                GenerateProject(msBuildPath, tempFolder, assemblyDefinitions, lib);


            string projectDir = Path.GetDirectoryName(project.Item1);


            Logger.Log(LogType.Log, $"Copying Files of {project.Item2.Count} Modules", 1);
            for (int i = 0; i < project.Item2.Count; i++)
            {

                string defDir = CreateDirectoryInFolderOrThrow(projectDir, project.Item2[i].Name);
                MoveFiles(defDir, project.Item2[i]);
            }

            string outDir = "";
            if (buildType == AssemblyGeneratorBuildType.Build)
            {
                outDir = DotNetHelper.BuildProject(msBuildPath, project.Item1, assemblyDefinitions, lib);
            }
            else
            {
                outDir = DotNetHelper.PublishProject(msBuildPath, project.Item1, assemblyDefinitions, lib);
            }

            Logger.Log(LogType.Log, "Moving Files to output Directory...", 1);
            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder, true);
            }

            Logger.Log(LogType.Log, "Cleaning Output Folder", 1);
            while (Directory.Exists(outputFolder))
            {
                //Console.Write(".");
                //Wait
            }

            Directory.Move(outDir, outputFolder);


            Logger.Log(LogType.Log, "Cleaning Temp Directory", 1);
            Directory.Delete(tempFolder, true);
            Logger.Log(LogType.Log, "Cleanup Finished.", 1);
        }

        public static ModuleDefinition GenerateModuleDefinition(string project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            CSharpProject p = ProjectLoader.LoadProject(project);
            List<CSharpReference> embedFiles = p.EmbeddedReferences;
            List<CSharpReference> packageFiles = p.PackageReferences;
            List<CSharpReference> projectFiles = p.ProjectReferences;

            ModuleDefinition def = new ModuleDefinition(
                Path.GetFileNameWithoutExtension(project),
                Path.GetDirectoryName(Path.GetFullPath(project)),
                packageFiles.ToArray(),
                projectFiles.ToArray(),
                embedFiles.ToArray());

            return def;
        }

        public static ModuleDefinition[] GenerateModuleDefinitions(string folder, string moduleConfigOutputDir,
            bool isWhiteList = false, string[] exceptionList = null)
        {

            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }
            if (moduleConfigOutputDir == null)
            {
                throw new ArgumentNullException(nameof(moduleConfigOutputDir));
            }
            Directory.CreateDirectory(moduleConfigOutputDir);
            string[] projects = Directory.GetFiles(folder, "*.csproj", SearchOption.AllDirectories);

            List<ModuleDefinition> ret = new List<ModuleDefinition>();

            foreach (string project in projects)
            {

                if (isWhiteList && !ContainsItem(project, exceptionList))
                {
                    continue;
                }

                if (!isWhiteList && ContainsItem(project, exceptionList))
                {
                    continue;
                }


                ret.Add(GenerateModuleDefinition(project));
                //ModuleDefinition.Save(Path.Combine(moduleConfigOutputDir, def.Name) + ".moduleconfig", def);
            }

            return ret.ToArray();
        }


        public static AssemblyDefinition GenerateAssemblyDefinition(string assemblyName, string moduleFolder)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }
            if (moduleFolder == null)
            {
                throw new ArgumentNullException(nameof(moduleFolder));
            }

            string[] files = Directory.GetFiles(moduleFolder, "*.moduleconfig", SearchOption.AllDirectories);
            return GenerateAssemblyDefinition(assemblyName, files.Select(x => ModuleDefinition.Load(x)).ToArray());
        }

        public static AssemblyDefinition GenerateAssemblyDefinition(string assemblyName, ModuleDefinition[] modules)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }
            if (modules == null)
            {
                throw new ArgumentNullException(nameof(modules));
            }
            AssemblyDefinition defs = new AssemblyDefinition(assemblyName);
            foreach (ModuleDefinition module in modules)
            {
                defs.IncludedModules.Add(module);
            }

            return defs;
        }

        #endregion

        #region Private Functions

        private static CSharpReference PrepareForTransfer(CSharpReference reference, ModuleDefinition original)
        {
            List<SerializableKeyValuePair<string, string>> attribs = reference.internalAttributes;
            for (int i = 0; i < attribs.Count; i++)
            {
                if (attribs[i].Key == "Include")
                {
                    SerializableKeyValuePair<string, string> serializableKeyValuePair = attribs[i];
                    serializableKeyValuePair.Value = Path.GetFullPath(Path.Combine(original.RootDirectory,
                        reference.internalAttributes[i].Value));
                    attribs[i] = serializableKeyValuePair;
                }
            }

            reference.internalAttributes = attribs;
            return reference;

        }

        private static void DiscoverModules(ModuleDefinition module, List<ModuleDefinition> definitions)
        {
            Logger.Log(LogType.Log, "Adding Module: " + module.Name, 1);
            definitions.Add(module);
            for (int i = 0; i < module.Projects.Length; i++)
            {
                string path =
                    Path.GetFullPath(Path.Combine(module.RootDirectory, module.Projects[i].Attributes["Include"]));

                ModuleDefinition mod =
                    GenerateModuleDefinition(path);
                if (definitions.Count(x => x.Name == mod.Name) == 0)
                {
                    DiscoverModules(mod, definitions);
                }
            }
        }

        private static Tuple<string, List<ModuleDefinition>> GenerateProject(string msBuildPath, string workingDir,
            AssemblyDefinition definition, bool lib = true)
        {
            if (workingDir == null)
            {
                throw new ArgumentNullException(nameof(workingDir));
            }
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            if (!Directory.Exists(workingDir))
            {
                throw new DirectoryNotFoundException("Can not find the working directory: " + workingDir);
            }


            Logger.Log(LogType.Log, "Generating csproject File...", 1);

            DotNetHelper.New(msBuildPath, workingDir, definition.AssemblyName, lib);

            File.Delete(Path.Combine(workingDir, definition.AssemblyName,
                lib ? "Class1.cs" : "Program.cs")); //Delete Default Class

            string projectFile = Path.Combine(workingDir, definition.AssemblyName, definition.AssemblyName + ".csproj");

            List<ModuleDefinition> modules = new List<ModuleDefinition>();
            for (int i = 0; i < definition.IncludedModules.Count; i++)
            {
                if (modules.Count(x => x.Name == definition.IncludedModules[i].Name) == 0)
                {
                    DiscoverModules(definition.IncludedModules[i], modules);
                }
            }

            Logger.Log(LogType.Log, $"Discovered {modules.Count} Modules.", 1);

            CSharpProject p = ProjectLoader.LoadProject(projectFile);

            foreach (ModuleDefinition defintionDefinition in modules)
            {
                for (int i = 0; i < defintionDefinition.Packages.Length; i++)
                {
                    p.AddReference(defintionDefinition.Packages[i]);
                }
                //for (int i = 0; i < defintionDefinition.Projects.Length; i++)
                //{
                //    CSharpReference r = PrepareForTransfer(defintionDefinition.Projects[i],
                //        defintionDefinition);
                //    string path = r.Attributes["Include"];
                //    if (modules.Count(x => path == Path.GetFullPath(Path.Combine(x.RootDirectory, x.Name + ".csproj"))) == 0)
                //    {
                //        p.AddReference(r);
                //    }
                //}

                for (int i = 0; i < defintionDefinition.EmbeddedFiles.Length; i++)
                {
                    p.AddReference(defintionDefinition.EmbeddedFiles[i]);
                }
            }

            File.Delete(projectFile);
            p.Save(projectFile);

            return new Tuple<string, List<ModuleDefinition>>(projectFile, modules);
        }


        private static void MoveFiles(string targetDir, ModuleDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            string[] scripts = definition.ScriptFiles;
            string root = definition.RootDirectory;
            if (!root.EndsWith("\\"))
            {
                root += "\\";
            }
            for (int i = 0; i < scripts.Length; i++)
            {
                string relativePath = scripts[i].Replace(root, "");
                string targetPath = Path.Combine(targetDir, relativePath);
                string originalPath = scripts[i];
                string containingDir = Path.GetDirectoryName(targetPath);

                if (containingDir != "" && !Directory.Exists(containingDir))
                {
                    Directory.CreateDirectory(containingDir);
                }
                File.Copy(originalPath, targetPath);
            }
            for (int i = 0; i < definition.EmbeddedFiles.Length; i++)
            {
                string relativePath = definition.EmbeddedFiles[i].Attributes["Include"];
                string targetPath = Path.Combine(targetDir, relativePath);
                string originalPath = Path.Combine(root, relativePath);
                string containingDir = Path.GetDirectoryName(targetPath);
                if (containingDir != "" && !Directory.Exists(containingDir))
                {
                    Directory.CreateDirectory(containingDir);
                }
                File.Copy(originalPath, targetPath);
            }

        }

        #endregion

        #region Private Helper Functions

        private static bool ContainsItem(string val, string[] blacklist)
        {
            if (val == null)
            {
                throw new ArgumentNullException(nameof(val));
            }
            if (blacklist == null)
            {
                throw new ArgumentNullException(nameof(blacklist));
            }
            for (int i = 0; i < blacklist.Length; i++)
            {
                if (blacklist[i] == null)
                {
                    throw new ArgumentNullException(nameof(blacklist), "Item " + i + " is null");
                }
                if (val.Contains(blacklist[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetTempFolder()
        {
#if Release
            string ret = Path.GetTempPath(); //Something Like C:\Temp
#else
            string ret = Path.GetFullPath(".\\");
#endif
            ret = Path.Combine(ret,
                Path.GetFileNameWithoutExtension(Path.GetTempFileName())); //Something Like C:\Temp\tmp02qa\
            if (Directory.Exists(ret))
            {
                throw new Exception("Temp Dir already exists");
            }
            Directory.CreateDirectory(ret);
            return ret;


        }


        private static string CreateDirectoryInFolderOrThrow(string parentFolder, string newFolderName)
        {
            string ret = Path.Combine(parentFolder, newFolderName);
            if (Directory.Exists(ret))
            {
                throw new Exception("Directory Already exists");
            }
            else
            {
                Directory.CreateDirectory(ret);
            }
            return ret;
        }

        #endregion
    }
}