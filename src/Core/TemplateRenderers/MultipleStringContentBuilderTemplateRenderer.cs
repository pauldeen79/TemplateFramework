namespace TemplateFramework.Core.TemplateRenderers;

public class MultipleStringContentBuilderTemplateRenderer : ITemplateRenderer
{
    private readonly IEnumerable<IMultipleContentBuilderTemplateCreator<StringBuilder>> _creators;

    public MultipleStringContentBuilderTemplateRenderer(
        IEnumerable<IMultipleContentBuilderTemplateCreator<StringBuilder>> creators)
    {
        Guard.IsNotNull(creators);

        _creators = creators;
    }

    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is MultipleContentBuilderEnvironment<StringBuilder>;

    public async Task Render(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        if (context.GenerationEnvironment is not MultipleContentBuilderEnvironment<StringBuilder> builderEnvironment)
        {
            throw new NotSupportedException("GenerationEnvironment should be of type MultipleContentBuilderEnvironment");
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

    private IMultipleContentBuilderTemplate<StringBuilder>? TryGetMultipleContentBuilderTemplate(object template)
        => _creators.Select(x => x.TryCreate(template)).FirstOrDefault(x => x is not null);
}
