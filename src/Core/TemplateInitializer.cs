namespace TemplateFramework.Core;

public sealed class TemplateInitializer : ITemplateInitializer
{
    private readonly IEnumerable<ITemplateInitializerComponent> _components;

    public TemplateInitializer(IEnumerable<ITemplateInitializerComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components;
    }

    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        foreach (var component in _components)
        {
            component.Initialize(request, engine);
        }
    }
}
