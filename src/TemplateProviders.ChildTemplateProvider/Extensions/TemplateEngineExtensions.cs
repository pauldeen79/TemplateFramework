namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Extensions;

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

        foreach (var item in items)
        {
            var result = await instance.Render(new RenderTemplateRequest(item.Identifier, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(item.Identifier, item.Model, item.Index, items.Length))), cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }

    public static async Task<Result> RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);

        foreach (var item in childModels)
        {
            var result = await instance.Render(new RenderTemplateRequest(new TemplateByModelIdentifier(item), item, generationEnvironment, string.Empty, null, null), cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        var identifier = new TemplateByModelIdentifier(childModel);
        return await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<Result> RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken)
    {
        var identifier = new TemplateByModelIdentifier(childModel);

        return await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), cancellationToken)
            .ConfigureAwait(false);
    }
}
