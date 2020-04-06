using System;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Crash;
using Byt3.ExtPP.Plugins;

namespace Byt3.ExtPP.CLI
{
    class Program
    {
        static void Main(string[] args)
        {

            //ADL Setup
            Debug.DefaultInitialization();
            CrashHandler.Initialize();


            Assembly asm= Assembly.GetAssembly(typeof(ChainCollection));

            Core.CLI._Main(args);
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
