namespace Byt3.Console.Runner
{
    public class ConsoleRunner
    {
        public static void Run(string[] args)
        {
            ConsolePaths.SetUpPaths();

            ConsoleLibrary lib = ConsoleLibrary.Load(ConsolePaths.RunnerConfig);
           if(lib.CheckConsoles())
           {
               lib.Save(ConsolePaths.RunnerConfig);
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