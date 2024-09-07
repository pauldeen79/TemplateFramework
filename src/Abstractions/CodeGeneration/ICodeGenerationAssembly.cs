namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationAssembly
{
    Task<Result> Generate(ICodeGenerationAssemblySettings settings, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken);
}
