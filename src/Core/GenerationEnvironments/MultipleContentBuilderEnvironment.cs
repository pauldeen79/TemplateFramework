namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class MultipleContentBuilderEnvironment : GenerationEnvironmentBase
{
    internal MultipleContentBuilderEnvironment(IMultipleContentBuilder builder)
        : base(GenerationEnvironmentType.MultipleContentBuilder)
    {
        Builder = builder;
    }

    public IMultipleContentBuilder Builder { get; }

    public override void Process(ICodeGenerationProvider provider, bool dryRun)
    {
        if (dryRun)
        {
            return;
        }

        if (!string.IsNullOrEmpty(provider.LastGeneratedFilesFilename))
        {
            var prefixedLastGeneratedFilesFilename = Path.Combine(provider.Path, provider.LastGeneratedFilesFilename);
            Builder.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFilename, provider.RecurseOnDeleteGeneratedFiles, provider.Encoding);
            Builder.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFilename, provider.Encoding);
        }

        Builder.SaveAll(provider.Encoding);
    }
}
