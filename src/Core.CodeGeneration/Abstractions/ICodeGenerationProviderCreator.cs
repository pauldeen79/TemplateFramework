namespace TemplateFramework.Core.CodeGeneration.Abstractions;

public interface ICodeGenerationProviderCreator
{
    ICodeGenerationProvider? TryCreateInstance(Type type);
}
