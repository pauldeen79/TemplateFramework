namespace TemplateFramework.Core.TemplateRenderers;

public sealed class MultipleContentTemplateRenderer : ITemplateRenderer
{
    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is MultipleContentBuilderEnvironment;

    public void Render(IRenderTemplateRequest request)
    {
        Guard.IsNotNull(request);

        if (request.GenerationEnvironment is not MultipleContentBuilderEnvironment builderEnvironment)
        {
            throw new NotSupportedException("GenerationEnvironment should be of type IMultipleContentBuilder or IMultipleContentBuilderContainer");
        }

        var multipleContentBuilder = builderEnvironment.Builder;

        //TODO: Check if we need a wrapper here, when loading external assemblies...
        if (request.Template is IMultipleContentBuilderTemplate multipleContentBuilderTemplate)
        {
            // No need to convert string to MultipleContentBuilder, and then add it again..
            // We can simply pass the MultipleContentBuilder instance
            multipleContentBuilderTemplate.Render(multipleContentBuilder);
            return;
        }

        // Make a new request, because we are using a different generation environment.
        // Render using a stringbuilder, then add it to multiple contents
        var stringBuilder = new StringBuilder();
        var singleRequest = new RenderTemplateRequest(request.Template, request.Model, stringBuilder, request.DefaultFilename, request.AdditionalParameters, request.Context);
        new StringBuilderTemplateRenderer().Render(singleRequest);
        multipleContentBuilder.AddContent(request.DefaultFilename, false, new StringBuilder(stringBuilder.ToString()));
    }
}
