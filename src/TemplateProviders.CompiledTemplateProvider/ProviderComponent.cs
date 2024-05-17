namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

public sealed class ProviderComponent : ITemplateProviderComponent
{
    private readonly IAssemblyService _assemblyService;
    private readonly ITemplateFactory _templateFactory;

    public ProviderComponent(IAssemblyService assemblyService, ITemplateFactory templateFactory)
    {
        Guard.IsNotNull(assemblyService);
        Guard.IsNotNull(templateFactory);

        _assemblyService = assemblyService;
        _templateFactory = templateFactory;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is CompiledTemplateIdentifier;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        Guard.IsAssignableToType<CompiledTemplateIdentifier>(identifier);

        var createCompiledTemplateRequest = (CompiledTemplateIdentifier)identifier;
        var assembly = _assemblyService.GetAssembly(createCompiledTemplateRequest.AssemblyName, createCompiledTemplateRequest.CurrentDirectory);
        var type = assembly.GetType(createCompiledTemplateRequest.ClassName)
            ?? throw new InvalidOperationException($"Could not get type for class {createCompiledTemplateRequest.ClassName}");

        var template = _templateFactory.Create(type)
            ?? throw new InvalidOperationException($"Could not create instance of type {createCompiledTemplateRequest.ClassName}");
        
        return template;
    }
}
