using System.Collections.Generic;
using System.Linq;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.ElementModifiers
{
    public abstract class FLElementModifiers
    {
        protected readonly List<string> Modifiers;

        public FLElementModifiers(string elementName, IEnumerable<string> modifiers)
        {
            ElementName = elementName;
            Modifiers = new List<string>();
            foreach (string modifier in modifiers)
            {
                if (!Modifiers.Contains(modifier))
                {
                    Modifiers.Add(modifier);
                }
            }

            InternalValidate();
        }

        public string ElementName { get; }
        protected abstract string[] ValidKeywords { get; }


        public List<string> GetModifiers()
        {
            return new List<string>(Modifiers);
        }

        private void InternalValidate()
        {
            for (int i = 0; i < Modifiers.Count; i++)
            {
                if (!ValidKeywords.Contains(Modifiers[i]))
                {
                    throw new FLInvalidFLElementModifierUseException(ElementName, Modifiers[i],
                        "This modifier is not valid on item");
                }
            }


            Validate();
        }

        protected abstract void Validate();

        public override string ToString()
        {
            return Modifiers.Unpack(" ");
        }
    }
}