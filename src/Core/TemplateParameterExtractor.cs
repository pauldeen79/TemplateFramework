namespace TemplateFramework.Core;

public class TemplateParameterExtractor : ITemplateParameterExtractor
{
    private readonly IEnumerable<ITemplateParameterExtractorComponent> _components;

    public TemplateParameterExtractor(IEnumerable<ITemplateParameterExtractorComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components;
    }

    public ITemplateParameter[] Extract(object templateInstance)
    {
        Guard.IsNotNull(templateInstance);

        var component = _components.FirstOrDefault(x => x.Supports(templateInstance));
        if (component is null)
        {
            return Array.Empty<ITemplateParameter>();
        }

        return component.Extract(templateInstance);
    }
}
