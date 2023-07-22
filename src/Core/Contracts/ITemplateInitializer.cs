namespace TemplateFramework.Core.Contracts;

public interface ITemplateInitializer
{
    void Initialize<TModel>(IRenderTemplateRequest<TModel> request, ITemplateEngine engine);
}
