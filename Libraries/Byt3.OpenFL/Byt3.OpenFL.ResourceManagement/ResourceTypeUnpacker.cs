using System.IO;
using Byt3.Utilities.ProgressFeedback;

namespace Byt3.OpenFL.ResourceManagement
{
    public abstract class ResourceTypeUnpacker
    {
        public abstract string UnpackerName { get; }
        public abstract void Unpack(string targetDir, string name, Stream stream, IProgressIndicator progressIndicator);
    }
}