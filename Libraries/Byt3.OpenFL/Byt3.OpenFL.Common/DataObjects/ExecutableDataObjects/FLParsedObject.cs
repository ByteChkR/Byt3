using Byt3.ADL;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public abstract class FLParsedObject : ALoggable<LogType>, IParsedObject
    {
        protected FLParsedObject() : base(OpenFLDebugConfig.Settings)
        {
        }

        public FLProgram Root { get; private set; }

        public virtual void SetRoot(FLProgram root)
        {
            Root = root;
        }
    }
}