namespace Byt3.OpenFL.Parsing.DataObjects
{
    public abstract class ParsedObject
    {
        public FLParseResult Root { get; private set; }

        public virtual void SetRoot(FLParseResult root)
        {
            Root = root;
        }
    }
}