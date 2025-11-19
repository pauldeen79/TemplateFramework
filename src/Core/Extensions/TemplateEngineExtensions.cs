namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    public static async Task<Result> RenderChildTemplatesAsync(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken token)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);

        var items = childModels
            .OfType<object?>()
            .Select((model, index) => new { Model = model, Index = index })
            .ToArray();

        return await items.PerformUntilFailure(x => instance.RenderAsync(new RenderTemplateRequest(identifier, x.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, x.Model, x.Index, items.Length))), token)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplatesAsync(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken token)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var items = childModels
            .OfType<object?>()
            .Select((model, index) => new { Model = model, Index = index, Identifier = identifierFactory(model) })
            .ToArray();

        return await items.PerformUntilFailure(x => instance.RenderAsync(new RenderTemplateRequest(x.Identifier, x.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(x.Identifier, x.Model, x.Index, items.Length))), token)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplatesAsync(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken token)
    {
        Guard.IsNotNull(childModels);

        return await childModels.PerformUntilFailure(x => instance.RenderAsync(new RenderTemplateRequest(identifier, x, generationEnvironment, string.Empty, null, null), token)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplatesAsync(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, CancellationToken token)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(identifierFactory);

        return await childModels.PerformUntilFailure(x => instance.RenderAsync(new RenderTemplateRequest(identifierFactory(x), x, generationEnvironment, string.Empty, null, null), token)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplatesAsync(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, ITemplateIdentifier> templateIdentifierFactory, CancellationToken token)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateIdentifierFactory);

        var items = childModels
            .OfType<object?>()
            .Select((model, index) => new { Model = model, Index = index })
            .ToArray();

        return await items.PerformUntilFailure(x =>
        {
            var identifier = templateIdentifierFactory(x.Model);
            return instance.RenderAsync(new RenderTemplateRequest(identifier, x.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, x.Model, x.Index, items.Length))), token);
        }).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplatesAsync(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, object> templateFactory, CancellationToken token)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        return await items.PerformUntilFailure(x =>
        {
            var template = templateFactory(x.Model);
            return instance.RenderAsync(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), x.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(new TemplateInstanceIdentifier(template), x, x.Index, items.Length))), token);
        }).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplatesAsync(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken token)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        return await items.PerformUntilFailure(x => instance.RenderAsync(new RenderTemplateRequest(identifier, x.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, x.Model, x.Index, items.Length))), token)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);

        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory(childModel);
        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);

        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier))), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory();
        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier))), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken token)
    {
        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, CancellationToken token)
    {
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory(childModel);
        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken token)
    {
        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, null), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<ITemplateIdentifier> identifierFactory, CancellationToken token)
    {
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory();
        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, null), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken token)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, childModel, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), token)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplateAsync(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken token)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, null, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier))), token)
            .ConfigureAwait(false);
    }
}
