namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationAssembly
{
    void Generate(ICodeGenerationAssemblySettings settings, IGenerationEnvironment generationEnvironment);
}
