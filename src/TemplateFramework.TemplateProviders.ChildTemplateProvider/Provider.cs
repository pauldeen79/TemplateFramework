﻿namespace TemplateFramework.TemplateProviders.ChildTemplateProvider;

public class Provider : ITemplateProvider
{
    private readonly IEnumerable<ITemplateCreator> _childTemplateCreators;

    public Provider(IEnumerable<ITemplateCreator> childTemplateCreators)
    {
        Guard.IsNotNull(childTemplateCreators);

        _childTemplateCreators = childTemplateCreators;
    }

    public object Create(ICreateTemplateRequest request)
    {
        Guard.IsNotNull(request);

        if (request is CreateTemplateByModelRequest createTemplateByModelRequest)
        {
            return CreateByModel(createTemplateByModelRequest.Model);
        }
        else if (request is CreateTemplateByNameRequest createTemplateByNameRequest)
        {
            return CreateByName(createTemplateByNameRequest.Name);
        }

        throw new NotSupportedException($"Unsupported create template request: {request.GetType().FullName}");
    }

    private object CreateByModel(object? model)
    {
        var creator = _childTemplateCreators.FirstOrDefault(x => x.SupportsModel(model));
        if (creator is null)
        {
            throw new NotSupportedException($"Model of type {model?.GetType()} is not supported");
        }

        return creator.CreateByModel(model) ?? throw new InvalidOperationException("Child template creator returned a null instance");
    }

    private object CreateByName(string name)
    {
        var creator = _childTemplateCreators.FirstOrDefault(x => x.SupportsName(name));
        if (creator is null)
        {
            throw new NotSupportedException($"Name {name} is not supported");
        }

        return creator.CreateByName(name) ?? throw new InvalidOperationException("Child template creator returned a null instance");
    }

    public bool Supports(ICreateTemplateRequest request) => request is CreateTemplateByModelRequest or CreateTemplateByNameRequest;
}
