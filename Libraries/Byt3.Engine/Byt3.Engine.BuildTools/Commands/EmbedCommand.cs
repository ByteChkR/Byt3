using System;
using System.IO;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.Common;

namespace Byt3.Engine.BuildTools.Commands
{
    public class EmbedCommand : AbstractCommand
    {
        private static void EmbedFiles(string[] args)
        {
            try
            {
                string[] files = Directory.GetFiles(Path.GetFullPath(args[1]), "*", SearchOption.AllDirectories);
                AssemblyEmbedder.EmbedFilesIntoProject(Path.GetFullPath(args[0]), files);
            }
            catch (Exception e)
            {
                throw new BuilderInputException("Input Error", e);
            }
        }

        public EmbedCommand() : base( new[] {"--embed", "-e"},
            "--embed <Path/To/CSProj/File> <Folder/To/Embed>\nEmbeds the files in the specified folder into the .csproj file of the game project.",
            false)
        {
            CommandAction = (info, strings) => EmbedFiles(strings);
        }
    }
}