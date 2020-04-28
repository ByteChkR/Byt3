using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Byt3.CommandRunner;
using Byt3.Engine.AssetPackaging;
using Byt3.Engine.BuildTools.Common;
using Byt3.Utilities.Exceptions;

namespace Byt3.Engine.BuildTools
{
    /// <summary>
    /// Class Containing the Building Logic that is used in the CLI and GUI Wrappers
    /// </summary>
    public static class Builder
    {
        private static bool IsWindows => Type.GetType("Mono.Runtime") == null;

        public static BuildSettings LoadSettings(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(BuildSettings));
            FileStream fs = new FileStream(path, FileMode.Open);
            BuildSettings bs = (BuildSettings) xs.Deserialize(fs);
            fs.Close();
            return bs;
        }

        public static bool RunCommand(string args)
        {
            return RunCommand(args.Split(' '));
        }


        public static bool RunCommand(string[] args)
        {
            Console.WriteLine("Windows: " + IsWindows);
            Runner.AddCommand(new DefaultHelpCommand());
            Runner.AddAssembly(Assembly.GetExecutingAssembly());


            return Runner.RunCommands(args);
        }


        public static void BuildProject(string filepath)
        {
            int ret = BuildCommand(filepath);
            if (ret != 0)
            {
                throw new BuildFailedException("Compilation Command Failed.");
            }

            ret = PublishCommand(filepath);
            if (ret != 0)
            {
                throw new BuildFailedException("Publish Command Failed.");
            }
        }

        public class BuildFailedException : Byt3Exception
        {
            public BuildFailedException(string message) : base(message) { }
        }

        private static int BuildCommand(string filepath)
        {
            string exec = IsWindows ? "cmd.exe" : "dotnet";
            string extra = IsWindows ? "/C dotnet " : "";
            Console.WriteLine("Windows: " + IsWindows);
            Console.WriteLine("Using Shell: " + exec);
            return ProcessUtils.RunProcess(exec, $"{extra}build {filepath} -c Release",
                null);
        }

        private static int PublishCommand(string filepath)
        {
            string exec = IsWindows ? "cmd.exe" : "dotnet";
            string extra = IsWindows ? "/C dotnet " : "";
            Console.WriteLine("Windows: " + IsWindows);
            Console.WriteLine("Using Shell: " + exec);
            return ProcessUtils.RunProcess(exec, $"{extra}publish {filepath} -c Release",
                null);
        }

        private static AssetPackageInfo CreatePackageInfo(string memoryFileExts, string unpackedFileExts,
            char separator = '+')
        {
            AssetPackageInfo info = new AssetPackageInfo();
            List<string> unpackExts = unpackedFileExts.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            for (int i = 0; i < unpackExts.Count; i++)
            {
                info.FileInfos.Add(unpackExts[i], new AssetFileInfo {PackageType = AssetPackageType.Unpack});
            }

            List<string> packExts = memoryFileExts.Split("+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            for (int i = 0; i < packExts.Count; i++)
            {
                info.FileInfos.Add(packExts[i], new AssetFileInfo {PackageType = AssetPackageType.Memory});
            }

            return info;
        }

        public static void PackAssets(string outputFolder, int packSize, string memoryFileExts,
            string unpackedFileExts, string assetFolder, bool compression)
        {
            AssetPacker.MaxsizeKilobytes = packSize;

            Console.WriteLine("Parsing File info...");

            AssetPackageInfo info = CreatePackageInfo(memoryFileExts, unpackedFileExts);

            Console.WriteLine("Creating Asset Pack(" + assetFolder + ")...");
            AssetResult ret = AssetPacker.PackAssets(assetFolder, outputFolder, info, compression);
            Console.WriteLine("Packaging " + ret.IndexList.Count + " Assets in " + ret.Packs.Count + " Packs.");

            Console.WriteLine("Saving Asset Pack to " + outputFolder);
            ret.Save();

            Console.WriteLine("Packaging Assets Finished.");
        }

        public static string[] ParseFileList(string fileList, string projectFolder, string projectName,
            bool copyAssetsWhenError, bool copyPacksWhenError, bool isStandalone)
        {
            string[] files;
            if (fileList != null && File.Exists(fileList))
            {
                files = File.ReadAllLines(fileList);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Path.GetFullPath(files[i]);
                }
            }
            else
            {
                Console.WriteLine(
                    "Warning. No Game Package File list. Using /asset /pack folder as well as projectname.dll");
                List<string> f = new List<string>();
                string packFolder = projectFolder + "/packs";
                string assetFolder = projectFolder + "/assets";
                if (Directory.Exists(assetFolder) && copyAssetsWhenError)
                {
                    string[] ff = Directory.GetFiles(assetFolder, "*", SearchOption.AllDirectories);
                    for (int i = 0; i < ff.Length; i++)
                    {
                        if (!f.Contains(ff[i]))
                        {
                            f.Add(ff[i]);
                        }
                    }
                }

                if (Directory.Exists(packFolder) && copyPacksWhenError)
                {
                    string[] ff = Directory.GetFiles(packFolder, "*", SearchOption.AllDirectories);
                    for (int i = 0; i < ff.Length; i++)
                    {
                        if (!f.Contains(ff[i]))
                        {
                            f.Add(ff[i]);
                        }
                    }
                }

                string helper = Path.GetFullPath(projectFolder + "/" + projectName + ".dll");
                if (File.Exists(helper) && !f.Contains(helper))
                {
                    f.Add(helper);
                }

                helper = Path.GetFullPath(projectFolder + "/" + projectName + ".runtimeconfig.json");
                if (File.Exists(helper) && !f.Contains(helper))
                {
                    f.Add(helper);
                }

                helper = Path.GetFullPath(projectFolder + "/" + projectName + ".deps.json");
                if (File.Exists(helper) && !f.Contains(helper))
                {
                    f.Add(helper);
                }

                if (isStandalone)
                {
                    string[] ff = ParseEngineFileList(fileList, projectFolder);
                    for (int i = 0; i < ff.Length; i++)
                    {
                        if (!f.Contains(ff[i]))
                        {
                            f.Add(ff[i]);
                        }
                    }
                }

                files = f.ToArray();
            }

            return files;
        }

        public static string[] ParseEngineFileList(string fileList, string projectFolder)
        {
            string[] files;
            if (fileList != null && File.Exists(fileList))
            {
                files = File.ReadAllLines(fileList);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Path.GetFullPath(files[i]);
                }
            }
            else
            {
                Console.WriteLine(
                    "Warning. No Game Package File list. Using /asset /pack folder as well as projectname.dll");
                List<string> f = new List<string>();

                f.AddRange(Directory.GetFiles(projectFolder + "/runtimes", "*", SearchOption.AllDirectories));
                f.AddRange(Directory.GetFiles(projectFolder, "*.dll", SearchOption.TopDirectoryOnly));

                files = f.ToArray();
            }

            return files;
        }
    }
}