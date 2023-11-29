namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, ITemplateContext context)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var identifier = identifierFactory(item.Model);
            instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(childModels);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, string.Empty, null, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(identifierFactory);

        foreach (var model in childModels)
        {
            var identifier = identifierFactory(model);
            instance.Render(new RenderTemplateRequest(identifier, model, generationEnvironment, string.Empty, null, null));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, ITemplateIdentifier> templateIdentifierFactory)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateIdentifierFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var identifier = templateIdentifierFactory(item.Model);
            instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, object> templateFactory)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            instance.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(new TemplateInstanceIdentifier(template), item, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))));
        }
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory(childModel);
        instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context)
    {
        Guard.IsNotNull(context);

        instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<ITemplateIdentifier> identifierFactory, ITemplateContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory();
        instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier)
    {
        instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory)
    {
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory(childModel);
        instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier)
    {
        instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<ITemplateIdentifier> identifierFactory)
    {
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory();
        instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, null));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))));
    }

    public static void RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier))));
    }
}
