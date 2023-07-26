namespace TemplateFramework.Core.Extensions;

public static class MultipleContentBuilderExtensions
{
    public static IGenerationEnvironment ToGenerationEnvironment(this IMultipleContentBuilder instance) => new MultipleContentBuilderEnvironment(instance);
}
