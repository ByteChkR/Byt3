using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.AssemblyGenerator;
using Byt3.CommandRunner;
using Byt3.Engine.Debug;
using Byt3.Engine.Demos;
using Byt3.Engine.Tutorials;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.FastString;
using Byt3.Utilities.IL;
using Byt3Console.AssemblyGenerator;
using Byt3Console.Engine.BuildTools;
using Byt3Console.Engine.Player;
using Byt3Console.ExtPP;
using Byt3Console.OpenFL;
using Byt3Console.OpenFL.Benchmarks;
using Byt3Console.OpenFL.ScriptGenerator;
using Byt3Console.VersionHelper;
using TestingProjectConsole;
using Debug = Byt3.ADL.Debug;

namespace TestingProject
{
    internal static class Program
    {
        internal static bool Exit;

        private static readonly Dictionary<string, AConsole> Consoles = new Dictionary<string, AConsole>
        {
            {"testing", new TestingConsole()},
            {"flbench", new FLBenchmarkConsole()},
            {"flgen", new GenerateFLScriptsConsole()},
            {"asmgen", new AssemblyGeneratorConsole()},
            {"ebuild", new EngineBuilderConsole()},
            {"eplay", new EnginePlayerConsole()},
            {"tutorials", new TutorialConsoleStarter()},
            {"demos", new DemoConsoleStarter()},
            {"extpp", new ExtPPConsole()},
            {"vh", new VersionHelperConsole()},
            {"fl", new FLConsole()},
        };

        public class ILTest
        {
            public string ValueField = "LOL";
            public string PropertyField { get; set; } = "Prop";

            public string WriteMessage()
            {
                return "";
            }

            public void TestMethod(int farg)
            {

            }
        }

        public static void ILConstructorTest(int iters)
        {
            Type type = typeof(ILTest);
            ILDelegates.TypeConstructor ctor = ILTools.GetConstructor<ILDelegates.TypeConstructor>(type);

            object test = ctor();
            if (!(test is ILTest))
            {
                throw new Exception();
            }

            RunTest("Constructor", "Reflection", "IL",
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        object obj = Activator.CreateInstance(type);
                    }
                },
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        object obj = ctor();
                    }
                }
            );
        }

        private static void RunTest(string name, string testAName, string testBName, Action testA, Action testB)
        {
            Console.WriteLine($"{name} Tests:");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            testA?.Invoke();

            sw.Stop();
            Console.WriteLine($"\tResult {testAName}: {sw.ElapsedMilliseconds}ms");
            sw.Reset();
            sw.Start();

            testB?.Invoke();

            sw.Stop();
            Console.WriteLine($"\tResult {testBName}: {sw.ElapsedMilliseconds}ms");
        }

        public static void ILTests(int iters)
        {
            ILConstructorTest(iters);


            Type type = typeof(ILTest);
            ILDelegates.TypeConstructor ctor = ILTools.GetConstructor<ILDelegates.TypeConstructor>(type);
            FieldInfo fi = type.GetField("ValueField");
            PropertyInfo pi = type.GetProperty("PropertyField");
            MethodInfo mi = type.GetMethod("WriteMessage");
            MethodInfo testMeth = type.GetMethod("TestMethod");



            ILTest obj = (ILTest)ctor();
            obj.ValueField = "123";
            obj.PropertyField = "456";

            ILDelegates.MethodDel fv = ILTools.GetFieldValue<ILDelegates.MethodDel>(type, fi);
            object fieldSanityCheck = fi.GetValue(obj);
            if (fieldSanityCheck is string str)
            {
                if (str != "123") throw new Exception();
            }

            RunTest("Getting Field Value", "Reflection", "IL",
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        object ret = fi.GetValue(obj);
                    }
                },
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        object ret = fv(obj);
                    }
                });


            ILDelegates.MethodDel getPF = ILTools.GetPropertyGet<ILDelegates.MethodDel>(type, pi);

            object getPropertySanityCheck = getPF(obj);
            if (getPropertySanityCheck is string propertyValue)
            {
                if (propertyValue != "456") throw new Exception();
            }

            RunTest("Get Property Value", "Reflection", "IL",
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        object ret = pi.GetValue(obj);
                    }
                },
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        object ret = getPF(obj);
                    }
                });



            ILDelegates.MethodDel_v<string> setPF = ILTools.GetPropertySet<ILDelegates.MethodDel_v<string>>(type, pi);

            setPF(obj, "EEEEEEE");
            if (obj.PropertyField != "EEEEEEE")
            {
                throw new Exception();
            }


            RunTest("Set Property Value", "Reflection", "IL",
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        pi.SetValue(obj, "EEEEEEE");
                    }
                },
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        setPF(obj, "EEEEEEE");
                    }
                });

            ILDelegates.MethodDel write = ILTools.GetMethodDel<ILDelegates.MethodDel>(type, mi);
            object funcCallSanityCheck = write(obj);
            if (funcCallSanityCheck is string funcRet)
            {
                if (funcRet != "") throw new Exception();
            }

            RunTest("Function Call", "Reflection", "IL",
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        object methRet = mi.Invoke(obj, new object[0]);
                    }
                },
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        object methRet = write(obj);
                    }
                });


            ILDelegates.MethodDel_v<int> func = ILTools.GetMethodDel<ILDelegates.MethodDel_v<int>>(type, testMeth);

            RunTest("Function Call with Parameter", "Reflection", "IL",
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        testMeth.Invoke(obj, new object[] { 100 });
                    }
                },
                () =>
                {
                    for (int i = 0; i < iters; i++)
                    {
                        func(obj, 100);
                    }
                });


        }

        private static void Main(string[] args)
        {

            int[] iterations = new[] { 100, 100_000, 1_000_000, 10_000_000, 100_000_000 };

            for (int i = 0; i < iterations.Length; i++)
            {
                Console.WriteLine("ITERATIONS: " + iterations[i]);
                ILTests(iterations[i]);
            }
            return;
            Console.ReadLine();

            Debug.DefaultInitialization();

            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            AssemblyGeneratorDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            EngineDebugConfig.Settings.MinSeverity = Verbosity.Level1;

            ADLLogger<LogType> logger = new ADLLogger<LogType>(new ProjectDebugConfig("Testing Project", -1, 10,
                PrefixLookupSettings.AddPrefixIfAvailable));


            logger.Log(LogType.Log, "Available Consoles: " + Consoles.Keys.Unpack(", "), 1);
            logger.Log(LogType.Log, "Type \"exit\" to exit", 1);

            if (args.Length != 0)
            {
                if (Consoles.ContainsKey(args[0]))
                {
                    string[] arg = new string[args.Length - 1];

                    for (int i = 1; i < args.Length; i++)
                    {
                        arg[i - 1] = args[i];
                    }

                    Consoles[args[0]].Run(arg);

                    Runner.RemoveAllCommands();

                    return;
                }
            }

            while (true)
            {
                Console.Write("root>");
                string line = Console.ReadLine();
                string[] command = line.Split(new[] { ' ' });
                if (command[0] == "exit")
                {
                    break;
                }

                if (Consoles.ContainsKey(command[0]))
                {
                    string[] arg = new string[command.Length - 1];

                    for (int i = 1; i < command.Length; i++)
                    {
                        arg[i - 1] = command[i];
                    }

                    Consoles[command[0]].Run(arg);

                    Runner.RemoveAllCommands();
                }

                if (Exit)
                {
                    break;
                }
            }
        }
    }
}