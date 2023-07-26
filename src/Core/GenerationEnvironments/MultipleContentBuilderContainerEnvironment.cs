namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class MultipleContentBuilderContainerEnvironment : GenerationEnvironmentBase
{
    public MultipleContentBuilderContainerEnvironment(IMultipleContentBuilderContainer container)
        : base(GenerationEnvironmentType.MultipleContentBuilderContainer)
    {
        Guard.IsNotNull(container);

        Container = container;
    }

    public IMultipleContentBuilderContainer Container { get; }

    public override void Process(ICodeGenerationProvider provider, bool dryRun)
    {
        Guard.IsNotNull(provider);

        if (dryRun)
        {
            return;
        }

        var builder = Container.MultipleContentBuilder;
        if (!string.IsNullOrEmpty(provider.LastGeneratedFilesFilename))
        {
            var prefixedLastGeneratedFilesFilename = Path.Combine(provider.Path, provider.LastGeneratedFilesFilename);
            builder.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFilename, provider.RecurseOnDeleteGeneratedFiles, provider.Encoding);
            builder.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFilename, provider.Encoding);
        }

        builder.SaveAll(provider.Encoding);
    }
}
