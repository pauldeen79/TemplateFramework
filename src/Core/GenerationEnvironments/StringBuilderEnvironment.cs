namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class StringBuilderEnvironment : GenerationEnvironmentBase
{
    public StringBuilderEnvironment(StringBuilder builder) : this(new FileSystem(), builder)
    {
    }

    internal StringBuilderEnvironment(IFileSystem fileSystem, StringBuilder builder)
        : base(GenerationEnvironmentType.StringBuilder)
    {
        Guard.IsNotNull(builder);

        Builder = builder;
        FileSystem = fileSystem;
    }

    public StringBuilder Builder { get; }
    public IFileSystem FileSystem { get; }

    public override void Process(ICodeGenerationProvider provider, bool dryRun)
    {
        Guard.IsNotNull(provider);

        if (dryRun)
        {
            return;
        }

        FileSystem.WriteAllText(provider.DefaultFilename, Builder.ToString(), provider.Encoding);
    }
}
