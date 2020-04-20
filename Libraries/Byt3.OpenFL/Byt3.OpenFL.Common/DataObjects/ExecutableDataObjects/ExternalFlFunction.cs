using System.Collections.Generic;
using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class ExternalFlFunction : FLFunction
    {
        private readonly FLProgram ExternalFunction;

        public ExternalFlFunction(string name, FLProgram external) : base(name, new List<FLInstruction>())
        {
            ExternalFunction = external;
        }

        public override void Process()
        {
            ExternalFunction.ActiveChannels = Root.ActiveChannels;
            ExternalFunction.SetCLVariables(Root.Instance, Root.ActiveBuffer);
            ExternalFunction.EntryPoint.Process();
            FLBuffer buf = ExternalFunction.ActiveBuffer;

            buf.SetRoot(Root);
            buf.SetKey("dynamicresult_" + Name);
            Root.ActiveChannels = ExternalFunction.ActiveChannels;
            Root.ActiveBuffer = ExternalFunction.ActiveBuffer;
        }
    }
}