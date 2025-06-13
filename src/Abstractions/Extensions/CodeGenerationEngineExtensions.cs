namespace TemplateFramework.Abstractions.Extensions;

public static class CodeGenerationEngineExtensions
{
    public static Task<Result> GenerateAsync(this ICodeGenerationEngine instance, ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings)
        => instance.GenerateAsync(codeGenerationProvider, generationEnvironment, settings, CancellationToken.None);
}
