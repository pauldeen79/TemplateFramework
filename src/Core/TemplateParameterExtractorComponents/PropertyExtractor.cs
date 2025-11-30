namespace TemplateFramework.Core.TemplateParameterExtractorComponents;

public class PropertyExtractor : ITemplateParameterExtractorComponent
{
    public Task<Result<ITemplateParameter[]>> ExtractAsync(object templateInstance, CancellationToken token)
        => Task.Run(() =>
        {
            if (templateInstance is null)
            {
                return Result.Continue<ITemplateParameter[]>();
            }

            return Result.Success<ITemplateParameter[]>(templateInstance.GetType().GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .Select(p => new TemplateParameter(p.Name, p.PropertyType))
                .ToArray());
        }, token);
}
