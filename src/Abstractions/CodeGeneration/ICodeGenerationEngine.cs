namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    //TODO: Refactor code generation to use Task<Result>
    Task Generate(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings, CancellationToken cancellationToken);
}
