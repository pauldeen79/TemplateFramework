namespace TemplateFramework.Core;

public class TemplateProvider : ITemplateProvider
{
    private readonly IEnumerable<ITemplateProviderComponent> _components;

    public TemplateProvider(IEnumerable<ITemplateProviderComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components;
    }

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        var component = _components.FirstOrDefault(x => x.Supports(identifier));
        if (component is null)
        {
            throw new NotSupportedException($"Type of identifier {identifier.GetType().FullName} is not supported");
        }

        return component.Create(identifier);
    }
}
