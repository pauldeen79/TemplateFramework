namespace TemplateFramework.Core.Infrastructure;

[ExcludeFromCodeCoverage]
internal sealed class FileSystem : IFileSystem
{
    public void CreateDirectory(string path)
        => Directory.CreateDirectory(path);

    public bool DirectoryExists(string path)
        => Directory.Exists(path);

    public void FileDelete(string path)
        => File.Delete(path);

    public bool FileExists(string path)
        => File.Exists(path);

    public string[] GetFiles(string path, string searchPattern, bool recurse)
        => Directory.GetFiles(path, searchPattern, GetSearchOption(recurse));

    public string[] ReadAllLines(string path, Encoding encoding)
        => File.ReadAllLines(path, encoding);

    public string ReadAllText(string path, Encoding encoding)
        => File.ReadAllText(path, encoding);

    public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        => File.WriteAllLines(path, contents, encoding);

    public void WriteAllText(string path, string contents, Encoding encoding)
        => File.WriteAllText(path, contents, encoding);

    private static SearchOption GetSearchOption(bool recurse)
        => recurse
            ? SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;

    public DateTime GetFileLastWriteTime(string fileName)
        => new FileInfo(fileName).LastWriteTime;
}
