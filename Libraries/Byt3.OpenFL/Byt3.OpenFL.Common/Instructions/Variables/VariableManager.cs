using System.Collections.Generic;
using Byt3.Utilities.Exceptions;

namespace Byt3.OpenFL.Common.Instructions.Variables
{
    public class VariableManager<T>
    {
        private readonly Dictionary<string, T> Variables;
        private readonly VariableManager<T> Parent;

        public VariableManager()
        {
            Variables = new Dictionary<string, T>();
        }

        public VariableManager(Dictionary<string, T> variables)
        {
            Variables = variables;
        }

        private VariableManager(VariableManager<T> parent) : this()
        {
            Parent = parent;
        }

        public void AddGlobal(string varName, T value)
        {
            if (Parent != null)
            {
                Parent.AddGlobal(varName, value);
            }
            else
            {
                AddVariable(varName, value);
            }
        }

        public void AddVariable(string varName, T value)
        {
            Variables.Add(varName, value);
        }

        public bool IsDefined(string varName)
        {
            return IsDefinedLocal(varName) || Parent != null && Parent.IsDefined(varName);
        }

        public bool IsDefinedLocal(string varName)
        {
            return Variables.ContainsKey(varName);
        }

        public void ChangeVariable(string varName, T value)
        {
            if (Variables.ContainsKey(varName))
            {
                Variables[varName] = value;
                return;
            }

            if (Parent != null)
            {
                Parent.ChangeVariable(varName, value);
                return;
            }

            throw new Byt3Exception("Changing variable failed. Can not find variable.");
        }

        public T GetVariable(string varName)
        {
            if (Variables.ContainsKey(varName))
            {
                return Variables[varName];
            }

            if (Parent != null)
            {
                return Parent.GetVariable(varName);
            }

            throw new Byt3Exception("Variable Not defined");
        }

        public Dictionary<string, T>.Enumerator GetEnumerator()
        {
            return Variables.GetEnumerator();
        }

        public VariableManager<T> AddScope()
        {
            return new VariableManager<T>(this);
        }
    }
}