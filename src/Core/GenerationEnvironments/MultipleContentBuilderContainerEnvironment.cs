namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class MultipleContentBuilderContainerEnvironment : GenerationEnvironmentBase
{
    public MultipleContentBuilderContainerEnvironment(IMultipleContentBuilderContainer container)
        : this(container, new FileSystem())
    {
    }

    internal MultipleContentBuilderContainerEnvironment(IMultipleContentBuilderContainer container, IFileSystem fileSystem)
        : base(GenerationEnvironmentType.MultipleContentBuilderContainer)
    {
        Guard.IsNotNull(container);

        _fileSystem = fileSystem;
        Container = container;
    }

    private readonly IFileSystem _fileSystem;

    public IMultipleContentBuilderContainer Container { get; }

    public override void Process(ICodeGenerationProvider provider, string basePath)
    {
        Guard.IsNotNull(provider);

        var multipleContent = Container.MultipleContentBuilder.Build();
        if (!string.IsNullOrEmpty(provider.LastGeneratedFilesFilename))
        {
            var prefixedLastGeneratedFilesFilename = Path.Combine(provider.Path, provider.LastGeneratedFilesFilename);
            DeleteLastGeneratedFiles(_fileSystem, basePath, provider.Encoding, prefixedLastGeneratedFilesFilename, provider.RecurseOnDeleteGeneratedFiles);
            SaveLastGeneratedFiles(_fileSystem, basePath, provider.Encoding, prefixedLastGeneratedFilesFilename, multipleContent.Contents);
        }

        SaveAll(_fileSystem, basePath, provider.Encoding, multipleContent.Contents);
    }
}
