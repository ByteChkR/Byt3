namespace Byt3.OpenFL.Parsing.DataObjects
{
    public class FLExecutionContext
    {
        public byte[] ActiveChannels { get; }
        public FLBufferInfo ActiveBuffer { get; }

        public FLExecutionContext(byte[] activeChannels, FLBufferInfo activeBuffer)
        {
            ActiveChannels = activeChannels;
            ActiveBuffer = activeBuffer;
        }
    }
}