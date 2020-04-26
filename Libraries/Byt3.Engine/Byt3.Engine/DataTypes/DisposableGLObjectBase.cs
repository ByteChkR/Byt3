using Byt3.DisposableManagement;
using Byt3.Engine.Debug;

namespace Byt3.Engine.DataTypes
{
    public abstract class DisposableGLObjectBase : DisposableObjectBase
    {
        private bool IsCopy { get; }
        protected DisposableGLObjectBase(bool isCopy, object handleIdentifier) : base(handleIdentifier)
        {
            IsCopy = isCopy;
            if (!isCopy)
                EngineStatisticsManager.GlObjectCreated(this);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (!IsCopy)
                EngineStatisticsManager.GlObjectDestroyed(this);
        }
    }
}