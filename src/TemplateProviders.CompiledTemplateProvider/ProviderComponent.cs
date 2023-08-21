namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

public sealed class ProviderComponent : ITemplateProviderComponent
{
    private readonly IAssemblyService _assemblyService;

    public ProviderComponent(IAssemblyService assemblyService)
    {
        Guard.IsNotNull(assemblyService);
        _assemblyService = assemblyService;
    }

    public bool Supports(ICreateTemplateRequest request) => request is CreateCompiledTemplateRequest;

    public object Create(ICreateTemplateRequest request)
    {
        Guard.IsNotNull(request);
        Guard.IsOfType<CreateCompiledTemplateRequest>(request);

        var createCompiledTemplateRequest = (CreateCompiledTemplateRequest)request;
        var assembly = _assemblyService.GetAssembly(createCompiledTemplateRequest.AssemblyName, createCompiledTemplateRequest.CurrentDirectory);
        var template = assembly.CreateInstance(createCompiledTemplateRequest.ClassName);
        if (template is null)
        {
            throw new InvalidOperationException($"Could not create instance of type {createCompiledTemplateRequest.ClassName}");
        }

        return new TemplateWrapper(template);
    }
}
