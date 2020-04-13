namespace Byt3.OpenCL.Common
{
    /// <summary>
    /// Interface for the TextProcessor IO Callbacks.
    /// </summary>
    public interface IIOCallback
    {
        bool FileExists(string file);
        string[] ReadAllLines(string file);
        string[] GetFiles(string path);
        string[] GetFiles(string path, string searchPattern);
    }
}