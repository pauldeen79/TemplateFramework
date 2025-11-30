namespace TemplateFramework.Core.TemplateParameterExtractorComponents;

public class TypedExtractor : ITemplateParameterExtractorComponent
{
    public async Task<Result<ITemplateParameter[]>> ExtractAsync(object templateInstance, CancellationToken token)
    {
        if (templateInstance is not IParameterizedTemplate parameterizedTemplate)
        {
            return Result.Continue<ITemplateParameter[]>();
        }

        return await parameterizedTemplate.GetParametersAsync(token).ConfigureAwait(false);
    }
}
