namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Extensions;

public static class TemplateEngineExtensions
{
    //TODO: Refactor all methods to use Task<Result> instead of Task
    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);
        Guard.IsNotNull(context);

        var items = childModels
            .OfType<object?>()
            .Select((model, index) => new { Model = model, Index = index, Identifier = new TemplateByModelIdentifier(model) })
            .ToArray();
        await Task.WhenAll(items
            .Select(item => instance.Render(new RenderTemplateRequest(item.Identifier, item.Model, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(item.Identifier, item.Model, item.Index, items.Length))), cancellationToken))
            ).ConfigureAwait(false);
    }

    public static async Task RenderChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(childModels);

        await Task.WhenAll(childModels
            .OfType<object?>()
            .Select((model, index) => instance.Render(new RenderTemplateRequest(new TemplateByModelIdentifier(model), model, generationEnvironment, string.Empty, null, null), cancellationToken)))
            .ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        var identifier = new TemplateByModelIdentifier(childModel);
        await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, context.CreateChildContext(new ChildTemplateContext(identifier, childModel))), cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task RenderChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken)
    {
        var identifier = new TemplateByModelIdentifier(childModel);

        await instance
            .Render(new RenderTemplateRequest(identifier, childModel, generationEnvironment, string.Empty, null, null), cancellationToken)
            .ConfigureAwait(false);
    }
}
