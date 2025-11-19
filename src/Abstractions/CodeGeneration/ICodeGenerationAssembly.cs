namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationAssembly
{
    Task<Result> GenerateAsync(ICodeGenerationAssemblySettings settings, IGenerationEnvironment generationEnvironment, CancellationToken token);
}
