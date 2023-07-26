namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class MultipleContentBuilderEnvironment : GenerationEnvironmentBase
{
    public MultipleContentBuilderEnvironment(IMultipleContentBuilder builder)
        : this(new FileSystem(), builder)
    {
    }

    internal MultipleContentBuilderEnvironment(IFileSystem fileSystem, IMultipleContentBuilder builder)
        : base(GenerationEnvironmentType.MultipleContentBuilder)
    {
        Guard.IsNotNull(builder);

        _fileSystem = fileSystem;
        Builder = builder;
    }

    private readonly IFileSystem _fileSystem;

    public IMultipleContentBuilder Builder { get; }

    public override void Process(ICodeGenerationProvider provider, string basePath)
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
    }
}
