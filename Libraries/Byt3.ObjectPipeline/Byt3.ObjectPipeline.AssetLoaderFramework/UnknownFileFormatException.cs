using System;
using Byt3.Utilities.Exceptions;

namespace Byt3.ObjectPipeline.AssetLoaderFramework
{
    public class UnknownFileFormatException : Byt3Exception
    {
        public UnknownFileFormatException(string filePath) : base("Unknown File format: " + filePath)
        {
        }
    }
}