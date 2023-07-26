namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class MultipleContentBuilderContainerEnvironment : GenerationEnvironmentBase
{
    internal MultipleContentBuilderContainerEnvironment(IMultipleContentBuilderContainer builder)
        : base(GenerationEnvironmentType.MultipleContentBuilderContainer)
    {
        Container = builder;
    }

    public IMultipleContentBuilderContainer Container { get; }

    public override void Process(ICodeGenerationProvider provider, bool dryRun)
    {
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
