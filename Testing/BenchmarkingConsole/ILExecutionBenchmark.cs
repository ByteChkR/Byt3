using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Byt3.Utilities.IL;

namespace BenchmarkingConsole
{
    [MemoryDiagnoser]
    public class ILExecutionBenchmark
    {
        private ILDelegates.TypeConstructor ctor;
        private FieldInfo fi;
        private ILDelegates.MethodDel fv;
        private ILDelegates.MethodDel getPropVal;
        private ILTest instance;
        private ILDelegates.MethodDel_v<int> paramDel;
        private ILDelegates.MethodDel paramLessDel;
        private MethodInfo paramLessMi;
        private MethodInfo paramMi;
        private PropertyInfo pi;
        private ILDelegates.MethodDel_v<string> setPropVal;
        private Type TestType;


        [Params(100_000, 10_000_000)] public int Iterations { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            TestType = typeof(ILTest);
            ctor = ILTools.GetConstructor<ILDelegates.TypeConstructor>(TestType);
            fi = TestType.GetField("ValueField");
            fv = ILTools.GetFieldValue<ILDelegates.MethodDel>(TestType, fi);
            instance = (ILTest) ctor();
            pi = TestType.GetProperty("PropertyField");
            getPropVal = ILTools.GetPropertyGet<ILDelegates.MethodDel>(TestType, pi);
            setPropVal = ILTools.GetPropertySet<ILDelegates.MethodDel_v<string>>(TestType, pi);
            paramLessMi = TestType.GetMethod("WriteMessage");
            paramLessDel = ILTools.GetMethodDel<ILDelegates.MethodDel>(TestType, paramLessMi);
            paramMi = TestType.GetMethod("TestMethod");
            paramDel = ILTools.GetMethodDel<ILDelegates.MethodDel_v<int>>(TestType, paramMi);
        }

        [Benchmark]
        public void ActivatorConstructorTest()
        {
            object obj = Activator.CreateInstance(TestType);
        }

        [Benchmark]
        public void ILConstructorTest()
        {
            object obj = ctor();
        }


        [Benchmark]
        public void ILGetFieldValueTest()
        {
            object ret = fv(instance);
        }

        [Benchmark]
        public void ActivatorGetFieldValueTest()
        {
            object ret = fi.GetValue(instance);
        }

        [Benchmark]
        public void ILGetPropertyValue()
        {
            object ret = getPropVal(instance);
        }

        [Benchmark]
        public void ActivatorGetPropertyValue()
        {
            object ret = pi.GetValue(instance);
        }

        [Benchmark]
        public void ILSetPropertyValue()
        {
            setPropVal(instance, "EEEEEEE");
        }

        [Benchmark]
        public void ActivatorSetPropertyValue()
        {
            pi.SetValue(instance, "EEEEEEE");
        }

        [Benchmark]
        public void ILParameterlessFunction()
        {
            object methRet = paramLessDel(instance);
        }

        [Benchmark]
        public void ActivatorParameterlessFunctionCall()
        {
            object methRet = paramLessMi.Invoke(instance, new object[0]);
        }

        [Benchmark]
        public void ILFunction()
        {
            paramDel(instance, 100);
        }

        [Benchmark]
        public void ActivatorFunction()
        {
            paramMi.Invoke(instance, new object[] {100});
        }

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
    }
}