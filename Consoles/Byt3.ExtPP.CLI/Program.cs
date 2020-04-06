using System;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Streams;
using Byt3.ExtPP.CLI.Core;
using Byt3.ExtPP.Plugins;

namespace Byt3.ExtPP.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            

            Assembly asm= Assembly.GetAssembly(typeof(ChainCollection));

            Core.CLI._Main(args);
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
