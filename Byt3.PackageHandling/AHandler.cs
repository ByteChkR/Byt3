namespace Byt3.PackageHandling
{
    public abstract class AHandler
    {
        internal abstract void Handle(object objectToHandle);
    }

    public abstract class AHandler<T> : AHandler
    {
        internal override void Handle(object objectToHandle)
        {
            Handle((T)objectToHandle);
        }

        public abstract void Handle(T objectToHandle);
    }
}