using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.Utilities.DotNet;
using Byt3.Utilities.DotNet.ProjectParsing;
using Byt3.Utilities.Serialization;

namespace Byt3.AssemblyGenerator
{
    public static class AssemblyGenerator
    {
        private static string GetTempFolder()
        {
#if Release
            string ret = Path.GetTempPath(); //Something Like C:\Temp
#else
            string ret = Path.GetFullPath(".\\");
#endif
            ret = Path.Combine(ret, Path.GetFileNameWithoutExtension(Path.GetTempFileName())); //Something Like C:\Temp\tmp02qa\
            if (Directory.Exists(ret)) throw new Exception("Temp Dir already exists");
            Directory.CreateDirectory(ret);
            return ret;


        }

        private static CSharpReference PrepareForTransfer(CSharpReference reference, ModuleDefinition original)
        {
            List<KVP<string, string>> attribs = reference.internalAttributes;
            for (int i = 0; i < attribs.Count; i++)
            {
                if (attribs[i].Key == "Include")
                {
                    KVP<string, string> kvp = attribs[i];
                    kvp.Value = Path.GetFullPath(Path.Combine(original.RootDirectory,
                        reference.internalAttributes[i].Value));
                    attribs[i] = kvp;
                }
            }

            reference.internalAttributes = attribs;
            return reference;

        }

        private static void DiscoverModules(ModuleDefinition module, List<ModuleDefinition> definitions)
        {
            Console.WriteLine("Adding Module: " + module.Name);
            definitions.Add(module);
            for (int i = 0; i < module.Projects.Length; i++)
            {
                string path = Path.GetFullPath(Path.Combine(module.RootDirectory, module.Projects[i].Attributes["Include"]));

                ModuleDefinition mod =
                    GenerateModuleDefinition(path);
                if (definitions.Count(x => x.Name == mod.Name) == 0)
                {
                    DiscoverModules(mod, definitions);
                }
            }
        }

        private static Tuple<string, List<ModuleDefinition>> GenerateProject(string msBuildPath, string workingDir, AssemblyDefinition defintion, bool lib = true)
        {
            Console.WriteLine("Generating csproject File...");

            DotNetHelper.New(msBuildPath, workingDir, defintion.AssemblyName, lib);

            File.Delete(Path.Combine(workingDir, defintion.AssemblyName, lib ? "Class1.cs" : "Program.cs")); //Delete Default Class

            string projectFile = Path.Combine(workingDir, defintion.AssemblyName, defintion.AssemblyName + ".csproj");

            List<ModuleDefinition> modules = new List<ModuleDefinition>();
            for (int i = 0; i < defintion.IncludedModules.Count; i++)
            {
                if (modules.Count(x => x.Name == defintion.IncludedModules[i].Name) == 0)
                    DiscoverModules(defintion.IncludedModules[i], modules);
            }

            Console.WriteLine($"Discovered {modules.Count} Modules.");

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

            string[] scripts = definition.ScriptFiles;
            string root = definition.RootDirectory;
            if (!root.EndsWith("\\")) root += "\\";
            for (int i = 0; i < scripts.Length; i++)
            {
                string relativePath = scripts[i].Replace(root, "");
                string targetPath = Path.Combine(targetDir, relativePath);
                string originalPath = scripts[i];
                string containingDir = Path.GetDirectoryName(targetPath);

                if (containingDir != "" && !Directory.Exists(containingDir)) Directory.CreateDirectory(containingDir);
                File.Copy(originalPath, targetPath);
            }
            for (int i = 0; i < definition.EmbeddedFiles.Length; i++)
            {
                string relativePath = definition.EmbeddedFiles[i].Attributes["Include"];
                string targetPath = Path.Combine(targetDir, relativePath);
                string originalPath = Path.Combine(root, relativePath);
                string containingDir = Path.GetDirectoryName(targetPath);
                if (containingDir != "" && !Directory.Exists(containingDir)) Directory.CreateDirectory(containingDir);
                File.Copy(originalPath, targetPath);
            }

        }

        private static string PrepareDefinitionDir(string targetDir, string definitionName)
        {
            string ret = Path.Combine(targetDir, definitionName);
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



        public static void GenerateAssembly(string msBuildPath, AssemblyDefinition assemblyDefinitions, string outputFolder, AssemblyGeneratorBuildType buildType, bool lib = true)
        {
            Console.WriteLine("Generating Assembly...");
            string tempFolder = GetTempFolder();

            Tuple<string, List<ModuleDefinition>> project = GenerateProject(msBuildPath, tempFolder, assemblyDefinitions, lib);


            string projectDir = Path.GetDirectoryName(project.Item1);


            Console.WriteLine($"Copying Files of {project.Item2.Count} Modules");
            for (int i = 0; i < project.Item2.Count; i++)
            {

                string defDir = PrepareDefinitionDir(projectDir, project.Item2[i].Name);
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

            Console.WriteLine("Moving Files to output Directory...");
            if (Directory.Exists(outputFolder)) Directory.Delete(outputFolder, true);

            Console.WriteLine("Cleaning Output Folder");
            while (Directory.Exists(outputFolder))
            {
                Console.Write(".");
                //Wait
            }

            Directory.Move(outDir, outputFolder);


            Console.WriteLine("Cleaning Temp Directory");
            Directory.Delete(tempFolder, true);
            Console.WriteLine("Cleanup Finished.");
        }

        public static ModuleDefinition GenerateModuleDefinition(string project)
        {
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
        public static ModuleDefinition[] GenerateModuleDefinitions(string folder, string moduleConfigOutputDir, bool isWhiteList = false, string[] exceptionList = null)
        {
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

        private static bool ContainsItem(string val, string[] blacklist)
        {
            for (int i = 0; i < blacklist.Length; i++)
            {
                if (val.Contains(blacklist[i])) return true;
            }

            return false;
        }

        public static AssemblyDefinition GenerateAssemblyDefinition(string assemblyName, string moduleFolder)
        {
            string[] files = Directory.GetFiles(moduleFolder, "*.moduleconfig", SearchOption.AllDirectories);
            return GenerateAssemblyDefinition(assemblyName, files.Select(x => ModuleDefinition.Load(x)).ToArray());
        }

        public static AssemblyDefinition GenerateAssemblyDefinition(string assemblyName, ModuleDefinition[] modules)
        {
            AssemblyDefinition defs = new AssemblyDefinition(assemblyName);
            foreach (ModuleDefinition module in modules)
            {
                defs.IncludedModules.Add(module);
            }

            return defs;
        }
    }
}