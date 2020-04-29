using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Byt3.Utilities.Benchmarking;
using NUnit.Framework;

namespace Byt3.Utilities.IL.Tests
{

    public class ReflectionTests
    {
        public class ILTest
        {
            public string ValueField = "123";
            public string PropertyField { get; set; } = "456";

            public string WriteMessage()
            {
                return "";
            }

            public void TestMethod(int farg)
            {

            }
        }


        private static string RunTest(string testName, Action test, int iterations)
        {
            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            BenchmarkHelper.RunNumber = iterations;
            BenchmarkHelper helper = new BenchmarkHelper(testName);
            logOut.AppendLine("Running Iterations: " + iterations);


            logOut.AppendLine($"------------------------Run {testName} Starting------------------------");

            PerformanceTester.PerformanceResult result = PerformanceTester.Tester.RunTest(testName, iterations,
                null,
                (int its) => { test(); },
                null);
            logOut.AppendLine("\t" + result);
            logOut.AppendLine($"------------------------Run {testName} Finished------------------------");
            helper.WriteResult(result);
            return logOut.ToString();

        }

        public static string RunBenchmark(string testName, string testAName, string testBName, Action testA, Action testB)
        {

            StringBuilder logOut = new StringBuilder($"Performance Tests: {DateTime.Now:HH:mm:ss}\n");
            int[] iterations = new int[100];

            int step = 10000;
            int current = step;
            for (int i = 0; i < iterations.Length; i++)
            {
                iterations[i] = current;
                current = iterations[i] + step;
            }


            for (int i = 0; i < iterations.Length; i++)
            {
                string itTestName = testName + "+";
                logOut.AppendLine("Running Iteration: " + iterations[i]);


                logOut.AppendLine($"------------------------Run {testAName} Starting------------------------");

                RunTest(itTestName + testAName, testA, iterations[i]);

                logOut.AppendLine($"------------------------Run {testBName} Starting------------------------");

                RunTest(itTestName + testBName, testB, iterations[i]);

            }


            logOut.AppendLine();
            return logOut.ToString();
        }

        [Test]
        public void ILConstructorTest()
        {
            Type type = typeof(ILTest);
            ILDelegates.TypeConstructor ctor = ILTools.GetConstructor<ILDelegates.TypeConstructor>(type);

            object test = ctor();
            if (!(test is ILTest))
            {
                throw new Exception();
            }

            RunBenchmark("Constructor", "Reflection", "IL", () =>
            {
                object obj = Activator.CreateInstance(type);
            }, () =>
            {
                object obj = ctor();
            });
        }

        [Test]
        public void GetFieldValueTest()
        {
            ILTest obj = new ILTest();
            Type type = typeof(ILTest);
            FieldInfo fi = type.GetField("ValueField");


            ILDelegates.MethodDel fv = ILTools.GetFieldValue<ILDelegates.MethodDel>(type, fi);
            object fieldSanityCheck = fi.GetValue(obj);
            if (fieldSanityCheck is string str)
            {
                if (str != "123") throw new Exception();
            }

            RunBenchmark("Getting Field Value", "Reflection", "IL",
                () =>
                {
                    object ret = fi.GetValue(obj);
                },
                () =>
                {
                    object ret = fv(obj);
                });

        }

        [Test]
        public void GetPropertyValue()
        {
            ILTest obj = new ILTest();
            Type type = typeof(ILTest);
            PropertyInfo pi = type.GetProperty("PropertyField");
            ILDelegates.MethodDel getPF = ILTools.GetPropertyGet<ILDelegates.MethodDel>(type, pi);


            object getPropertySanityCheck = getPF(obj);
            if (getPropertySanityCheck is string propertyValue)
            {
                if (propertyValue != "456") throw new Exception();
            }

            RunBenchmark("Get Property Value", "Reflection", "IL",
                () =>
                {
                    object ret = pi.GetValue(obj);
                },
                () =>
                {
                    object ret = getPF(obj);
                });

        }

        [Test]
        public void SetPropertyValue()
        {
            ILTest obj = new ILTest();
            Type type = typeof(ILTest);
            PropertyInfo pi = type.GetProperty("PropertyField");


            ILDelegates.MethodDel_v<string> setPF = ILTools.GetPropertySet<ILDelegates.MethodDel_v<string>>(type, pi);

            setPF(obj, "EEEEEEE");
            if (obj.PropertyField != "EEEEEEE")
            {
                throw new Exception();
            }


            RunBenchmark("Set Property Value", "Reflection", "IL",
                () =>
                {
                    pi.SetValue(obj, "EEEEEEE");
                },
                () =>
                {
                    setPF(obj, "EEEEEEE");
                });

        }

        [Test]
        public void ParameterlessFunctionCall()
        {
            ILTest obj = new ILTest();
            Type type = typeof(ILTest);


            MethodInfo mi = type.GetMethod("WriteMessage");
            ILDelegates.MethodDel write = ILTools.GetMethodDel<ILDelegates.MethodDel>(type, mi);
            object funcCallSanityCheck = write(obj);
            if (funcCallSanityCheck is string funcRet)
            {
                if (funcRet != "") throw new Exception();
            }

            RunBenchmark("Function Call", "Reflection", "IL",
                () =>
                {
                    object methRet = mi.Invoke(obj, new object[0]);
                },
                () =>
                {
                    object methRet = write(obj);
                });


        }

        [Test]
        public void FunctionCall()
        {
            ILTest obj = new ILTest();
            Type type = typeof(ILTest);
            MethodInfo testMeth = type.GetMethod("TestMethod");



            ILDelegates.MethodDel_v<int> func = ILTools.GetMethodDel<ILDelegates.MethodDel_v<int>>(type, testMeth);

            RunBenchmark("Function Call with Parameter", "Reflection", "IL",
                () =>
                {
                    testMeth.Invoke(obj, new object[] { 100 });
                },
                () =>
                {
                    func(obj, 100);
                });


        }

    }
}