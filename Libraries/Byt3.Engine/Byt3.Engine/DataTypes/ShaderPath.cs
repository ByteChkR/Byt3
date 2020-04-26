using System;
using OpenTK.Graphics.OpenGL;

namespace Byt3.Engine.DataTypes
{
    /// <summary>
    /// A Struct used for linking a file to a specific shader type
    /// </summary>
    [Serializable]
    internal struct ShaderPath
    {
        public ShaderType Type;
        public string Path;
    }
}