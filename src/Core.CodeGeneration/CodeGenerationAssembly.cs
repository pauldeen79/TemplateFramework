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

        var assembly = GetAssembly(settings);

        return GetOutputFromAssembly(assembly, settings);
    }

    [ExcludeFromCodeCoverage]
    private static Assembly GetAssembly(ICodeGenerationAssemblySettings settings)
    {
        if (settings.AssemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        {
            // This is plug-in architecture that conforms to Microsoft's documentation.
            var pluginLocation = Path.GetDirectoryName(settings.AssemblyName);
            if (string.IsNullOrEmpty(pluginLocation))
            {
                throw new InvalidOperationException($"Could not get directory from assembly name {settings.AssemblyName}");
            }

            var context = new PluginLoadContext(pluginLocation);
            return context.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
        else
        {
            // This is kind of quirk mode, with an assembly name.
            // Works as long as you are using the same package reference on both sides. (the host program and hte plug-in assembly)
            var context = new CustomAssemblyLoadContext("TemplateFramework.Core.CodeGeneration", true, () => new[] { settings.CurrentDirectory });
            return LoadAssembly(context, settings);
        }
    }

    private static Assembly LoadAssembly(AssemblyLoadContext context, ICodeGenerationAssemblySettings settings)
    {
        try
        {
            return context.LoadFromAssemblyName(new AssemblyName(settings.AssemblyName));
        }
        catch (Exception e) when (e.Message.StartsWith("The given assembly name was invalid.", StringComparison.InvariantCulture) || e.Message.EndsWith("The system cannot find the file specified.", StringComparison.InvariantCulture))
        {
            if (settings.AssemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathRooted(settings.AssemblyName))
            {
                return context.LoadFromAssemblyPath(Path.Combine(settings.CurrentDirectory, Path.GetFileName(settings.AssemblyName)));
            }
            return context.LoadFromAssemblyPath(settings.AssemblyName);
        }
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
                if (instance == null)
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
        if (classNameFilter == null)
        {
            return true;
        }

        var items = classNameFilter.ToArray();
        return !items.Any() || Array.Find(items, x => x == type.FullName) != null;
    }
}
