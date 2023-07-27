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

        if (request.Template is IMultipleContentBuilderTemplate multipleContentBuilderTemplate)
        {
            // No need to convert string to MultipleContentBuilder, and then add it again..
            // We can simply pass the MultipleContentBuilder instance
            multipleContentBuilderTemplate.Render(multipleContentBuilder);
            return;
        }

        var stringBuilder = new StringBuilder();
        var singleRequest = new RenderTemplateRequest(request.Template, stringBuilder, request.DefaultFilename, request.AdditionalParameters, null); // note that additional parameters are currently ignored by the implemented class
        new StringBuilderTemplateRenderer().Render(singleRequest);
        multipleContentBuilder.AddContent(request.DefaultFilename, false, new StringBuilder((string?)stringBuilder.ToString()));
    }
}
