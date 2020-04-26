using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    internal class InstructionArgumentSignature
    {
        public List<InstructionArgumentCategory> Signature;

        public bool Compare(List<InstructionArgumentCategory> sig)
        {
            if (sig.Count != Signature.Count)
            {
                return false;
            }

            bool matches = true;

            for (int i = 0; i < sig.Count; i++)
            {
                matches &= (sig[i] & Signature[i]) != 0;
            }

            return matches;
        }

        public override string ToString()
        {
            if (Signature.Count == 0)
            {
                return "NoSignature";
            }

            string ret = "";
            for (int i = 0; i < Signature.Count; i++)
            {
                ret += Signature[i] + "; ";
            }

            return ret;
        }
    }
}