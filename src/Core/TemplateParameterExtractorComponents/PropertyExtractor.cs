namespace TemplateFramework.Core.TemplateParameterExtractorComponents;

public class PropertyExtractor : ITemplateParameterExtractorComponent
{
    public Task<Result<ITemplateParameter[]>> ExtractAsync(object templateInstance, CancellationToken cancellationToken)
        => Task.Run(() =>
        {
            Guard.IsNotNull(templateInstance);

            return Result.Success<ITemplateParameter[]>(templateInstance.GetType().GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .Select(p => new TemplateParameter(p.Name, p.PropertyType))
                .ToArray());
        }, cancellationToken);

    public bool Supports(object templateInstance) => templateInstance is not null;
}
