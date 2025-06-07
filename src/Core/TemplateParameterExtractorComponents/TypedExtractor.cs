namespace TemplateFramework.Core.TemplateParameterExtractorComponents;

public class TypedExtractor : ITemplateParameterExtractorComponent
{
    public async Task<Result<ITemplateParameter[]>> ExtractAsync(object templateInstance, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(templateInstance);
        Guard.IsAssignableToType<IParameterizedTemplate>(templateInstance);

        var parameterizedTemplate = (IParameterizedTemplate)templateInstance;

        return await parameterizedTemplate.GetParametersAsync(cancellationToken).ConfigureAwait(false);
    }

    public bool Supports(object templateInstance) => templateInstance is IParameterizedTemplate;
}
