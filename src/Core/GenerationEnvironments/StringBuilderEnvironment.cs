namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class StringBuilderEnvironment : IGenerationEnvironment
{
    public StringBuilderEnvironment()
        : this(new FileSystem(), new StringBuilder())
    {
    }

    public StringBuilderEnvironment(StringBuilder builder)
        : this(new FileSystem(), builder)
    {
    }

    internal StringBuilderEnvironment(IFileSystem fileSystem, StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        _fileSystem = fileSystem;
        Builder = builder;
    }

    private readonly IFileSystem _fileSystem;

    public StringBuilder Builder { get; }

    public GenerationEnvironmentType Type => GenerationEnvironmentType.StringBuilder;

    public void SaveContents(ICodeGenerationProvider provider, string basePath, string defaultFilename)
    {
        Guard.IsNotNull(provider);
        Guard.IsNotNullOrEmpty(defaultFilename);

        var path = string.IsNullOrEmpty(basePath)
            ? defaultFilename
            : Path.Combine(basePath, defaultFilename);

        _fileSystem.WriteAllText(path, Builder.ToString(), provider.Encoding);
    }
}
