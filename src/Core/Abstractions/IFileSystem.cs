namespace TemplateFramework.Core.Abstractions;

internal interface IFileSystem
{
    bool FileExists(string path);
    void FileDelete(string path);

    bool DirectoryExists(string path);
    void CreateDirectory(string path);

    string[] GetFiles(string path, string searchPattern, bool recurse);

    string ReadAllText(string path, Encoding encoding);
    string[] ReadAllLines(string path, Encoding encoding);

    void WriteAllText(string path, string contents, Encoding encoding);
    void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);
}
