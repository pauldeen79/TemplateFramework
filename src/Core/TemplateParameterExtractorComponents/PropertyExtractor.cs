namespace TemplateFramework.Core.TemplateParameterExtractorComponents;

public class PropertyExtractor : ITemplateParameterExtractorComponent
{
    public Result<ITemplateParameter[]> Extract(object templateInstance)
    {
        Guard.IsNotNull(templateInstance);

        return Result.Success<ITemplateParameter[]>(templateInstance.GetType().GetProperties()
            .Where(p => p.CanRead && p.CanWrite)
            .Select(p => new TemplateParameter(p.Name, p.PropertyType))
            .ToArray());
    }

    public bool Supports(object templateInstance) => templateInstance is not null;
}
