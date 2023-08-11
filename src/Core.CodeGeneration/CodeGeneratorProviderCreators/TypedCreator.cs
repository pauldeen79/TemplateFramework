namespace TemplateFramework.Core.CodeGeneration.CodeGeneratorProviderCreators;

public class TypedCreator : ICodeGenerationProviderCreator
{
    public ICodeGenerationProvider? TryCreateInstance(Type type)
    {
        Guard.IsNotNull(type);

        if (Array.Exists(type.GetInterfaces(), i => i == typeof(ICodeGenerationProvider)))
        {
            return (ICodeGenerationProvider)Activator.CreateInstance(type)!;
        }

        return null;
    }
}
