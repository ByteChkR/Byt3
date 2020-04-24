namespace Byt3.ExtPP.Base.Interfaces
{
    public interface IFileContent
    {
        bool TryGetLines(out string[] lines);
        string GetKey();
        void SetKey(string key);
        string GetFilePath();
        string GetDefinedName();
        bool HasValidFilepath { get; }
    }
}