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

        var component = _components.FirstOrDefault(x => x.Supports(templateInstance));
        if (component is null)
        {
            return Result.Continue(Array.Empty<ITemplateParameter>());
        }

        return await component.ExtractAsync(templateInstance, token).ConfigureAwait(false);
    }
}
