namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class MultipleContentBuilderEnvironment : GenerationEnvironmentBase
{
    internal MultipleContentBuilderEnvironment(IMultipleContentBuilder builder)
        : base(GenerationEnvironmentType.MultipleContentBuilder)
    {
        Builder = builder;
    }

    public IMultipleContentBuilder Builder { get; }
}
