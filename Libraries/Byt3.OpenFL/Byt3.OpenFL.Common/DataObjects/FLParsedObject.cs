using Byt3.ADL;

namespace Byt3.OpenFL.Common.DataObjects
{
    public abstract class FLParsedObject : ALoggable<LogType>
    {
        public FLProgram Root { get; private set; }

        protected FLParsedObject() : base(OpenFLDebugConfig.Settings)
        {
        }

        public virtual void SetRoot(FLProgram root)
        {
            Root = root;
        }
    }
}