using BenchmarkDotNet.Attributes;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;

namespace BenchmarkingConsole
{
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
}