namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    void Render(IRenderTemplateRequest request);
}
