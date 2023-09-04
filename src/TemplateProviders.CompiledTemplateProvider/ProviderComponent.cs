﻿namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

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

    public bool Supports(ITemplateIdentifier identifier) => identifier is CreateCompiledTemplateRequest;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        Guard.IsAssignableToType<CreateCompiledTemplateRequest>(identifier);

        var createCompiledTemplateRequest = (CreateCompiledTemplateRequest)identifier;
        var assembly = _assemblyService.GetAssembly(createCompiledTemplateRequest.AssemblyName, createCompiledTemplateRequest.CurrentDirectory);
        var type = assembly.GetType(createCompiledTemplateRequest.ClassName);
        if (type is null)
        {
            throw new InvalidOperationException($"Could not get type for class {createCompiledTemplateRequest.ClassName}");
        }

        var template = _templateFactory.Create(type);
        if (template is null)
        {
            throw new InvalidOperationException($"Could not create instance of type {createCompiledTemplateRequest.ClassName}");
        }

        return template;
    }
}
