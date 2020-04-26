using System;

namespace Byt3.Engine.Core
{
    /// <summary>
    /// A Custom Attribute that is used to save and load the variable dynamically with reflection
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConfigVariable : Attribute
    {
    }
}