namespace Byt3.ExtPP.Base
{
    public interface IFileContent
    {
        bool TryGetLines(out string[] lines);
        string GetKey();
        void SetKey(string key);
        string GetFilePath();
        bool HasValidFilepath { get; }

    }
}