namespace TemplateFramework.Core.GenerationEnvironments;

public abstract class GenerationEnvironmentBase<T> : IGenerationEnvironment
{
    protected GenerationEnvironmentBase(T builder)
        : this(new FileSystem(), new RetryMechanism(), builder)
    {
    }

    protected GenerationEnvironmentBase(IFileSystem fileSystem, IRetryMechanism retryMechanism, T builder)
    {
        Guard.IsNotNull(builder);

        _fileSystem = fileSystem;
        _retryMechanism = retryMechanism;
        Builder = builder;
    }

    private readonly IFileSystem _fileSystem;
    private readonly IRetryMechanism _retryMechanism;

    public T Builder { get; }

    public Task<Result> SaveContentsAsync(ICodeGenerationProvider provider, string basePath, string defaultFilename, CancellationToken cancellationToken)
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

        var normalizedContents = Build().NormalizeLineEndings();
        _retryMechanism.Retry(() => _fileSystem.WriteAllText(path, normalizedContents, provider.Encoding));

        return Task.FromResult(Result.Success());
    }

    protected abstract string Build();
}
