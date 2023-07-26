namespace TemplateFramework.Core.GenerationEnvironments;

public abstract class GenerationEnvironmentBase : IGenerationEnvironment
{
    protected GenerationEnvironmentBase(GenerationEnvironmentType type)
    {
        Type = type;
    }

    public GenerationEnvironmentType Type { get; }

    public abstract void Process(ICodeGenerationProvider provider, string basePath);

    protected void SaveAll(IFileSystem fileSystem, string basePath, Encoding encoding, IEnumerable<IContent> contents)
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
            Retry(() => fileSystem.WriteAllText(path, normalizedContents, encoding));
        }
    }

    public void SaveLastGeneratedFiles(IFileSystem fileSystem, string basePath, Encoding encoding, string lastGeneratedFilesPath, IEnumerable<IContent> contents)
    {
        Guard.IsNotNull(fileSystem);
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(encoding);
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);

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

    public void DeleteLastGeneratedFiles(IFileSystem fileSystem, string basePath, Encoding encoding, string lastGeneratedFilesPath, bool recurse)
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

    private void DeleteFilesFromLastGeneratedFilesContents(IFileSystem fileSystem, string basePath, string fullPath, Encoding encoding)
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

    private void DeleteFilesUsingPattern(IFileSystem fileSystem, string lastGeneratedFilesPath, bool recurse, string basePath)
    {
        foreach (var filename in GetFiles(fileSystem, basePath, lastGeneratedFilesPath, recurse))
        {
            fileSystem.FileDelete(filename);
        }
    }

    private string[] GetFiles(IFileSystem fileSystem, string basePath, string lastGeneratedFilesPath, bool recurse)
        => fileSystem.GetFiles(basePath, lastGeneratedFilesPath, recurse);

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
