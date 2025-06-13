﻿namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Extensions;

public static class TemplateEngineExtensions
{
    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);

        var items = childModels
            .OfType<object?>()
            .Select((model, index) => new { Model = model, Index = index, Identifier = new TemplateByModelIdentifier(model) })
            .ToArray();

        return await items.PerformUntilFailure(x => instance.RenderAsync(new RenderTemplateRequest(x.Identifier, x.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(x.Identifier, x.Model, x.Index, items.Length))), cancellationToken)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);

        return await childModels.PerformUntilFailure(x => instance.RenderAsync(new RenderTemplateRequest(new TemplateByModelIdentifier(x), x, generationEnvironment, string.Empty, null, null), cancellationToken)).ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        var identifier = new TemplateByModelIdentifier(childModel);
        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken)
    {
        var identifier = new TemplateByModelIdentifier(childModel);

        return await instance
            .RenderAsync(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), cancellationToken)
            .ConfigureAwait(false);
    }
}
