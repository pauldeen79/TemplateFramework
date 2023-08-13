namespace TemplateFramework.Core.Abstractions;

public interface ITemplateInitializer
{
    void Initialize(IRenderTemplateRequest request, ITemplateEngine engine);
}
