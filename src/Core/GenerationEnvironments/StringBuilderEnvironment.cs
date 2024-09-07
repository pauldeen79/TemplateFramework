namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class StringBuilderEnvironment : GenerationEnvironmentBase<StringBuilder>
{
    public StringBuilderEnvironment()
        : base(new StringBuilder())
    {
    }

    public StringBuilderEnvironment(StringBuilder builder)
        : base(builder)
    {
    }

    internal StringBuilderEnvironment(IFileSystem fileSystem, IRetryMechanism retryMechanism, StringBuilder builder)
        : base(fileSystem, retryMechanism, builder)
    {
    }

    protected override string Build() => Builder.ToString();
}
