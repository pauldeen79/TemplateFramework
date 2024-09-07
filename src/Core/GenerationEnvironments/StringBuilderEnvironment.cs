namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class StringBuilderEnvironment : GenerationEnvironmentBase<StringBuilder>
{
    public StringBuilderEnvironment()
        : base(new FileSystem(), new RetryMechanism(), new StringBuilder())
    {
    }

    public StringBuilderEnvironment(StringBuilder builder)
        : base(new FileSystem(), new RetryMechanism(), builder)
    {
    }

    internal StringBuilderEnvironment(IFileSystem fileSystem, IRetryMechanism retryMechanism, StringBuilder builder)
        : base(fileSystem, retryMechanism, builder)
    {
    }

    protected override string Build() => Builder.ToString();
}
