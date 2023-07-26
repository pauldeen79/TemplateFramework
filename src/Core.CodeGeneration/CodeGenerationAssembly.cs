namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssembly : ICodeGenerationAssembly
{
    private readonly ICodeGenerationEngine _codeGenerationEngine;

    public CodeGenerationAssembly(ICodeGenerationEngine codeGenerationEngine)
    {
        Guard.IsNotNull(codeGenerationEngine);

        _codeGenerationEngine = codeGenerationEngine;
    }

    public void Generate(ICodeGenerationAssemblySettings settings, IMultipleContentBuilder generationEnvironment)
    {
        Guard.IsNotNull(settings);
        Guard.IsNotNull(generationEnvironment);

        var assembly = AssemblyHelper.GetAssembly(settings.AssemblyName, settings.CurrentDirectory);
        var environment = generationEnvironment.ToGenerationEnvironment();

        foreach (var codeGenerationProvider in GetCodeGeneratorProviders(assembly, settings.ClassNameFilter))
        {
            _codeGenerationEngine.Generate(codeGenerationProvider, environment, settings);
        }
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
