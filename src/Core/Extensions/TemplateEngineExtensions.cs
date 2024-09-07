﻿namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);

        var items = childModels
            .OfType<object?>()
            .Select((model, index) => new { Model = model, Index = index })
            .ToArray();

        return await items.PerformUntilFailure(x => instance.Render(new RenderTemplateRequest(identifier, x.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, x.Model, x.Index, items.Length))), cancellationToken)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var items = childModels
            .OfType<object?>()
            .Select((model, index) => new { Model = model, Index = index, Identifier = identifierFactory(model) })
            .ToArray();

        return await items.PerformUntilFailure(x => instance.Render(new RenderTemplateRequest(x.Identifier, x.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(x.Identifier, x.Model, x.Index, items.Length))), cancellationToken)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);

        foreach (var item in childModels)
        {
            var result = await instance.Render(new RenderTemplateRequest(identifier, item, generationEnvironment, string.Empty, null, null), cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }

    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(identifierFactory);

        foreach (var item in childModels)
        {
            var identifier = identifierFactory(item);
            var result = await instance.Render(new RenderTemplateRequest(identifier, item, generationEnvironment, string.Empty, null, null), cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }

    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, ITemplateIdentifier> templateIdentifierFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateIdentifierFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var identifier = templateIdentifierFactory(item.Model);
            var result = await instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))), cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }

    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, Func<object?, object> templateFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(templateFactory);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var template = templateFactory(item.Model);
            var result = await instance.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(new TemplateInstanceIdentifier(template), item, item.Index, items.Length))), cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }

    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        var items = childModels.OfType<object?>().Select((model, index) => new { Model = model, Index = index }).ToArray();
        foreach (var item in items)
        {
            var result = await instance.Render(new RenderTemplateRequest(identifier, item.Model, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, item.Model, item.Index, items.Length))), cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        return await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory(childModel);
        return await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        return await instance
            .Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier))), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<ITemplateIdentifier> identifierFactory, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory();
        return await instance
            .Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier))), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        return await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, Func<object?, ITemplateIdentifier> identifierFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory(childModel);
        return await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        return await instance
            .Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, null), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, Func<ITemplateIdentifier> identifierFactory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(identifierFactory);

        var identifier = identifierFactory();
        return await instance
            .Render(new RenderTemplateRequest(identifier, null, generationEnvironment, string.Empty, null, null), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        return await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, IGenerationEnvironment generationEnvironment, ITemplateContext context, ITemplateIdentifier identifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(identifier);

        return await instance
            .Render(new RenderTemplateRequest(identifier, null, generationEnvironment, context.DefaultFilename, null, context.CreateChildContext(new ChildTemplateContext(identifier))), cancellationToken)
            .ConfigureAwait(false);
    }
}
