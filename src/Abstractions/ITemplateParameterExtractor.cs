namespace TemplateFramework.Abstractions;

public interface ITemplateParameterExtractor
{
    Task<Result<ITemplateParameter[]>> ExtractAsync(object templateInstance, CancellationToken cancellationToken);
}
