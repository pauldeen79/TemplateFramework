namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssembly : ICodeGenerationAssembly
{
    private readonly ICodeGenerationEngine _codeGenerationEngine;
    private readonly IEnumerable<ICodeGenerationProviderCreator> _creators;

    public CodeGenerationAssembly(ICodeGenerationEngine codeGenerationEngine, IEnumerable<ICodeGenerationProviderCreator> creators)
    {
        Guard.IsNotNull(codeGenerationEngine);
        Guard.IsNotNull(creators);

        _codeGenerationEngine = codeGenerationEngine;
        _creators = creators;
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

    private IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly, IEnumerable<string>? classNameFilter)
        => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && Array.Exists(t.GetConstructors(), c => c.GetParameters().Length == 0))
            .Where(t => FilterIsValid(t, classNameFilter))
            .Select(t => TryCreateInstance(t)!)
            .Where(x => x is not null);

    private ICodeGenerationProvider? TryCreateInstance(Type type)
        => _creators.Select(creator => creator.TryCreateInstance(type))
                    .FirstOrDefault(x => x is not null);

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
