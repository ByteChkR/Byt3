using System;
using System.IO;

namespace Byt3.ObjectPipeline.AssetLoaderFramework
{
    /// <summary>
    /// Loads a specific file format from an input Stream
    /// </summary>
    public abstract class AssetTypeLoader<TOut> : PipelineStage<Stream, TOut>
    {
        public abstract string FileExtension { get; }
    }
}
