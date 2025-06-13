namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    Task<Result> RenderAsync(IRenderTemplateRequest request, CancellationToken cancellationToken);
    Task<Result<ITemplateParameter[]>> GetParametersAsync(object templateInstance, CancellationToken cancellationToken);
}
