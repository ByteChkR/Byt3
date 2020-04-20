using System.Collections.Generic;

namespace Byt3.OpenFL.Console.Commands
{
    public struct FieldInformations
    {
        public Dictionary<string, FieldInformation> Fields;

        public FieldInformations(Dictionary<string, FieldInformation> fields)
        {
            Fields = fields;
        }
    }
}