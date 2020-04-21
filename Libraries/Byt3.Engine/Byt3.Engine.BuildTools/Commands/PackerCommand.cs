using System;
using System.IO;
using Byt3.CommandRunner;

namespace Byt3.Engine.BuildTools.Commands
{
    public class PackerCommand : AbstractCommand
    {
        private static void PackAssets(StartupArgumentInfo info, string[] args)
        {
            try
            {
                Byt3.Engine.BuildTools.Builder.PackAssets(Path.GetFullPath(args[0]), int.Parse(args[1]), args[2], args[3],
                    Path.GetFullPath(args[4]), false);
            }
            catch (Exception e)
            {

                throw new ApplicationException("Input Error", e);
            }
        }

        public PackerCommand() : base(PackAssets, new[] { "--pack-assets", "--packer" }, "--packer <outputFolder> <packSize> <fileExtensions> <unpackFileExtensions> <assetFolder>\nPackage the Asset Files", false)
        {

        }
    }
}