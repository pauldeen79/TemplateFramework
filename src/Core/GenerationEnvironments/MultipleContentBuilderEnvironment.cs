namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class MultipleContentBuilderEnvironment : GenerationEnvironmentBase
{
    public MultipleContentBuilderEnvironment(IMultipleContentBuilder builder)
        : base(GenerationEnvironmentType.MultipleContentBuilder)
    {
        Guard.IsNotNull(builder);

        Builder = builder;
    }

    public IMultipleContentBuilder Builder { get; }

    public override void Process(ICodeGenerationProvider provider, bool dryRun)
    {
        Guard.IsNotNull(provider);

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
