namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    Task<Result> RenderAsync(IRenderTemplateRequest request, CancellationToken token);
    Task<Result<ITemplateParameter[]>> GetParametersAsync(object templateInstance, CancellationToken token);
}
