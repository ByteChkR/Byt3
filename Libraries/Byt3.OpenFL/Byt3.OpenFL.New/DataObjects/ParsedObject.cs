namespace Byt3.OpenFL.New.DataObjects
{
    public abstract class ParsedObject
    {
        public ParsedSource Root { get; private set; }

        public virtual void SetRoot(ParsedSource root)
        {
            Root = root;
        }
    }
}