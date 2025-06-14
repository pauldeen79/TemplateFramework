﻿namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssembly : ICodeGenerationAssembly
{
    private readonly ICodeGenerationEngine _codeGenerationEngine;
    private readonly IAssemblyService _assemblyService;
    private readonly IEnumerable<ICodeGenerationProviderCreator> _creators;

    public CodeGenerationAssembly(
        ICodeGenerationEngine codeGenerationEngine,
        IAssemblyService assemblyService,
        IEnumerable<ICodeGenerationProviderCreator> creators)
    {
        Guard.IsNotNull(codeGenerationEngine);
        Guard.IsNotNull(assemblyService);
        Guard.IsNotNull(creators);

        _codeGenerationEngine = codeGenerationEngine;
        _assemblyService = assemblyService;
        _creators = creators;
    }

    public async Task<Result> GenerateAsync(ICodeGenerationAssemblySettings settings, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(settings);
        Guard.IsNotNull(generationEnvironment);

        var assembly = _assemblyService.GetAssembly(settings.AssemblyName, settings.CurrentDirectory);
        var results = await Task.WhenAll(GetCodeGeneratorProviders(assembly, settings.ClassNameFilter)
            .Select(x => _codeGenerationEngine.GenerateAsync(x, generationEnvironment, settings, cancellationToken)))
            .ConfigureAwait(false);

        return Result.Aggregate(results, Result.Success(), nonSuccesfulResults => Result.Error(nonSuccesfulResults, "One or more code generation engines returned a non-succesful result, see the inner results for more details"));
    }

    private IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly, IEnumerable<string> classNameFilter)
        => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && Array.Exists(t.GetConstructors(), c => c.GetParameters().Length == 0))
            .Where(t => FilterIsValid(t, classNameFilter))
            .Select(t => TryCreateInstance(t)!)
            .Where(x => x is not null);

    private ICodeGenerationProvider? TryCreateInstance(Type type)
        => _creators.Select(creator => creator.TryCreateInstance(type))
                    .FirstOrDefault(x => x is not null);

    private static bool FilterIsValid(Type type, IEnumerable<string> classNameFilter)
    {
        var items = classNameFilter.ToArray();

        return items.Length == 0 || Array.Find(items, x => x == type.FullName) is not null;
    }
}
