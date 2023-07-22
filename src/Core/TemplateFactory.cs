﻿namespace TemplateFramework.Core;

public class TemplateFactory : ITemplateFactory
{
    private readonly IEnumerable<ITemplateCreator> _childTemplateCreators;

    public TemplateFactory(IEnumerable<ITemplateCreator> childTemplateCreators)
    {
        Guard.IsNotNull(childTemplateCreators);

        _childTemplateCreators = childTemplateCreators;
    }

    public object CreateByModel(object? model)
    {
        var creator = _childTemplateCreators.FirstOrDefault(x => x.SupportsModel(model));
        if (creator is null)
        {
            throw new NotSupportedException($"Model of type {model?.GetType()} is not supported");
        }

        return creator.CreateByModel(model) ?? throw new InvalidOperationException("Child template creator returned a null instance");
    }

    public object CreateByName(string name)
    {
        Guard.IsNotNull(name);

        var creator = _childTemplateCreators.FirstOrDefault(x => x.SupportsName(name));
        if (creator is null)
        {
            throw new NotSupportedException($"Name {name} is not supported");
        }

        return creator.CreateByName(name) ?? throw new InvalidOperationException("Child template creator returned a null instance");
    }
}
