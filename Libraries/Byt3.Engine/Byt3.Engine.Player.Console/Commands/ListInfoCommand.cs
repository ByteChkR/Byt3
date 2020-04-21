using System.IO;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;
using Byt3.Engine.BuildTools.PackageCreator.Versions;

namespace Byt3.Engine.Player.Console.Commands
{
    public class ListInfoCommand : AbstractCommand
    {



        private static void ListInfo(StartupArgumentInfo info, string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]) || !args[0].EndsWith(".game") && !args[0].EndsWith(".engine"))
            {
                System.Console.WriteLine("Could not find file");
                return;
            }

            IPackageManifest pm = Creator.ReadManifest(args[0]);
            System.Console.WriteLine(pm);
        }
        public ListInfoCommand() : base(ListInfo, new[] { "--list-info", "-l" }, "--list-info <<Path/To/File>\nLists Information about the .engine or .game file.", false)
        {

        }
    }
}