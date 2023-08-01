namespace TemplateFramework.Core.CodeGeneration.CodeGeneratorProviderCreators;

public class WrappedCreator : ICodeGenerationProviderCreator
{
    public ICodeGenerationProvider? TryCreateInstance(Type type)
    {
        Guard.IsNotNull(type);

        if (Array.Exists(type.GetInterfaces(), i => i.FullName == typeof(ICodeGenerationProvider).FullName))
        {
            var instance = Activator.CreateInstance(type);
            if (instance is not null)
            {
                return new CodeGenerationProviderWrapper(instance);
            }
        }

        return null;
    }
}
