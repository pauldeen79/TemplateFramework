namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    void Generate(ICodeGenerationProvider provider, ITemplateFileManager templateFileManager, ICodeGenerationSettings settings);
}
