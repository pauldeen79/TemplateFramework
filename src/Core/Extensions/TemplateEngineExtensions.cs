namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters)
    {
        Guard.IsNotNull(models);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, additionalParameters, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, string defaultFilename)
    {
        Guard.IsNotNull(models);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, defaultFilename, null, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template, object additionalParameters)
    {
        Guard.IsNotNull(models);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, additionalParameters, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object template)
    {
        Guard.IsNotNull(models);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, null, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, object additionalParameters)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(templateFactory);

        foreach (var model in models)
        {
            var template = templateFactory(model);
            instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, additionalParameters, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(templateFactory);

        foreach (var model in models)
        {
            var template = templateFactory(model);
            instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, null, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, object additionalParameters)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(templateFactory);

        foreach (var model in models)
        {
            var template = templateFactory(model);
            instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, additionalParameters, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(templateFactory);

        foreach (var model in models)
        {
            var template = templateFactory(model);
            instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, null, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, ICreateTemplateRequest> createTemplateRequestFactory)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequestFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = context.Provider.Create(createTemplateRequestFactory(item.Model));
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object additionalParameters, ITemplateContext context, Func<object?, ICreateTemplateRequest> createTemplateRequestFactory)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequestFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = context.Provider.Create(createTemplateRequestFactory(item.Model));
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, object> createTemplateFactory)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = createTemplateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object additionalParameters, ITemplateContext context, Func<object?, object> createTemplateFactory)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateFactory);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = createTemplateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, ITemplateContext context, object template)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(template);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable models, IGenerationEnvironment generationEnvironment, object additionalParameters, ITemplateContext context, object template)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(context);
        Guard.IsNotNull(template);

        var items = models.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, string defaultFilename, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, string defaultFilename, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, object additionalParameters, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters)
    {
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, additionalParameters, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, string defaultFilename)
    {
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template, object additionalParameters)
    {
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, additionalParameters, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object template)
    {
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename, object additionalParameters)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, additionalParameters, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, string defaultFilename)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, defaultFilename, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, object additionalParameters)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, additionalParameters, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(model);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, string defaultFilename, object additionalParameters)
    {
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, additionalParameters, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, string defaultFilename)
    {
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, object additionalParameters)
    {
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, additionalParameters, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template)
    {
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, string defaultFilename, object additionalParameters)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, additionalParameters, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, string defaultFilename)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, object additionalParameters)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, additionalParameters, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, ITemplateContext context, ICreateTemplateRequest createTemplateRequest)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequest);

        var template = context.Provider.Create(createTemplateRequest);
        Guard.IsNotNull(template);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateContext context, ICreateTemplateRequest createTemplateRequest)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequest);

        var template = context.Provider.Create(createTemplateRequest);
        Guard.IsNotNull(template);
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? model, IGenerationEnvironment generationEnvironment, object additionalParameters, ITemplateContext context, ICreateTemplateRequest createTemplateRequest)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequest);

        var template = context.Provider.Create(createTemplateRequest);
        Guard.IsNotNull(template);
        instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, context.DefaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template, model))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object additionalParameters, ITemplateContext context, ICreateTemplateRequest createTemplateRequest)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequest);

        var template = context.Provider.Create(createTemplateRequest);
        Guard.IsNotNull(template);
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, context.DefaultFilename, additionalParameters, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    private sealed class ChildTemplateContext : IChildTemplateContext
    {
        public ChildTemplateContext(object template) : this(template, null, null, null)
        {
        }

        public ChildTemplateContext(object template, object? model) : this(template, model, null, null)
        {
        }

        public ChildTemplateContext(object template, object? model, int? iterationNumber, int? iterationCount)
        {
            Guard.IsNotNull(template);
            Template = template;
            Model = model;
            IterationNumber = iterationNumber;
            IterationCount = iterationCount;
        }

        public object Template { get; }
        public object? Model { get; }

        public int? IterationNumber { get; set; }
        public int? IterationCount { get; set; }
    }
}
