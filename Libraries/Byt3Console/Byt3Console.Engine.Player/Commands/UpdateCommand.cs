using System.Diagnostics;
using System.IO;
using System.Net;
using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class UpdateCommand : AbstractCommand
    {
        private static void Update()
        {
            WebClient wc = new WebClient();
            System.Console.WriteLine("Updating Build Tools...");
            string destination = Path.GetTempFileName() + ".exe";
            wc.DownloadFile(@"http://213.109.162.193/apps/Installer.exe", destination);
            Process.Start(destination, "--silent --pid " + Process.GetCurrentProcess().Id);
            wc.Dispose();
            System.Console.WriteLine("Update Downloaded. Update will be applied when application exits.");
        }

        public UpdateCommand() : base( new[] {"--update"}, "--update Updates the Build Tools", false)
        {
            CommandAction = (info, strings) => Update();
        }
    }
}