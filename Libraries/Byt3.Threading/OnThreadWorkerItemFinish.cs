namespace Byt3.Threading
{
    public delegate void OnThreadWorkerItemFinish<in TOut>(TOut result);
    public delegate void OnThreadWorkerItemFinish(object result);
}