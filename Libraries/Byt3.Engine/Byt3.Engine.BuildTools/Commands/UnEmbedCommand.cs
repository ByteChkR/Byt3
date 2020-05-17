using System;
using System.IO;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.Common;

namespace Byt3.Engine.BuildTools.Commands
{
    public class UnEmbedCommand : AbstractCommand
    {
        public UnEmbedCommand() : base(new[] {"--unembed", "-u"},
            "--unembed <Path/To/CSProj/File>\nUnembeds that were embedded into the .csproj file of the game project.")
        {
            CommandAction = (info, strings) => UnembedFiles(strings);
        }

        private static void UnembedFiles(string[] args)
        {
            try
            {
                AssemblyEmbedder.UnEmbedFilesFromProject(Path.GetFullPath(args[0]));
            }
            catch (Exception e)
            {
                throw new BuilderInputException("Input Error", e);
            }
        }
    }
}