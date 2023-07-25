﻿namespace TemplateFramework.Core;

public class MultipleContentBuilder : IMultipleContentBuilder
{
    private readonly IFileSystem _fileSystem;
    private readonly List<IContentBuilder> _contentList;

    public MultipleContentBuilder() : this(new FileSystem(), Encoding.UTF8, string.Empty)
    {
    }

    public MultipleContentBuilder(string basePath) : this(new FileSystem(), Encoding.UTF8, basePath)
    {
    }

    public MultipleContentBuilder(Encoding encoding) : this(new FileSystem(), encoding, string.Empty)
    {
    }

    public MultipleContentBuilder(Encoding encoding, string basePath) : this(new FileSystem(), encoding, basePath)
    {
    }

    public MultipleContentBuilder(IFileSystem fileSystem, Encoding encoding, string basePath)
    {
        Guard.IsNotNull(fileSystem);
        Guard.IsNotNull(encoding);
        Guard.IsNotNull(basePath);
        
        _fileSystem = fileSystem;
        _contentList = new List<IContentBuilder>();
        Encoding = encoding;
        BasePath = basePath;
    }

    public string BasePath { get; set; }

    public Encoding Encoding { get; set; }

    public void SaveAll()
    {
        foreach (var content in _contentList.Select(x => x.Build()))
        {
            var path = string.IsNullOrEmpty(BasePath) || Path.IsPathRooted(content.Filename)
                ? content.Filename
                : Path.Combine(BasePath, content.Filename);

            if (content.SkipWhenFileExists && _fileSystem.FileExists(path))
            {
                continue;
            }

            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !_fileSystem.DirectoryExists(dir))
            {
                _fileSystem.CreateDirectory(dir);
            }

            var contents = content.Contents.NormalizeLineEndings();
            Retry(() => _fileSystem.WriteAllText(path, contents, Encoding));
        }
    }

    public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
    {
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);

        var fullPath = string.IsNullOrEmpty(BasePath) || Path.IsPathRooted(lastGeneratedFilesPath)
            ? lastGeneratedFilesPath
            : Path.Combine(BasePath, lastGeneratedFilesPath);

        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir) && !_fileSystem.DirectoryExists(dir))
        {
            _fileSystem.CreateDirectory(dir);
        }

        if (!fullPath.Contains('*', StringComparison.InvariantCulture))
        {
            _fileSystem.WriteAllLines(fullPath, _contentList.Select(x => x.Build()).OrderBy(c => c.Filename).Select(c => c.Filename), Encoding);
        }
    }

    public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse)
    {
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);

        var basePath = BasePath;
        if (lastGeneratedFilesPath.Contains(Path.DirectorySeparatorChar, StringComparison.InvariantCulture))
        {
            var lastSlash = lastGeneratedFilesPath.LastIndexOf(Path.DirectorySeparatorChar);
            basePath = Path.Combine(basePath, lastGeneratedFilesPath.Substring(0, lastSlash));
            lastGeneratedFilesPath = lastGeneratedFilesPath.Substring(lastSlash + 1);
        }

        var fullPath = GetFullPath(basePath, lastGeneratedFilesPath);

        if (fullPath.Contains('*', StringComparison.InvariantCulture)
            && !string.IsNullOrEmpty(basePath)
            && _fileSystem.DirectoryExists(basePath))
        {
            DeleteFilesUsingPattern(lastGeneratedFilesPath, recurse, basePath);
        }
        else if (!fullPath.Contains('*', StringComparison.InvariantCulture) && _fileSystem.FileExists(fullPath))
        {
            DeleteFilesFromLastGeneratedFilesContents(basePath, fullPath);
        }
    }

    public IContentBuilder AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder)
    {
        Guard.IsNotNull(filename);

        var content = builder is null
            ? new ContentBuilder()
            : new ContentBuilder(builder);

        content.Filename = filename;
        content.SkipWhenFileExists = skipWhenFileExists;

        _contentList.Add(content);

        return content;
    }

    public IEnumerable<IContentBuilder> Contents => _contentList.AsReadOnly();

    public IMultipleContent Build() => new MultipleContent(BasePath, Encoding, Contents.Select(x => x.Build()));

    private void DeleteFilesFromLastGeneratedFilesContents(string basePath, string fullPath)
    {
        foreach (var filename in _fileSystem.ReadAllLines(fullPath, Encoding))
        {
            var fileFullPath = string.IsNullOrEmpty(basePath) || Path.IsPathRooted(filename)
                ? filename
                : Path.Combine(basePath, filename);

            if (_fileSystem.FileExists(fileFullPath))
            {
                _fileSystem.FileDelete(fileFullPath);
            }
        }
    }

    private void DeleteFilesUsingPattern(string lastGeneratedFilesPath, bool recurse, string basePath)
    {
        foreach (var filename in GetFiles(basePath, lastGeneratedFilesPath, recurse))
        {
            _fileSystem.FileDelete(filename);
        }
    }

    private string[] GetFiles(string basePath, string lastGeneratedFilesPath, bool recurse)
        => _fileSystem.GetFiles(basePath, lastGeneratedFilesPath, recurse);

    private static void Retry(Action action)
    {
        for (int i = 1; i <= 3; i++)
        {
            try
            {
                action();
                return;
            }
            catch (IOException x) when (x.Message.Contains("because it is being used by another process", StringComparison.InvariantCulture))
            {
                Thread.Sleep(i * 500);
            }
        }
    }

    private static string GetFullPath(string basePath, string lastGeneratedFilesPath)
        => string.IsNullOrEmpty(basePath) || Path.IsPathRooted(lastGeneratedFilesPath)
            ? lastGeneratedFilesPath
            : Path.Combine(basePath, lastGeneratedFilesPath);
}
