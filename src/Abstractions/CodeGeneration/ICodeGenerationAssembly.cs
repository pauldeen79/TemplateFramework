namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationAssembly
{
    string Generate(ICodeGenerationAssemblySettings settings);
}
