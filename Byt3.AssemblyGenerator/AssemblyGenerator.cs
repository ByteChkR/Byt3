using System;
using System.Diagnostics;
using System.IO;

namespace Byt3.BuildSystem
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

        private static string GenerateProject(string workingDir, string projectName)
        {
            Console.WriteLine("Generating csproject File...");
            string arguments = $"classlib -n {projectName}";
            DotnetAction("new", arguments, workingDir);

            File.Delete(Path.Combine(workingDir, projectName, "Class1.cs")); //Delete Default Class

            return Path.Combine(workingDir, projectName, projectName + ".csproj");
        }

        private static void DotnetAction(string targetCommand, string arguments, string workingDir)
        {
            CommandInfo info =
                new CommandInfo($"dotnet {targetCommand} {arguments}", workingDir, true);
            info.CreateWindow = false;
            info.CaptureConsoleOut = true;
            info.OnErrorReceived = OnErrorReceived;
            info.OnOutputReceived = OnOutReceived;
            ProcessRunner.RunCommand(info);
        }

        private static void OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            Console.WriteLine("\t[ERR]" + e.Data);

        }

        private static void OnOutReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            Console.WriteLine("\t[OUT]" + e.Data);

        }


        private static string BuildProject(string projectFile, AssemblyDefinitions definitions)
        {
            Console.WriteLine("Building Assembly: " + definitions.AssemblyName);
            string arguments = $"-c {definitions.BuildConfiguration}";
            if (!definitions.NoTargetRuntime)
            {
                arguments = $"--runtime {definitions.BuildTargetRuntime} {arguments}";
            }

            string workingDir = Path.GetDirectoryName(projectFile);
            DotnetAction("build", arguments, workingDir);
            string ret = Path.Combine(workingDir, "bin", definitions.BuildConfiguration, "netstandard2.0");
            if (!definitions.NoTargetRuntime)
            {
                ret = Path.Combine(ret, definitions.BuildTargetRuntime);
            }

            return ret;
        }

        private static string PublishProject(string projectFile, AssemblyDefinitions definitions)
        {
            Console.WriteLine("Publishing Assembly: " + definitions.AssemblyName);
            string arguments = $"-c {definitions.BuildConfiguration}";
            if (!definitions.NoTargetRuntime)
            {
                arguments = $"--runtime {definitions.BuildTargetRuntime} {arguments}";
            }
            string workingDir = Path.GetDirectoryName(projectFile);
            DotnetAction("publish", arguments, workingDir);
            string ret = Path.Combine(workingDir, "bin", definitions.BuildConfiguration, "netstandard2.0");
            if (!definitions.NoTargetRuntime)
            {
                ret = Path.Combine(ret, definitions.BuildTargetRuntime);
            }

            ret = Path.Combine(ret,"publish");
            return ret;
        }

        private static void MoveFiles(string targetDir, ModuleDefinition definition)
        {
            Console.WriteLine("Copying Files of Module: " + definition.Name);
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
                Console.WriteLine("File Copied: " + relativePath);
            }

        }

        private static string PrepareDefinitionDir(string targetDir, string definitionName)
        {
            Console.WriteLine("Preparing Definition Directory: " + definitionName);
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



        public static void GenerateAssembly(AssemblyDefinitions assemblyDefinitions, string outputFolder)
        {
            Console.WriteLine("Generating Assembly...");
            string tempFolder = GetTempFolder();

            string project = GenerateProject(tempFolder, assemblyDefinitions.AssemblyName);
            string projectDir = Path.GetDirectoryName(project);


            for (int i = 0; i < assemblyDefinitions.Definitions.Count; i++)
            {

                string defDir = PrepareDefinitionDir(projectDir, assemblyDefinitions.Definitions[i].Name);
                MoveFiles(defDir, assemblyDefinitions.Definitions[i]);
            }

            string outDir = "";
            if (assemblyDefinitions.Type == BuildType.Build)
            {
                outDir = BuildProject(project, assemblyDefinitions);
            }
            else
            {
                outDir = PublishProject(project, assemblyDefinitions);
            }

            Console.WriteLine("Moving Files to output Directory...");
            if (Directory.Exists(outputFolder)) Directory.Delete(outputFolder, true);
            Directory.Move(outDir, outputFolder);


            Console.WriteLine("Cleaning Temp Directory");
            Directory.Delete(tempFolder, true);
            Console.WriteLine("Cleanup Finished.");
        }


        public static void GenerateModuleDefinitions(string folder, string moduleConfigOutputDir, bool isWhiteList = false, string[] exceptionList = null)
        {
            Directory.CreateDirectory(moduleConfigOutputDir);
            string[] projects = Directory.GetFiles(folder, "*.csproj", SearchOption.AllDirectories);

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
                Console.WriteLine("Creating Module Info for: " + Path.GetFileNameWithoutExtension(project));
                ModuleDefinition def = new ModuleDefinition(Path.GetFileNameWithoutExtension(project), Path.GetDirectoryName(Path.GetFullPath(project)));
                ModuleDefinition.Save(Path.Combine(moduleConfigOutputDir, def.Name) + ".moduleconfig", def);
            }
        }

        private static bool ContainsItem(string val, string[] blacklist)
        {
            for (int i = 0; i < blacklist.Length; i++)
            {
                if (val.Contains(blacklist[i])) return true;
            }

            return false;
        }

        public static void GenerateAssemblyDefinitions(string assemblyName, string moduleFolder)
        {
            AssemblyDefinitions defs = new AssemblyDefinitions(BuildType.Publish, assemblyName);
            string[] files = Directory.GetFiles(moduleFolder, "*.moduleconfig", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                defs.Definitions.Add(ModuleDefinition.Load(file));
            }

            AssemblyDefinitions.Save(Path.Combine(moduleFolder, defs.AssemblyName + ".assemblyconfig"), defs);
        }

    }
}