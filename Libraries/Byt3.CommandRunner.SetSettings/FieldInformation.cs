using System.Reflection;

namespace Byt3.CommandRunner.SetSettings
{
    public struct FieldInformation
    {
        public string path;
        public FieldInfo info;
        public object reference;
    }
}