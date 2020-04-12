using System;
using Byt3.ADL;
using Byt3.ADL.Crash;

namespace Byt3.ExtPP.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //ADL Setup
            Debug.DefaultInitialization();
            CrashHandler.Initialize();

            Core.CLI._Main(args);
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}