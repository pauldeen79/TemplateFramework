﻿namespace TemplateFramework.Core;

public class TemplateProvider : ITemplateProvider
{
    private readonly List<ITemplateProviderComponent> _originalComponents;
    private readonly List<ITemplateProviderComponent> _components;

    public TemplateProvider(IEnumerable<ITemplateProviderComponent> components)
    {
        Guard.IsNotNull(components);

        _originalComponents = components.ToList();
        _components = new List<ITemplateProviderComponent>(_originalComponents);
    }

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        var component = _components.Find(x => x.Supports(identifier))
            ?? throw new NotSupportedException($"Type of identifier {identifier.GetType().FullName} is not supported");

        return component.Create(identifier);
    }

    public void RegisterComponent(ITemplateProviderComponent component)
    {
        Guard.IsNotNull(component);

        _components.Add(component);
    }

    public async Task StartSession(CancellationToken cancellationToken)
    {
        _components.Clear();
        _components.AddRange(_originalComponents);

        await Task.WhenAll(_components.OfType<ISessionAwareComponent>()
            .Select(x => x.StartSession(cancellationToken)))
            .ConfigureAwait(false);
    }
}
