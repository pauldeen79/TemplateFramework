namespace TemplateFramework.Core.GenerationEnvironments;

public class MultipleStringContentBuilderEnvironment : MultipleContentBuilderEnvironment<StringBuilder>
{
    public MultipleStringContentBuilderEnvironment() : base(new MultipleContentBuilder())
    {
    }
}

public class MultipleContentBuilderEnvironment<T> : IGenerationEnvironment where T : class, new()
{
    public MultipleContentBuilderEnvironment(IMultipleContentBuilder<T> builder)
        : this(new FileSystem(), new RetryMechanism(), builder)
    {
    }

    internal MultipleContentBuilderEnvironment(IFileSystem fileSystem, IRetryMechanism retryMechanism, IMultipleContentBuilder<T> builder)
    {
        Guard.IsNotNull(builder);

        _fileSystem = fileSystem;
        _retryMechanism = retryMechanism;
        Builder = builder;
    }

    private readonly IFileSystem _fileSystem;
    private readonly IRetryMechanism _retryMechanism;

    public IMultipleContentBuilder<T> Builder { get; }

    public Task SaveContents(ICodeGenerationProvider provider, string basePath, string defaultFilename, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(provider);

        var multipleContent = Builder.Build();
        if (!string.IsNullOrEmpty(provider.LastGeneratedFilesFilename))
        {
            var prefixedLastGeneratedFilesFilename = Path.Combine(provider.Path, provider.LastGeneratedFilesFilename);
            DeleteLastGeneratedFiles(_fileSystem, basePath, provider.Encoding, prefixedLastGeneratedFilesFilename, provider.RecurseOnDeleteGeneratedFiles);
            SaveLastGeneratedFiles(_fileSystem, basePath, provider.Encoding, prefixedLastGeneratedFilesFilename, multipleContent.Contents);
        }

        SaveAll(_fileSystem, basePath, provider.Encoding, multipleContent.Contents);

        return Task.CompletedTask;
    }

    internal void SaveAll(IFileSystem fileSystem, string basePath, Encoding encoding, IEnumerable<IContent> contents)
    {
        Guard.IsNotNull(fileSystem);
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(encoding);
        Guard.IsNotNull(contents);

        foreach (var content in contents)
        {
            var path = string.IsNullOrEmpty(basePath) || Path.IsPathRooted(content.Filename)
                ? content.Filename
                : Path.Combine(basePath, content.Filename);

            if (content.SkipWhenFileExists && fileSystem.FileExists(path))
            {
                continue;
            }

            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !fileSystem.DirectoryExists(dir))
            {
                fileSystem.CreateDirectory(dir);
            }

            var normalizedContents = content.Contents.NormalizeLineEndings();
            _retryMechanism.Retry(() => fileSystem.WriteAllText(path, normalizedContents, encoding));
        }
    }

    internal void SaveLastGeneratedFiles(IFileSystem fileSystem, string basePath, Encoding encoding, string lastGeneratedFilesPath, IEnumerable<IContent> contents)
    {
        Guard.IsNotNull(fileSystem);
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(encoding);
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);
        Guard.IsNotNull(contents);

        var fullPath = string.IsNullOrEmpty(basePath) || Path.IsPathRooted(lastGeneratedFilesPath)
            ? lastGeneratedFilesPath
            : Path.Combine(basePath, lastGeneratedFilesPath);

        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir) && !fileSystem.DirectoryExists(dir))
        {
            fileSystem.CreateDirectory(dir);
        }

        if (!fullPath.Contains('*', StringComparison.InvariantCulture))
        {
            fileSystem.WriteAllLines(fullPath, contents.OrderBy(c => c.Filename).Select(c => c.Filename), encoding);
        }
    }

    internal void DeleteLastGeneratedFiles(IFileSystem fileSystem, string basePath, Encoding encoding, string lastGeneratedFilesPath, bool recurse)
    {
        Guard.IsNotNull(fileSystem);
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(encoding);
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);

        if (lastGeneratedFilesPath.Contains(Path.DirectorySeparatorChar, StringComparison.InvariantCulture))
        {
            var lastSlash = lastGeneratedFilesPath.LastIndexOf(Path.DirectorySeparatorChar);
            basePath = Path.Combine(basePath, lastGeneratedFilesPath.Substring(0, lastSlash));
            lastGeneratedFilesPath = lastGeneratedFilesPath.Substring(lastSlash + 1);
        }

        var fullPath = GetFullPath(basePath, lastGeneratedFilesPath);

        if (fullPath.Contains('*', StringComparison.InvariantCulture)
            && !string.IsNullOrEmpty(basePath)
            && fileSystem.DirectoryExists(basePath))
        {
            DeleteFilesUsingPattern(fileSystem, lastGeneratedFilesPath, recurse, basePath);
        }
        else if (!fullPath.Contains('*', StringComparison.InvariantCulture) && fileSystem.FileExists(fullPath))
        {
            DeleteFilesFromLastGeneratedFilesContents(fileSystem, basePath, fullPath, encoding);
        }
    }

    private static void DeleteFilesFromLastGeneratedFilesContents(IFileSystem fileSystem, string basePath, string fullPath, Encoding encoding)
    {
        foreach (var filename in fileSystem.ReadAllLines(fullPath, encoding))
        {
            var fileFullPath = string.IsNullOrEmpty(basePath) || Path.IsPathRooted(filename)
                ? filename
                : Path.Combine(basePath, filename);

            if (fileSystem.FileExists(fileFullPath))
            {
                fileSystem.FileDelete(fileFullPath);
            }
        }
    }

    private static void DeleteFilesUsingPattern(IFileSystem fileSystem, string lastGeneratedFilesPath, bool recurse, string basePath)
    {
        foreach (var filename in GetFiles(fileSystem, basePath, lastGeneratedFilesPath, recurse))
        {
            fileSystem.FileDelete(filename);
        }
    }

    private static string[] GetFiles(IFileSystem fileSystem, string basePath, string lastGeneratedFilesPath, bool recurse)
        => fileSystem.GetFiles(basePath, lastGeneratedFilesPath, recurse);

    private static string GetFullPath(string basePath, string lastGeneratedFilesPath)
        => string.IsNullOrEmpty(basePath) || Path.IsPathRooted(lastGeneratedFilesPath)
            ? lastGeneratedFilesPath
            : Path.Combine(basePath, lastGeneratedFilesPath);
}
