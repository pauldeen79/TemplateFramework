namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

public sealed class ProviderComponent : ITemplateProviderComponent
{
    private readonly IAssemblyService _assemblyService;

    public ProviderComponent(IAssemblyService assemblyService)
    {
        Guard.IsNotNull(assemblyService);
        _assemblyService = assemblyService;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is CreateCompiledTemplateRequest;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        Guard.IsOfType<CreateCompiledTemplateRequest>(identifier);

        var createCompiledTemplateRequest = (CreateCompiledTemplateRequest)identifier;
        var assembly = _assemblyService.GetAssembly(createCompiledTemplateRequest.AssemblyName, createCompiledTemplateRequest.CurrentDirectory);
        var template = assembly.CreateInstance(createCompiledTemplateRequest.ClassName);
        if (template is null)
        {
            throw new InvalidOperationException($"Could not create instance of type {createCompiledTemplateRequest.ClassName}");
        }

        return template;
    }
}
