using Byt3.ADL.Configs;

namespace Byt3.ADL
{
    /// <summary>
    /// An empty interface that is used to Log where the log is coming from.
    /// Just add to any script and you can write logs by using this.Log(..)
    /// </summary>
    public abstract class ALoggable<T> where T : struct
    {
        protected readonly ADLLogger<T> Logger;

        protected ALoggable(IProjectDebugConfig settings)
        {
            Logger = new ADLLogger<T>(settings, GetType().Name);
        }
    }
}