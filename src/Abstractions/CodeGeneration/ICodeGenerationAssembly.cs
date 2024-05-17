namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationAssembly
{
    Task Generate(ICodeGenerationAssemblySettings settings, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken);
}
