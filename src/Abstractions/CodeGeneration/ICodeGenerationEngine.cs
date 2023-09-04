namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    void Generate(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings);
}
