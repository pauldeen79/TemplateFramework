namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class StringBuilderEnvironment : IGenerationEnvironment
{
    public StringBuilderEnvironment()
        : this(new FileSystem(), new RetryMechanism(), new StringBuilder())
    {
    }

    public StringBuilderEnvironment(StringBuilder builder)
        : this(new FileSystem(), new RetryMechanism(), builder)
    {
    }

    internal StringBuilderEnvironment(IFileSystem fileSystem, IRetryMechanism retryMechanism, StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        _fileSystem = fileSystem;
        _retryMechanism = retryMechanism;
        Builder = builder;
    }

    private readonly IFileSystem _fileSystem;
    private readonly IRetryMechanism _retryMechanism;

    public StringBuilder Builder { get; }

    public GenerationEnvironmentType Type => GenerationEnvironmentType.StringBuilder;

    public Task SaveContents(ICodeGenerationProvider provider, string basePath, string defaultFilename, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(provider);
        Guard.IsNotNullOrEmpty(defaultFilename);

        var path = string.IsNullOrEmpty(basePath)
            ? defaultFilename
            : Path.Combine(basePath, defaultFilename);

        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !_fileSystem.DirectoryExists(dir))
        {
            _fileSystem.CreateDirectory(dir);
        }

        var normalizedContents = Builder.ToString().NormalizeLineEndings();
        _retryMechanism.Retry(() => _fileSystem.WriteAllText(path, normalizedContents, provider.Encoding));

        return Task.CompletedTask;
    }
}
