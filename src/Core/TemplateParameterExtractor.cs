namespace TemplateFramework.Core;

public class TemplateParameterExtractor : ITemplateParameterExtractor
{
    private readonly IEnumerable<ITemplateParameterExtractorComponent> _components;

    public TemplateParameterExtractor(IEnumerable<ITemplateParameterExtractorComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components;
    }

    public async Task<Result<ITemplateParameter[]>> ExtractAsync(object templateInstance, CancellationToken token)
    {
        Guard.IsNotNull(templateInstance);

        foreach (var component in _components)
        {
            var result = await component.ExtractAsync(templateInstance, token).ConfigureAwait(false);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.Continue(Array.Empty<ITemplateParameter>());
    }
}
