namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class StringBuilderEnvironment : GenerationEnvironmentBase
{
    internal StringBuilderEnvironment(StringBuilder builder)
        : base(GenerationEnvironmentType.StringBuilder)
    {
        Builder = builder;
    }

    public StringBuilder Builder { get; }
}
