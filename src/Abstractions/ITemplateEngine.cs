namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    Task Render(IRenderTemplateRequest request);
    ITemplateParameter[] GetParameters(object templateInstance);
}
