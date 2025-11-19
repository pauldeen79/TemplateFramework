namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    Task<Result> GenerateAsync(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings, CancellationToken token);
}
