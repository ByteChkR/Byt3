using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Byt3.Callbacks;
using Byt3.CommandRunner;
using Byt3.Utilities.ConsoleInternals;

namespace Byt3Console.Engine.Player
{
    public class EnginePlayerConsole : AConsole
    {
        public override string ConsoleKey => "eplay";

        public override string ConsoleTitle => "Engine Player";

        public override bool Run(string[] args)
        {
            Wc.DownloadFileCompleted += WcOnDownloadFileCompleted;


            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            if (!Directory.Exists(EngineDir))
            {
                Directory.CreateDirectory(EngineDir);
            }

            EngineVersions = Directory.GetFiles(EngineDir, "*.engine", SearchOption.TopDirectoryOnly)
                .Select(x => Path.GetFileNameWithoutExtension(x)).ToList();


            Runner.AddAssembly(Assembly.GetExecutingAssembly());
            bool ret = Runner.RunCommands(args);
            ConsoleReadLine();
            return ret;
        }

        public static List<string> EngineVersions = new List<string>();
        public static bool ReadLine = true;
        public static WebClient Wc = new WebClient();

        public static string EngineVersion;

        public static string PlayerPath =
            Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);

        public static string EngineDir = Path.Combine(PlayerPath, "engine");
        public static string GameTempDir = Path.Combine(PlayerPath, "_game");
        public static string GameDir = Path.Combine(PlayerPath, "game");

        private static Version[] _availableVersionsOnServer = null;

        public static Version[] AvailableVersionsOnServer
        {
            get
            {
                if (_availableVersionsOnServer == null)
                {
                    _availableVersionsOnServer = GetEngineServerVersion();
                }

                return _availableVersionsOnServer;
            }
        }


        private static void ConsoleReadLine()
        {
            if (ReadLine)
            {
                Console.WriteLine("Press Enter to Continue...");
                Console.ReadLine();
            }
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private static Version[] GetEngineServerVersion()
        {
            Console.WriteLine("Downloading Version List..");
            string s = Wc.DownloadString("http://213.109.162.193/apps/EngineArchives/version.list");

            string[] versions = s.Split('\n');
            List<Version> versionList = new List<Version>();
            for (int i = 0; i < versions.Length; i++)
            {
                if (Version.TryParse(versions[i], out Version parsedResult))
                {
                    versionList.Add(parsedResult);
                }
            }

            versionList.Sort();
            Console.WriteLine("Fetched Version from Server");
            return versionList.ToArray();
        }


        public static bool DownloadEngineVersion(string version)
        {
            bool ret = IsVersionUrlCorrect(version);
            if (ret)
            {
                Console.WriteLine("Downloading Version: " + version);
                Wc.DownloadFile(new Uri("http://213.109.162.193/apps/EngineArchives/" + version + ".engine"),
                    EngineDir + "/" + version + ".engine");
            }

            return ret;
        }

        private static void WcOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("");
            Console.WriteLine("Success: " + !e.Cancelled);
        }


        public static bool IsEngineVersionAvailable(string version)
        {
            return IOManager.FileExists(EngineDir + "/" + version + ".engine");
        }


        private static bool IsVersionUrlCorrect(string version)
        {
            string addr = $"http://213.109.162.193/apps/EngineArchives/{version}.engine";
            HttpWebResponse response = null;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(addr);
            request.Method = "HEAD";

            bool ret = false;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
                ret = true;
            }
            catch (Exception)
            {
                ret = false;
            }
            finally
            {
                // Don't forget to close your response.
                if (response != null)
                {
                    response.Close();
                    ret = true;
                }
            }

            return ret;
        }
    }
}