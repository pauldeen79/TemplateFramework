namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, object template)
    {
        Guard.IsNotNull(childModels);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, string.Empty, null, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(templateFactory);

        foreach (var model in childModels)
        {
            var template = templateFactory(model);
            instance.Render(new RenderTemplateRequest(template, model, generationEnvironment, string.Empty, null, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, ICreateTemplateRequest> createTemplateRequestFactory)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequestFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = context.Provider.Create(createTemplateRequestFactory(item.Model));
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, object> createTemplateFactory)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = createTemplateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, object template)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(template);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(template, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template, childModel))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(childModel);
        instance.Render(new RenderTemplateRequest(template, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template, childModel))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, object template)
    {
        instance.Render(new RenderTemplateRequest(template, childModel, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, object> templateFactory)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory(childModel);
        instance.Render(new RenderTemplateRequest(template, childModel, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, object template)
    {
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<object> templateFactory)
    {
        Guard.IsNotNull(templateFactory);

        var template = templateFactory();
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context, ICreateTemplateRequest createTemplateRequest)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequest);

        var template = context.Provider.Create(createTemplateRequest);
        instance.Render(new RenderTemplateRequest(template, childModel, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template, childModel))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateContext context, ICreateTemplateRequest createTemplateRequest)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(createTemplateRequest);

        var template = context.Provider.Create(createTemplateRequest);
        instance.Render(new RenderTemplateRequest(template, null, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(template))));
    }

    //TODO: Make public, move to Core, and add unit tests
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
