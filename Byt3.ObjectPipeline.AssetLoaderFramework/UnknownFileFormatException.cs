using System;

namespace Byt3.ObjectPipeline.AssetLoaderFramework
{
    public class UnknownFileFormatException : ApplicationException
    {
        public readonly string FilePath;

        public UnknownFileFormatException(string filePath)
        {
            FilePath = filePath;
        }
    }
}