namespace Byt3.OpenFL.Parsing.DataObjects
{
    public class UnresolvedFunction
    {
        public string FunctionName;
        public bool External;

        public UnresolvedFunction(string functionName, bool external)
        {
            FunctionName = functionName;
            External = external;
        }
    }
}