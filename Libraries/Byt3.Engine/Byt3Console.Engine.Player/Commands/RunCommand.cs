﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.Common;
using Byt3.Engine.BuildTools.PackageCreator;
using Byt3.Engine.BuildTools.PackageCreator.Versions;

namespace Byt3Console.Engine.Player.Commands
{
    public class RunCommand : AbstractCommand
    {
        public static Process _p;


        private static void LoadEngine(string version)
        {
            if (version == "standalone")
            {
                System.Console.WriteLine("Engine is Contained in Game Package. Using engine from there.");
                return;
            }

            string filePath = version;
            if (version.StartsWith("path:") && File.Exists(version.Remove(0, 5)))
            {
                filePath = version.Remove(0, 5);
            }
            else if (!version.StartsWith("path:"))
            {
                if (!EnginePlayer.IsEngineVersionAvailable(version))
                {
                    if (!EnginePlayer.DownloadEngineVersion(version))
                    {
                        System.Console.WriteLine("Could not locate engine version : " + version);
                        System.Console.WriteLine("Finding Compatible..");

                        if (!Version.TryParse(version, out Version v))
                            throw new ArgumentException("Could not parse engine version : " + version);

                        bool foundVersion = false;
                        Version vx = new Version(v.Major, v.Minor, v.Build, 0);
                        for (int i = 0; i < EnginePlayer.AvailableVersionsOnServer.Length; i++)
                        {
                            Version avx = new Version(EnginePlayer.AvailableVersionsOnServer[i].Major,
                                EnginePlayer.AvailableVersionsOnServer[i].Minor, EnginePlayer.AvailableVersionsOnServer[i].Build, 0);
                            if (v != EnginePlayer.AvailableVersionsOnServer[i] && vx == avx)
                            {
                                EnginePlayer.DownloadEngineVersion(EnginePlayer.AvailableVersionsOnServer[i].ToString());
                                version = EnginePlayer.AvailableVersionsOnServer[i].ToString();
                                foundVersion = true;
                                break;
                            }
                        }

                        if (!foundVersion)
                            throw new ArgumentException("Could not locate engine version : " + v);
                    }
                }

                filePath = GetEnginePath(version);
            }


            System.Console.WriteLine("Loading Engine Version: " + version);

            Creator.UnpackPackage(filePath, EnginePlayer.GameDir);
        }


        private static void CreateFolder(string path)
        {
            List<string> s = new List<string>();
            string curpath = Path.GetDirectoryName(path);
            System.Console.WriteLine("Path: " + curpath);
            do
            {
                s.Add(curpath);
                curpath = Path.GetDirectoryName(curpath);
            } while (curpath != null && curpath.Trim() != "");

            s.Reverse();
            for (int i = 0; i < s.Count; i++)
            {
                if (!Directory.Exists(s[i]))
                {
                    Directory.CreateDirectory(s[i]);
                }
            }
        }
        private static string GetEnginePath(string version)
        {
            return EnginePlayer.EngineDir + "/" + version + ".engine";
        }

        private static void LoadGame(string gamePath, IPackageManifest pm)
        {
            //Load Game
            Creator.UnpackPackage(gamePath, EnginePlayer.GameTempDir);
            string[] files = Directory.GetFiles(EnginePlayer.GameTempDir, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                bool over = File.Exists(file.Replace(EnginePlayer.GameTempDir, EnginePlayer.GameDir));

                CreateFolder(file.Replace(EnginePlayer.GameTempDir, EnginePlayer.GameDir));

                File.Copy(file, file.Replace(EnginePlayer.GameTempDir, EnginePlayer.GameDir), over);
            }

            Directory.Delete(EnginePlayer.GameTempDir, true);
        }

        private static void SetUpDirectoryStructure()
        {
            if (Directory.Exists(EnginePlayer.GameDir))
            {
                Directory.Delete(EnginePlayer.GameDir, true);
            }

            if (Directory.Exists(EnginePlayer.GameTempDir))
            {
                Directory.Delete(EnginePlayer.GameTempDir, true);
            }

            DirectoryInfo di = Directory.CreateDirectory(EnginePlayer.GameDir);
            DirectoryInfo tempUnpackForGame = Directory.CreateDirectory(EnginePlayer.GameTempDir);
            if ((di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
                //Add Hidden flag    
                di.Attributes |= FileAttributes.Hidden;
            }

            if ((tempUnpackForGame.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
                //Add Hidden flag    
                tempUnpackForGame.Attributes |= FileAttributes.Hidden;
            }
        }

        public static void Run(StartupArgumentInfo info, string[] args)
        {
            if (args.Length == 0 || !args[0].EndsWith(".game") || !File.Exists(args[0]))
            {
                System.Console.WriteLine("Could not load Game File: " + (args.Length == 0 ? "" : Path.GetFullPath(args[0])));
                return;
            }

            SetUpDirectoryStructure();


            IPackageManifest pm = null;
            try
            {
                pm = Creator.ReadManifest(args[0]);
                System.Console.Title = pm.Title;
                string path = pm.Version;
                if (EnginePlayer.EngineVersion != null)
                {
                    path = EnginePlayer.EngineVersion;
                }

                LoadEngine(path);
                //Load Game
                LoadGame(args[0], pm);
            }
            catch (Exception e)
            {
                Directory.Delete(EnginePlayer.GameTempDir, true);
                Directory.Delete(EnginePlayer.GameDir, true);
                System.Console.WriteLine("Error Unpacking File.");
                System.Console.WriteLine(e);
            }

            string startCommand = "-c \"" + pm.StartCommand + "\"";
            string cli = "/bin/bash";
            if (Type.GetType("Mono.Runtime") == null) //Not Running Mono = Windows, Running Mono = Linux
            {
                startCommand = "/C " + pm.StartCommand;
                cli = "cmd.exe";
            }

            _p = new Process();

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = cli;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.Arguments = startCommand;
            psi.WorkingDirectory = EnginePlayer.GameDir;
            _p.StartInfo = psi;
            _p.Start();

            ConsoleRedirector crd =
                ConsoleRedirector.CreateRedirector(_p.StandardOutput, _p.StandardError, _p, System.Console.WriteLine);

            crd.StartThreads();

            while (!_p.HasExited)
            {
                Thread.Sleep(150);
            }

            crd.StopThreads();
            System.Console.WriteLine(crd.GetRemainingLogs());
            Directory.Delete(EnginePlayer.GameDir, true);


        }

        public RunCommand() : base(Run, new[] { "--run" }, "--run <Path/To/File.game>", false)
        {

        }
    }
}