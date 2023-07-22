namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    void Render<TModel>(IRenderTemplateRequest<TModel> request);
}
