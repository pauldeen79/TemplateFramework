namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    void Generate(ICodeGenerationProvider codeGenerationProvider, ITemplateProvider templateProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings);
}
