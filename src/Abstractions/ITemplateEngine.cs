namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    Task<Result> Render(IRenderTemplateRequest request, CancellationToken cancellationToken);
    Task<Result<ITemplateParameter[]>> GetParameters(object templateInstance);
}
