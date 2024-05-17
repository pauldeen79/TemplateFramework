namespace TemplateFramework.Core;

public sealed class TemplateInitializer : ITemplateInitializer
{
    private readonly IEnumerable<ITemplateInitializerComponent> _components;

    public TemplateInitializer(IEnumerable<ITemplateInitializerComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components;
    }

    public async Task Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        await Task.WhenAll(_components.Select(component => component.Initialize(context, cancellationToken))).ConfigureAwait(false);
    }
}
