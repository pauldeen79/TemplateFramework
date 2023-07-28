namespace TemplateFramework.Core.Contracts;

public interface ITemplateInitializer
{
    void Initialize(IRenderTemplateRequest request, ITemplateEngine engine);
}
