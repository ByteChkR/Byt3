namespace Byt3.PackageHandling
{
    public abstract class AHandler
    {
        internal abstract void Handle(object objectToHandle, object context);
    }

    public abstract class AHandler<T> : AHandler
    {
        internal override void Handle(object objectToHandle, object context)
        {
            Handle((T)objectToHandle, context);
        }

        public abstract void Handle(T objectToHandle, object context);
    }
}