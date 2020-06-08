﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Byt3.AutoUpdate
{
    internal static class Program
    {
        public static string[] Args;
        public static string TargetURL { get; private set; }
        public static Version CurrentVersion { get; private set; }
        public static string DestinationFile { get; private set; }
        public static string ProjectName { get; private set; }
        public static string DestinationFolder => Path.GetDirectoryName(DestinationFile);
        public static Process WaitProcess { get; private set; }
        public static Version TargetVersion { get; private set; }
        public static bool Direct;

        public static bool IsInDestinationFolder =>
            Assembly.GetExecutingAssembly().Location.StartsWith(Path.GetFullPath(DestinationFolder));

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                if (args[0] == "-direct")
                {
                    Direct = true;
                    TargetURL = args[1];
                    ProjectName = args[2];
                    CurrentVersion = Version.Parse(args[3]);
                    DestinationFile = Path.GetFullPath(args[4]);
                    int pid = int.Parse(args[5]);
                    TargetVersion = Version.Parse(args[6]);
                    Args = args.Reverse().Take(Math.Max(0, args.Length - 7)).Reverse().ToArray();
                    WaitProcess = Process.GetProcessById(pid);
                }
                else
                {
                    TargetURL = args[0];
                    ProjectName = args[1];
                    CurrentVersion = Version.Parse(args[2]);
                    DestinationFile = Path.GetFullPath(args[3]);
                    int pid = int.Parse(args[4]);
                    Args = args.Reverse().Take(Math.Max(0, args.Length - 5)).Reverse().ToArray();
                    WaitProcess = Process.GetProcessById(pid);
                }
            }
            catch (Exception e)
            {
                if (args.Length < 4)
                {
                    MessageBox.Show("Do not start the updater manually.");
                    return;
                }
            }

            //if (IsInDestinationFolder)
            //{
            //    string tmpFile = Path.Combine(Path.GetTempPath(), "tempinstaller.exe");
            //    File.Copy(Assembly.GetExecutingAssembly().Location, tmpFile, true);

            //    Process.Start(tmpFile, args.Unpack(" "));
            //    return;
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UpdateWindow());
        }
    }
}