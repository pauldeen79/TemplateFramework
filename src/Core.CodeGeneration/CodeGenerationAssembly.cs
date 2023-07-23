namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssembly : ICodeGenerationAssembly
{
    private readonly ICodeGenerationEngine _codeGenerationEngine;
    private readonly ITemplateFileManagerFactory _templateFileManagerFactory;

    public CodeGenerationAssembly(ICodeGenerationEngine codeGenerationEngine,
                                  ITemplateFileManagerFactory templateFileManagerFactory)
    {
        Guard.IsNotNull(codeGenerationEngine);
        Guard.IsNotNull(templateFileManagerFactory);

        _codeGenerationEngine = codeGenerationEngine;
        _templateFileManagerFactory = templateFileManagerFactory;
    }

    public string Generate(ICodeGenerationAssemblySettings settings)
    {
        Guard.IsNotNull(settings);

        var assembly = AssemblyHelper.GetAssembly(settings.AssemblyName, settings.CurrentDirectory);

        return GetOutputFromAssembly(assembly, settings);
    }

    private string GetOutputFromAssembly(Assembly assembly, ICodeGenerationAssemblySettings settings)
    {
        var templateFileManager = _templateFileManagerFactory.Create();
        templateFileManager.MultipleContentBuilder.BasePath = settings.BasePath;

        foreach (var codeGenerationProvider in GetCodeGeneratorProviders(assembly, settings.ClassNameFilter))
        {
            _codeGenerationEngine.Generate(codeGenerationProvider, templateFileManager, settings);
        }

        return templateFileManager.MultipleContentBuilder.ToString()!;
    }

    private static IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly, IEnumerable<string>? classNameFilter)
        => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && Array.Exists(t.GetInterfaces(), i => i.FullName == typeof(ICodeGenerationProvider).FullName))
            .Where(t => FilterIsValid(t, classNameFilter))
            .Select(t =>
            {
                var instance = Activator.CreateInstance(t);
                if (instance is null)
                {
                    throw new InvalidOperationException($"Could not create instance of type {t.FullName}");
                }
                if (instance is not ICodeGenerationProvider provider)
                {
                    throw new InvalidOperationException($"Type {t.FullName} is not of type ICodeGenerationProvider");
                }
                return provider;
            });

    private static bool FilterIsValid(Type type, IEnumerable<string>? classNameFilter)
    {
        if (classNameFilter is null)
        {
            return true;
        }

        var items = classNameFilter.ToArray();
        return !items.Any() || Array.Find(items, x => x == type.FullName) is not null;
    }
}
