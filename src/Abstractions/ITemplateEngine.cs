namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    Task<Result> Render(IRenderTemplateRequest request, CancellationToken cancellationToken);
    //TODO: Convert to Result also?
    ITemplateParameter[] GetParameters(object templateInstance);
}
