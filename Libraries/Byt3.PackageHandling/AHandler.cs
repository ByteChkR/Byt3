using Byt3.ADL;

namespace Byt3.PackageHandling
{
    public abstract class AHandler
    {
        protected readonly ALogger<LogType> Logger;

        protected AHandler()
        {
            Logger= new ALogger<LogType>("Handler: "+ GetType().Name);
        }

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