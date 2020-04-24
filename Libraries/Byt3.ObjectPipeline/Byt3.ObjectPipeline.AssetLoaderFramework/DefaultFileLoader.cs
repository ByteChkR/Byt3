using System.IO;
using Byt3.Callbacks;

namespace Byt3.ObjectPipeline.AssetLoaderFramework
{
    public class DefaultFileLoader : PipelineStage<string, Stream>
    {
        public override Stream Process(string input)
        {
            return IOManager.GetStream(input);
        }
    }
}