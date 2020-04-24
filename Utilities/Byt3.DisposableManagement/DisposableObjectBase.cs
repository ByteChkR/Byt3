using System;

namespace Byt3.DisposableManagement
{
    public abstract class DisposableObjectBase : IDisposable
    {
        public object HandleIdentifier { get; }

        protected DisposableObjectBase(object handleIdentifier)
        {
            HandleIdentifier = handleIdentifier;

        }


        public virtual void Dispose()
        {
        }



    }
}
