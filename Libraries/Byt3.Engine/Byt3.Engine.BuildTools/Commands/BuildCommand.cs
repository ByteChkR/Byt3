﻿using System;
using System.IO;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.Common;

namespace Byt3.Engine.BuildTools.Commands
{
    public class BuildCommand : AbstractCommand
    {
        private static void Build(string[] args)
        {
            try
            {
                string projectFolder = Path.GetDirectoryName(Path.GetFullPath(args[0]));
                string publishFolder = projectFolder + "/bin/Release/netcoreapp2.1/publish";
                Console.WriteLine(publishFolder);
                if (Directory.Exists(projectFolder + "/bin"))
                {
                    Console.WriteLine("Deleting publish folder to prevent copying the wrong assemblies.");
                    Directory.Delete(projectFolder + "/bin", true);
                }

                if (Directory.Exists(projectFolder + "/obj"))
                {
                    Console.WriteLine("Deleting publish folder to prevent copying the wrong assemblies.");
                    Directory.Delete(projectFolder + "/obj", true);
                }

                Builder.BuildProject(args[0]);


                //Making sure that the root path Path is existing
                IoUtils.CreateDirectoryPath(Path.GetFullPath(args[1]));
                Directory.Delete(Path.GetFullPath(args[1]), true);

                Directory.Move(publishFolder, Path.GetFullPath(args[1]));
                string[] debugFiles =
                    Directory.GetFiles(Path.GetFullPath(args[1]), "*.pdb", SearchOption.AllDirectories);
                for (int i = 0; i < debugFiles.Length; i++)
                {
                    File.Delete(debugFiles[i]);
                }
            }
            catch (Exception e)
            {
                throw new BuilderInputException("Input Error", e);
            }
        }

        public BuildCommand() : base( new[] {"--build", "-b"},
            "--build <Path/To/CSProj/File> <OutputDirectory>\nBuilds the Specified csproj file and moves all output to the output folder.",
            false)
        {
            CommandAction = (info, strings) => Build(strings);
        }
    }
}