namespace TemplateFramework.Core;

public class TemplateProvider : ITemplateProvider
{
    private readonly IEnumerable<ITemplateProviderComponent> _components;

    public TemplateProvider(IEnumerable<ITemplateProviderComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components;
    }

    public object Create(ICreateTemplateRequest request)
    {
        Guard.IsNotNull(request);

        var component = _components.FirstOrDefault(x => x.Supports(request));
        if (component is null)
        {
            throw new NotSupportedException($"Type of create template request ({request.GetType().FullName}) is not supported");
        }

        return component.Create(request);
    }
}
