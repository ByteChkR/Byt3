using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.Utilities.ConsoleInternals;

namespace Byt3Console.Runner
{
    public class ConsoleRunner
    {
        internal static Dictionary<string, ResolverWrapper> Resolvers { get; set; }

        public static void Run(string[] args, Dictionary<string, object> resolvers)
        {
            if (args.Length == 1 && args[0] == "reload")
            {
                if (File.Exists(ConsolePaths.RunnerConfig))
                {
                    File.Delete(ConsolePaths.RunnerConfig);
                }

                Console.WriteLine("Index File Cleared");
                return;
            }

            Resolvers = resolvers.ToDictionary(pair => pair.Key, pair => new ResolverWrapper(pair.Value));
            ConsolePaths.SetUpPaths();

            ConsoleLibrary lib = ConsoleLibrary.Load(ConsolePaths.RunnerConfig);
            if (lib.CheckConsoles())
            {
                try
                {
                    lib.Save(ConsolePaths.RunnerConfig);
                }
                catch (Exception)
                {
                }
            }

            if (args.Length != 0)
            {
                string[] consoleArgs = new string[args.Length - 1];
                Console.Write("Arguments: ");
                for (int i = 1; i < args.Length; i++)
                {
                    Console.Write(args[i] + " ");
                    consoleArgs[i - 1] = args[i];
                }

                Console.WriteLine();
                ConsoleItem item = lib.GetConsoleItem(args[0]);
                if (item != null)
                {
                    try
                    {
                        if (!item.Run(consoleArgs))
                        {
                            Console.WriteLine($"Exectuion of {args[0]} Failed.");
                            Console.ReadLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        if (e.InnerException != null)
                        {
                            Console.WriteLine("Inner: ");
                            Console.WriteLine(e.InnerException);
                        }

                        throw;
                    }
                }
                else
                {
                    Console.WriteLine("Can not find Console with Key: " + args[0]);
                }
            }
            else
            {
                Console.Write(lib.InstalledConsoles);
            }
        }
    }
}