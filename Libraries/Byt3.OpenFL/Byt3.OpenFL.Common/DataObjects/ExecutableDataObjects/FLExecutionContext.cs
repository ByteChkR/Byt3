using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class FLExecutionContext
    {
        public byte[] ActiveChannels { get; }
        public FLBuffer ActiveBuffer { get; }

        public FLExecutionContext(byte[] activeChannels, FLBuffer activeBuffer)
        {
            ActiveChannels = activeChannels;
            ActiveBuffer = activeBuffer;
        }
    }
}