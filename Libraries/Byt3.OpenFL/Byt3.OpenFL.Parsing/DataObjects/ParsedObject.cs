using Byt3.ADL;

namespace Byt3.OpenFL.Parsing.DataObjects
{
    public abstract class ParsedObject : ALoggable<LogType>
    {
        public FLParseResult Root { get; private set; }

        protected ParsedObject() : base(OpenFLDebugConfig.Settings)
        {
        }

        public virtual void SetRoot(FLParseResult root)
        {
            Root = root;
        }
    }
}