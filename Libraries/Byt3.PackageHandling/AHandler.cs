using Byt3.ADL;

namespace Byt3.PackageHandling
{
    public abstract class AHandler : ALoggable<LogType>
    {
        protected AHandler() : base(PackageHandlerDebugConfig.Settings)
        {
        }

        internal abstract void Handle(object objectToHandle, object context);
    }

    public abstract class AHandler<T> : AHandler
    {
        internal override void Handle(object objectToHandle, object context)
        {
            Handle((T) objectToHandle, context);
        }

        public abstract void Handle(T objectToHandle, object context);
    }
}