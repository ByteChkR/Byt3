using System.IO;
using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class ClearCacheCommand : AbstractCommand
    {
        private static void ClearCache()
        {
            System.Console.WriteLine("Deleting Engine Cache...");
            if (Directory.Exists(EnginePlayerConsole.EngineDir))
            {
                string[] files = Directory.GetFiles(EnginePlayerConsole.EngineDir, "*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i]);
                }
            }
        }

        public ClearCacheCommand() : base( new[] {"--clear-cache", "-cC"},
            "--clear-cache\nClears all engines in the cache", false)
        {
            CommandAction = (info, strings) => ClearCache();
        }
    }
}