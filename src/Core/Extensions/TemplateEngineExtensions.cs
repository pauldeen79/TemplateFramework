namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new TemplateContext(template, item, null, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new TemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new TemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new TemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, null, context.CreateChildContext(new TemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new TemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new TemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new TemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, null, context.CreateChildContext(new TemplateContext(template, model))));
    }

    //###

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new TemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, null, context.CreateChildContext(new TemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new TemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new TemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new TemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, null, context.CreateChildContext(new TemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new TemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new TemplateContext(template))));
    }
}
