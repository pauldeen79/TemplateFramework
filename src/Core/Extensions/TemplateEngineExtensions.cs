namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    //TODO: Create overloads without default filename and with additional parameters (and combinations)
    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    //TODO: Create overloads without model and default filename, and with additional parameters (and combinations)
    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new TemplateContext(template, model))));
    }
}
