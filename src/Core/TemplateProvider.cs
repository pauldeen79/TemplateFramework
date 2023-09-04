namespace TemplateFramework.Core;

public class TemplateProvider : ITemplateProvider
{
    private readonly List<ITemplateProviderComponent> _components;

    public TemplateProvider(IEnumerable<ITemplateProviderComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components.ToList();
    }

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        var component = _components.Find(x => x.Supports(identifier));
        if (component is null)
        {
            throw new NotSupportedException($"Type of identifier {identifier.GetType().FullName} is not supported");
        }

        return component.Create(identifier);
    }

    public void RegisterComponent(ITemplateProviderComponent component)
    {
        Guard.IsNotNull(component);

        _components.Add(component);
    }
}
