namespace Byt3.OpenFL.New.DataObjects
{
    public class FLExecutionContext
    {
        public byte[] ActiveChannels;
        public FLBufferInfo ActiveBuffer;

        public FLExecutionContext(byte[] activeChannels, FLBufferInfo activeBuffer)
        {
            ActiveChannels = activeChannels;
            ActiveBuffer = activeBuffer;
        }
    }
}