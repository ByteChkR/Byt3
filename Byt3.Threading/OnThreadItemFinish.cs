namespace Byt3.Threading
{
    public delegate void OnThreadItemFinish<in TOut>(TOut result);
    public delegate void OnThreadItemFinish(object result);
}