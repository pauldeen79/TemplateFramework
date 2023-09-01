namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

public sealed class ProviderComponent : ITemplateProviderComponent
{
    private readonly IAssemblyService _assemblyService;
    private readonly ICompiledTemplateFactory _factory;

    public ProviderComponent(IAssemblyService assemblyService, ICompiledTemplateFactory factory)
    {
        Guard.IsNotNull(assemblyService);
        Guard.IsNotNull(factory);

        _assemblyService = assemblyService;
        _factory = factory;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is CreateCompiledTemplateRequest;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        Guard.IsOfType<CreateCompiledTemplateRequest>(identifier);

        var createCompiledTemplateRequest = (CreateCompiledTemplateRequest)identifier;
        var assembly = _assemblyService.GetAssembly(createCompiledTemplateRequest.AssemblyName, createCompiledTemplateRequest.CurrentDirectory);
        var type = assembly.GetType(createCompiledTemplateRequest.ClassName);
        if (type is null)
        {
            throw new InvalidOperationException($"Could not get type for class {createCompiledTemplateRequest.ClassName}");
        }

        var template = _factory.Create(type);
        if (template is null)
        {
            throw new InvalidOperationException($"Could not create instance of type {createCompiledTemplateRequest.ClassName}");
        }

        return template;
    }
}
