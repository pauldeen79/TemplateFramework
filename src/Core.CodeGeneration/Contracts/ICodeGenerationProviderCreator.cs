namespace TemplateFramework.Core.CodeGeneration.Contracts;

public interface ICodeGenerationProviderCreator
{
    ICodeGenerationProvider? TryCreateInstance(Type type);
}
