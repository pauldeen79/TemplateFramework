namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    void Generate(ICodeGenerationProvider provider, IMultipleContentBuilder generationEnvironment, ICodeGenerationSettings settings);
}
