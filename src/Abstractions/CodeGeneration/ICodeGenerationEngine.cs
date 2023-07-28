namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    void Generate(ICodeGenerationProvider provider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings);
}
