using System.IO;

namespace Byt3.ObjectPipeline.AssetLoaderFramework
{
    public class DefaultFileLoader : PipelineStage<string, Stream>
    {
        public override Stream Process(string input)
        {
            return File.Open(input, FileMode.Open);
        }
    }
}