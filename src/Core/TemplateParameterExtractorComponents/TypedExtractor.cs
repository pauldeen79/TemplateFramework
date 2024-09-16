namespace TemplateFramework.Core.TemplateParameterExtractorComponents;

public class TypedExtractor : ITemplateParameterExtractorComponent
{
    public Result<ITemplateParameter[]> Extract(object templateInstance)
    {
        Guard.IsNotNull(templateInstance);
        Guard.IsAssignableToType<IParameterizedTemplate>(templateInstance);

        var parameterizedTemplate = (IParameterizedTemplate)templateInstance;

        return parameterizedTemplate.GetParameters();
    }

    public bool Supports(object templateInstance) => templateInstance is IParameterizedTemplate;
}
