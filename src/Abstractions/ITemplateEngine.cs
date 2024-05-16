namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    Task Render(IRenderTemplateRequest request, CancellationToken cancellationToken);
    ITemplateParameter[] GetParameters(object templateInstance);
}
