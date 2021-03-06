﻿using System;
using System.IO;
using Byt3.CommandRunner;

namespace Byt3.Engine.BuildTools.Commands
{
    public class PackerCommand : AbstractCommand
    {
        private static void PackAssets(string[] args)
        {
            try
            {
                Builder.PackAssets(Path.GetFullPath(args[0]), int.Parse(args[1]), args[2],
                    args[3],
                    Path.GetFullPath(args[4]), false);
            }
            catch (Exception e)
            {
                throw new BuilderInputException("Input Error", e);
            }
        }

        public PackerCommand() : base(new[] { "--pack-assets", "--packer" },
            "--packer <outputFolder> <packSize> <fileExtensions> <unpackFileExtensions> <assetFolder>\nPackage the Asset Files",
            false)
        {
            CommandAction = (info, strings) => PackAssets(strings);
        }
    }
}