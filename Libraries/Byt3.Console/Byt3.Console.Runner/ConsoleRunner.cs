using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Byt3.Utilities.Console.Internals;

namespace Byt3.Console.Runner
{


    public class ConsoleRunner
    {
        

        internal static Dictionary<string, ResolverWrapper> Resolvers { get; set; }

        public static void Run(string[] args, Dictionary<string, object> resolvers)
        {
            Resolvers = resolvers.ToDictionary(pair => pair.Key, pair => new ResolverWrapper(pair.Value));
            ConsolePaths.SetUpPaths();

            ConsoleLibrary lib = ConsoleLibrary.Load(ConsolePaths.RunnerConfig);
            if (lib.CheckConsoles())
            {
                try
                {
                    lib.Save(ConsolePaths.RunnerConfig);
                }
                catch (Exception e)
                {
                }
            }

            if (args.Length != 0)
            {
                string[] consoleArgs = new string[args.Length - 1];
                System.Console.Write("Arguments: ");
                for (int i = 1; i < args.Length; i++)
                {
                    System.Console.Write(args[i] + " ");
                    consoleArgs[i - 1] = args[i];
                }

                System.Console.WriteLine();
                ConsoleItem item = lib.GetConsoleItem(args[0]);
                if (item != null)
                {
                    if (!item.Run(consoleArgs))
                    {
                        System.Console.WriteLine($"Exectuion of {args[0]} Failed.");
                        System.Console.ReadLine();
                    }
                }
                else
                {
                    System.Console.WriteLine("Can not find Console with Key: " + args[0]);
                }
            }
            else
            {
                System.Console.Write(lib.InstalledConsoles);
            }
        }
    }
}