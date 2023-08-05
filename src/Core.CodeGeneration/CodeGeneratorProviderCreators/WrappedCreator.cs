namespace TemplateFramework.Core.CodeGeneration.CodeGeneratorProviderCreators;

public class WrappedCreator : ICodeGenerationProviderCreator
{
    private static readonly (string name, Type type)[] codeGeneratorProperties = new[]
    {
        (nameof(ICodeGenerationProvider.Path), typeof(string)),
        (nameof(ICodeGenerationProvider.RecurseOnDeleteGeneratedFiles), typeof(bool)),
        (nameof(ICodeGenerationProvider.LastGeneratedFilesFilename), typeof(string)),
        (nameof(ICodeGenerationProvider.Encoding), typeof(Encoding))
    };

    private static readonly (string name, Type type)[] codeGeneratorMethods = new[]
    {
        (nameof(ICodeGenerationProvider.CreateGenerator), typeof(object)),
        (nameof(ICodeGenerationProvider.CreateAdditionalParameters), typeof(object)),
        (nameof(ICodeGenerationProvider.CreateModel), typeof(object))
    };

    public ICodeGenerationProvider? TryCreateInstance(Type type)
    {
        Guard.IsNotNull(type);

        if (IsCodeGenerationProvider(type))
        {
            var instance = Activator.CreateInstance(type);
            if (instance is not null)
            {
                return new CodeGenerationProviderWrapper(instance);
            }
        }

        return null;
    }

    private bool IsCodeGenerationProvider(Type type)
    {
        var properties = type.GetProperties();
        var methods = type.GetMethods();

        return Array.TrueForAll(codeGeneratorProperties, x => Array.Exists(properties, p => p.Name == x.name && p.PropertyType == x.type))
            && Array.TrueForAll(codeGeneratorMethods, x => Array.Exists(methods, m => m.Name == x.name && m.ReturnType == x.type));
    }
}
