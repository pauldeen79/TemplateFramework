namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        await Task.WhenAll(items
            .Select(item => instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))), cancellationToken))
            ).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        await Task.WhenAll(items.Select(async item =>
        {
            var identifier = identifierFactory(item.Model);
            await instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))), cancellationToken).ConfigureAwait(false);
        })).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);

        await Task.WhenAll(childModels
            .OfType<object?>()
            .Select((model, index) => instance.Render(new RenderTemplateRequest(identifier, model, generationEnvironment, string.Empty, null, null), cancellationToken))
            ).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(identifierFactory);

        await Task.WhenAll(childModels.OfType<object>().Select(async model =>
        {
            var identifier = identifierFactory(model);
            await instance.Render(new RenderTemplateRequest(identifier, model, generationEnvironment, string.Empty, null, null), cancellationToken).ConfigureAwait(false);
        })).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, ITemplateIdentifier> templateIdentifierFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateIdentifierFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        await Task.WhenAll(items.Select(async item =>
        {
            var identifier = templateIdentifierFactory(item.Model);
            await instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))), cancellationToken).ConfigureAwait(false);
        })).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, object> templateFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        await Task.WhenAll(childModels
            .OfType<object?>()
            .Select((model, index) => new { Model = model, Index = index })
            .Select(async item =>
        {
            var template = templateFactory(item.Model);
            await instance.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(new TemplateInstanceIdentifier(template), item, item.Index, childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray().Length))), cancellationToken).ConfigureAwait(false);
        })).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        await Task.WhenAll(items
            .Select(item => instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))), cancellationToken))
            ).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        await instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory(childModel);
        await instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        await instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier))), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory();
        await instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier))), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        await instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory(childModel);
        await instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        await instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, null), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<ITemplateIdentifier> identifierFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory();
        await instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, null), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        await instance.Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        await instance.Render(new RenderTemplateRequest(identifier, null, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier))), cancellationToken).ConfigureAwait(false);
    }
}
