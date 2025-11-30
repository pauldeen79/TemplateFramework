namespace TemplateFramework.TemplateProviders.ChildTemplateProvider;

public sealed class ProviderComponent : ITemplateProviderComponent
{
    private readonly IEnumerable<ITemplateCreator> _childTemplateCreators;

    public ProviderComponent(IEnumerable<ITemplateCreator> childTemplateCreators)
    {
        Guard.IsNotNull(childTemplateCreators);

        _childTemplateCreators = childTemplateCreators;
    }

    public Result<object> Create(ITemplateIdentifier identifier)
    {
        if (!_childTemplateCreators.Any())
        {
            return Result.Continue<object>();
        }

        if (identifier is TemplateByModelIdentifier templateByModelIdentifier)
        {
            return CreateByModel(templateByModelIdentifier.Model);
        }
        else if (identifier is TemplateByNameIdentifier templateByNameIdentifier)
        {
            return CreateByName(templateByNameIdentifier.Name);
        }

        return Result.Continue<object>();
    }

    private Result<object> CreateByModel(object? model)
    {
        foreach (var creator in _childTemplateCreators)
        {
            var result = creator.CreateByModel(model).EnsureNotNull("Child template creator returned a null instance");
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.NotSupported<object>($"Model of type {model?.GetType()} is not supported");
    }

    private Result<object> CreateByName(string name)
    {
        foreach (var creator in _childTemplateCreators)
        {
            var result = creator.CreateByName(name).EnsureNotNull("Child template creator returned a null instance");
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.NotSupported<object>($"Template with name {name} is not supported");
    }
}
