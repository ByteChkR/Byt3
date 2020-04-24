using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class DownloadEngineCommand : AbstractCommand
    {

        private static void DownloadEngine(StartupArgumentInfo info, string[] args)
        {
            if (!EnginePlayer.IsEngineVersionAvailable(args[0]))
            {
                System.Console.WriteLine("Downloading Version " + args[0]);
                EnginePlayer.DownloadEngineVersion(args[0]);
            }
            else
            {
                System.Console.WriteLine("Engine Version not available. Skipping Download.");
            }
        }

        public DownloadEngineCommand() : base(DownloadEngine, new[] { "--download-engine", "-d" }, "--download-engine <Version>\n Tries to download a specified engine version", false)
        {

        }
    }
}