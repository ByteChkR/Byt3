namespace Byt3.OpenFL.Parsing.DataObjects
{
    public class UnresolvedFunction
    {
        public string FunctionName { get; }
        public bool External { get; }

        public UnresolvedFunction(string functionName, bool external)
        {
            FunctionName = functionName;
            External = external;
        }
    }
}