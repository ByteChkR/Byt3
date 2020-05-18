using System;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    public class SerializeArrayElementArgumentVariableIndex : SerializeArrayElementArgument
    {
        private string Index;
        public SerializeArrayElementArgumentVariableIndex(string value, string index)
        {
            Index = index;
            Value = value;
        }

        public string Value { get; }

        public override string Identifier => Value + $"[{Index}]";

        public override ImplicitCastBox GetValue(FLProgram script,FLFunction func)
        {
            if (script.DefinedBuffers[Value] is IEditableBuffer buffer)
            {
                return new ImplicitCastBox<decimal>(() =>
                    func.Variables.IsDefined(Index) ? buffer.GetData()[(int)func.Variables.GetVariable(Index)] : 0); //really slow
            }
            throw new InvalidOperationException($"{script.DefinedBuffers[Value]} does not implement IEditableBuffer");
        }
        


        public override string ToString()
        {
            return Identifier;
        }
    }
}