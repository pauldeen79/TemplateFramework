using CrossCutting.Common.Results;

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

    public Result<object> Create(ITemplateIdentifier identifier)
    {
        if (identifier is not CompiledTemplateIdentifier createCompiledTemplateRequest)
        {
            return Result.Continue<object>();
        }

        var assembly = _assemblyService.GetAssembly(createCompiledTemplateRequest.AssemblyName, createCompiledTemplateRequest.CurrentDirectory);
        var type = assembly.GetType(createCompiledTemplateRequest.ClassName);
        if (type is null)
        {
            return Result.Error<object>($"Could not get type for class {createCompiledTemplateRequest.ClassName}");
        }

        var template = _templateFactory.Create(type);
        if (template is null)
        {
            return Result.Error<object>($"Could not create instance of type {createCompiledTemplateRequest.ClassName}");
        }

        return Result.Success(template);
    }
}
