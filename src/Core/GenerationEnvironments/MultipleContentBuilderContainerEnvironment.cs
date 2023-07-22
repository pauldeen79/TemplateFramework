namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class MultipleContentBuilderContainerEnvironment : GenerationEnvironmentBase
{
    internal MultipleContentBuilderContainerEnvironment(IMultipleContentBuilderContainer builder)
        : base(GenerationEnvironmentType.MultipleContentBuilderContainer)
    {
        Builder = builder;
    }

    public IMultipleContentBuilderContainer Builder { get; }
}
