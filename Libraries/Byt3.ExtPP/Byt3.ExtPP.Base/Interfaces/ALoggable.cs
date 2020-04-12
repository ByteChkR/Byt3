using Byt3.ADL;

namespace Byt3.ExtPP.Base.Interfaces
{
    /// <summary>
    /// An empty interface that is used to Log where the log is coming from.
    /// Just add to any script and you can write logs by using this.Log(..)
    /// </summary>
    public abstract class ALoggable<T> where T : struct
    {
        protected readonly LevelFilteredLogger<T> Logger;

        protected ALoggable()
        {
            Logger = new LevelFilteredLogger<T>(GetType().Name);
        }
    }
}