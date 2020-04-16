using System;
using System.Collections.Generic;
using System.IO;

namespace Byt3.ObjectPipeline.AssetLoaderFramework
{
    public sealed class AssetLoader<TOut> : PipelineStage<string, TOut>
    {
        private readonly Dictionary<string, AssetTypeLoader<TOut>> typeLoaders =
            new Dictionary<string, AssetTypeLoader<TOut>>();

        private PipelineStage<string, Stream> fileLoader;

        public AssetLoader(PipelineStage<string, Stream> fileLoader = null, params AssetTypeLoader<TOut>[] typeLoaders)
        {
            this.fileLoader = fileLoader ?? new DefaultFileLoader();

            for (int i = 0; i < typeLoaders.Length; i++)
            {
                AddTypeLoader(typeLoaders[i]);
            }
        }

        public void SetFileLoader(PipelineStage<string, Stream> fileLoader)
        {
            this.fileLoader = fileLoader ?? throw new InvalidOperationException("File loader Can not be null");
        }

        public void AddTypeLoader(AssetTypeLoader<TOut> typeLoader)
        {
            typeLoaders[typeLoader.FileExtension] = typeLoader;
        }

        public override TOut Process(string input)
        {
            string ext = Path.GetExtension(input);
            if (typeLoaders.ContainsKey(ext))
            {
                return typeLoaders[ext].Process(fileLoader.Process(input));
            }

            throw new UnknownFileFormatException(input);
        }
    }
}