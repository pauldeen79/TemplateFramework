namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    void Render(IRenderTemplateRequest request);
    ITemplateParameter[] GetParameters(object templateInstance);
}
