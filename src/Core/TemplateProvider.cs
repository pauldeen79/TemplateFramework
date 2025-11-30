namespace TemplateFramework.Core;

public class TemplateProvider : ITemplateProvider
{
    private readonly List<ITemplateProviderComponent> _originalComponents;
    private readonly List<ITemplateProviderComponent> _components;

    public TemplateProvider(IEnumerable<ITemplateProviderComponent> components)
    {
        Guard.IsNotNull(components);

        _originalComponents = components.ToList();
        _components = [.. _originalComponents];
    }

    public Result<object> Create(ITemplateIdentifier identifier)
    {
        if (identifier is null)
        {
            return Result.Invalid<object>("Identifier is required");
        }

        foreach (var component in _components)
        {
            var result = component.Create(identifier);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }
        
        return Result.NotSupported<object>($"Type of identifier {identifier.GetType().FullName} is not supported");
    }

    public void RegisterComponent(ITemplateProviderComponent component)
    {
        Guard.IsNotNull(component);

        _components.Add(component);
    }

    public async Task<Result> StartSessionAsync(CancellationToken token)
    {
        _components.Clear();
        _components.AddRange(_originalComponents);

        var results = await Task.WhenAll(_components.OfType<ISessionAwareComponent>()
            .Select(x => x.StartSessionAsync(token)))
            .ConfigureAwait(false);

        return Result.Aggregate
        (
            results,
            Result.Success(),
            errors => Result.Error(errors, "An error occured while starting the session. See the inner results for more details.")
        );
    }
}
