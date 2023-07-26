namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class StringBuilderEnvironment : GenerationEnvironmentBase
{
    internal StringBuilderEnvironment(StringBuilder builder, IFileSystem fileSystem)
        : base(GenerationEnvironmentType.StringBuilder)
    {
        Builder = builder;
        FileSystem = fileSystem;
    }

    public StringBuilder Builder { get; }
    public IFileSystem FileSystem { get; }

    public override void Process(ICodeGenerationProvider provider, bool dryRun)
    {
        if (dryRun)
        {
            return;
        }

        FileSystem.WriteAllText(provider.DefaultFilename, Builder.ToString(), provider.Encoding);
    }
}
