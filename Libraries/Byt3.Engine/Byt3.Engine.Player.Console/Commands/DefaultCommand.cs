﻿using System.IO;
using Byt3.CommandRunner;

namespace Byt3.Engine.Player.Console.Commands
{
    public class DefaultCommand :AbstractCommand
    {
        private static void Default(StartupArgumentInfo info, string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                System.Console.WriteLine("Drag a file onto the executable, or specify the path in the command line.");
                HelpCommand.Help(info, args);
            }
            else if (args[0].EndsWith(".engine"))
            {
                AddEngineCommand.AddEngine(info, args);
            }
            else if (args[0].EndsWith(".game"))
            {
                RunCommand.Run(info, args);
            }
        }

        public DefaultCommand() : base(Default, new[] {"--default"},
            "Default command.\nExecutes the --run Command when a game is passed\nExecutes the --add-engine command when an engine is passed",
            true)
        {

        }
    }
}