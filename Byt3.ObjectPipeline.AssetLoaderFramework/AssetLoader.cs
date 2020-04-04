using System;
using System.Collections.Generic;
using System.IO;

namespace Byt3.ObjectPipeline.AssetLoaderFramework
{
    public sealed class AssetLoader<TOut> : PipelineStage<string, TOut>
    {
        private readonly Dictionary<string, AssetTypeLoader<TOut>> TypeLoaders = new Dictionary<string, AssetTypeLoader<TOut>>();
        private PipelineStage<string, Stream> FileLoader;

        public AssetLoader(PipelineStage<string, Stream> fileLoader = null, params AssetTypeLoader<TOut>[] typeLoaders)
        {
            FileLoader = fileLoader ?? new DefaultFileLoader();

            for (int i = 0; i < typeLoaders.Length; i++)
            {
                AddTypeLoader(typeLoaders[i]);
            }
        }

        public void SetFileLoader(Pipeline<string, Stream> fileLoader)
        {
            FileLoader = fileLoader ?? throw new InvalidOperationException("File loader Can not be null");
        }

        public void AddTypeLoader(AssetTypeLoader<TOut> typeLoader)
        {
            TypeLoaders[typeLoader.FileExtension] = typeLoader;
        }

        public override TOut Process(string input)
        {
            string ext = Path.GetExtension(input);
            if (TypeLoaders.ContainsKey(ext))
                return TypeLoaders[ext].Process(FileLoader.Process(input));

            throw new UnknownFileFormatException(input);
        }
    }
}