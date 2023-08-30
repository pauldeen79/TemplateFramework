﻿namespace TemplateFramework.Core.TemplateRenderers;

public sealed class MultipleContentTemplateRenderer : ITemplateRenderer
{
    private readonly ISingleContentTemplateRenderer _singleContentTemplateRenderer;
    private readonly ITemplateProvider _provider;
    private readonly IEnumerable<IMultipleContentBuilderTemplateCreator> _creators;

    public MultipleContentTemplateRenderer(
        ISingleContentTemplateRenderer singleContentTemplateRenderer,
        ITemplateProvider provider,
        IEnumerable<IMultipleContentBuilderTemplateCreator> creators)
    {
        Guard.IsNotNull(singleContentTemplateRenderer);
        Guard.IsNotNull(provider);
        Guard.IsNotNull(creators);

        _singleContentTemplateRenderer = singleContentTemplateRenderer;
        _provider = provider;
        _creators = creators;
    }

    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is MultipleContentBuilderEnvironment;

    public void Render(ITemplateEngineContext context)
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
            multipleContentBuilderTemplate.Render(multipleContentBuilder);
            return;
        }

        // Make a new request, because we are using a different generation environment.
        // Render using a stringbuilder, then add it to multiple contents
        var stringBuilder = new StringBuilder();
        var singleRequest = new RenderTemplateRequest(context.Identifier, context.Model, stringBuilder, context.DefaultFilename, context.AdditionalParameters, context.Context);
        var template = _provider.Create(context.Identifier);
        _singleContentTemplateRenderer.Render(new TemplateEngineContext(singleRequest, context.Engine, template));
        multipleContentBuilder.AddContent(context.DefaultFilename, false, new StringBuilder(stringBuilder.ToString()));
    }

    private IMultipleContentBuilderTemplate? TryGetMultipleContentBuilderTemplate(object template)
        => _creators.Select(x => x.TryCreate(template)).FirstOrDefault(x => x is not null);
}
