namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssembly : ICodeGenerationAssembly
{
    private readonly ICodeGenerationEngine _codeGenerationEngine;

    public CodeGenerationAssembly(ICodeGenerationEngine codeGenerationEngine)
    {
        Guard.IsNotNull(codeGenerationEngine);

        _codeGenerationEngine = codeGenerationEngine;
    }

    public void Generate(ICodeGenerationAssemblySettings settings, IGenerationEnvironment generationEnvironment)
    {
        Guard.IsNotNull(settings);
        Guard.IsNotNull(generationEnvironment);

        var assembly = AssemblyHelper.GetAssembly(settings.AssemblyName, settings.CurrentDirectory);

        foreach (var codeGenerationProvider in GetCodeGeneratorProviders(assembly, settings.ClassNameFilter))
        {
            _codeGenerationEngine.Generate(codeGenerationProvider, generationEnvironment, settings);
        }
    }

    private static IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly, IEnumerable<string>? classNameFilter)
        => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && Array.Exists(t.GetConstructors(), c => c.GetParameters().Length == 0))
            .Where(t => FilterIsValid(t, classNameFilter))
            .Select(t =>
            {
                if (Array.Exists(t.GetInterfaces(), i => i == typeof(ICodeGenerationProvider)))
                {
                    return (ICodeGenerationProvider)Activator.CreateInstance(t)!;
                }

                if (Array.Exists(t.GetInterfaces(), i => i.FullName == typeof(ICodeGenerationProvider).FullName))
                {
                    var instance = Activator.CreateInstance(t);
                    if (instance is not null)
                    {
                        return new CodeGenerationProviderWrapper(instance);
                    }
                    else
                    {
                        return null!;
                    }
                }

                return null!;

            }).Where(x => x is not null);

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
