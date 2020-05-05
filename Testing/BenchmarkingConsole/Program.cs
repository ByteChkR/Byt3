using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.Utilities.IL;

namespace BenchmarkingConsole
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ILExecutionBenchmark>();

            Console.ReadLine();
        }
    }

    [MemoryDiagnoser]
    public class CLBufferCreationBenchmark
    {
        [Params(128 * 128, 256 * 256, 512 * 512, 1024 * 1024)]
        public int BufferSize { get; set; }
        public CLAPI instance = CLAPI.GetInstance();

        

        [Benchmark]
        public void CreateEmptyAlloc()
        {
            MemoryBuffer buf = CLAPI.CreateEmpty<byte>(instance, BufferSize, MemoryFlag.ReadWrite, "TestEmptyAlloc");
            buf.Dispose();
        }

        [Benchmark]
        public void CreateEmptyCopy()
        {
            byte[] data = new byte[BufferSize];
            MemoryBuffer buf = CLAPI.CreateBuffer(instance, data, MemoryFlag.ReadWrite, "TestEmptyAlloc");
            buf.Dispose();
        }
    }

    [MemoryDiagnoser]
    public class ILExecutionBenchmark
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


        [Params(100_000, 10_000_000)]
        public int Iterations { get; set; }
        private Type TestType;
        private ILDelegates.TypeConstructor ctor;
        private ILDelegates.MethodDel fv;
        private ILDelegates.MethodDel getPropVal;
        private ILDelegates.MethodDel_v<string> setPropVal;
        private ILDelegates.MethodDel paramLessDel;
        private ILDelegates.MethodDel_v<int> paramDel;
        private FieldInfo fi;
        private MethodInfo paramLessMi;
        private MethodInfo paramMi;
        private PropertyInfo pi;
        private ILTest instance;

        [GlobalSetup]
        public void Setup()
        {
            TestType = typeof(ILTest);
            ctor = ILTools.GetConstructor<ILDelegates.TypeConstructor>(TestType);
            fi = TestType.GetField("ValueField");
            fv = ILTools.GetFieldValue<ILDelegates.MethodDel>(TestType, fi);
            instance = (ILTest)ctor();
            pi = TestType.GetProperty("PropertyField"); getPropVal = ILTools.GetPropertyGet<ILDelegates.MethodDel>(TestType, pi);
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
            paramMi.Invoke(instance, new object[] { 100 });
        }
    }
}
