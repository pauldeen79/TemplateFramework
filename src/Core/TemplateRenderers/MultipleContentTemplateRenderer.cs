namespace TemplateFramework.Core.TemplateRenderers;

public sealed class MultipleContentTemplateRenderer : ITemplateRenderer
{
    private readonly IEnumerable<IMultipleContentBuilderTemplateCreator> _creators;

    public MultipleContentTemplateRenderer(
        IEnumerable<IMultipleContentBuilderTemplateCreator> creators)
    {
        Guard.IsNotNull(creators);

        _creators = creators;
    }

    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is MultipleContentBuilderEnvironment;

    public async Task Render(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        if (context.GenerationEnvironment is not MultipleContentBuilderEnvironment builderEnvironment)
        {
            throw new NotSupportedException("GenerationEnvironment should be of type IMultipleContentBuilder or IMultipleContentBuilderContainer");
        }

        var multipleContentBuilder = builderEnvironment.Builder;

        var multipleContentBuilderTemplate = TryGetMultipleContentBuilderTemplate(context.Template);
        if (multipleContentBuilderTemplate is not null)
        {
            // No need to convert string to MultipleContentBuilder, and then add it again..
            // We can simply pass the MultipleContentBuilder instance
            await multipleContentBuilderTemplate.Render(multipleContentBuilder, cancellationToken).ConfigureAwait(false);
            return;
        }

        // Make a new request, because we are using a different generation environment.
        // Render using a stringbuilder, then add it to multiple contents
        var stringBuilder = new StringBuilder();
        var singleRequest = new RenderTemplateRequest(context.Identifier, context.Model, stringBuilder, context.DefaultFilename, context.AdditionalParameters, context.Context);
        await context.Engine.Render(singleRequest, cancellationToken).ConfigureAwait(false);

        multipleContentBuilder.AddContent(context.DefaultFilename, false, new StringBuilder(stringBuilder.ToString()));
    }

    private IMultipleContentBuilderTemplate? TryGetMultipleContentBuilderTemplate(object template)
        => _creators.Select(x => x.TryCreate(template)).FirstOrDefault(x => x is not null);
}
