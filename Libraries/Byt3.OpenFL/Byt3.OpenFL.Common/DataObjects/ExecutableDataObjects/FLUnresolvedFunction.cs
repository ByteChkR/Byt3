﻿namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class FLUnresolvedFunction
    {
        public string FunctionName { get; }
        public bool External { get; }

        public FLUnresolvedFunction(string functionName, bool external)
        {
            FunctionName = functionName;
            External = external;
        }
    }
}