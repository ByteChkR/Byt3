using System;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;

namespace Byt3.Engine.BuildTools.Commands
{
    public class CreatePatchDeltaCommand : AbstractCommand
    {
        public CreatePatchDeltaCommand() : base(new[] {"--create-patch-delta", "-cdpatch"},
            "--create-patch-delta <oldFile> <newFile> <destinationFile>")
        {
            CommandAction = (info, strings) => CreatePatchDelta(strings);
        }

        private static void CreatePatchDelta(string[] args)
        {
            if (args.Length != 3)
            {
                throw new BuilderInputException("Invalid Input");
            }

            try
            {
                Creator.CreatePatchFromDelta(args[0], args[1], args[2]);
            }
            catch (Exception e)
            {
                throw new BuilderInputException("Input Error", e);
            }
        }
    }
}