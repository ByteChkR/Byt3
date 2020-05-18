namespace Byt3.OpenFL.Common.Buffers
{
    public interface IEditableBuffer
    {
        int DataSize { get; }
        void SetData(byte[] data);
        byte[] GetData();
    }
}