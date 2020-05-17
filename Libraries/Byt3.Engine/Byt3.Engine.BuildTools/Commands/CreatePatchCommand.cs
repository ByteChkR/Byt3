using System;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;

namespace Byt3.Engine.BuildTools.Commands
{
    public class CreatePatchCommand : AbstractCommand
    {
        public CreatePatchCommand() : base(new[] {"--create-patch", "-cpatch"},
            "--create-patch <folder> <destinationFile>")
        {
            CommandAction = (a, b) => CreatePatch(b);
        }

        private static void CreatePatch(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ApplicationException("Invalid Input");
            }

            try
            {
                Creator.CreatePatchFromFolder(args[0], args[1]);
            }
            catch (Exception e)
            {
                throw new BuilderInputException("Input Error", e);
            }
        }
    }
}