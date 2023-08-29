namespace TemplateFramework.TemplateProviders.ChildTemplateProvider;

public sealed class ProviderComponent : ITemplateProviderComponent
{
    private readonly IEnumerable<ITemplateCreator> _childTemplateCreators;

    public ProviderComponent(IEnumerable<ITemplateCreator> childTemplateCreators)
    {
        Guard.IsNotNull(childTemplateCreators);

        _childTemplateCreators = childTemplateCreators;
    }

    public object Create(ITemplateIdentifier request)
    {
        Guard.IsNotNull(request);

        if (request is TemplateByModelIdentifier templateByModelIdentifier)
        {
            return CreateByModel(templateByModelIdentifier.Model);
        }
        else if (request is TemplateByNameIdentifier templateByNameIdentifier)
        {
            return CreateByName(templateByNameIdentifier.Name);
        }

        throw new NotSupportedException($"Unsupported template identifier: {request.GetType().FullName}");
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

    public bool Supports(ITemplateIdentifier request) => request is TemplateByModelIdentifier or TemplateByNameIdentifier;
}
