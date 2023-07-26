namespace TemplateFramework.Core.TemplateRenderers;

public class MultipleContentTemplateRenderer : ITemplateRenderer
{
    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is MultipleContentBuilderEnvironment or MultipleContentBuilderContainerEnvironment;

    public void Render(IRenderTemplateRequest request)
    {
        Guard.IsNotNull(request);

        IMultipleContentBuilder multipleContentBuilder;
        if (request.GenerationEnvironment is MultipleContentBuilderContainerEnvironment containerEnvironment)
        {
            // Use TemplateFileManager
            multipleContentBuilder = containerEnvironment.Container.MultipleContentBuilder
                ?? throw new InvalidOperationException("MultipleContentBuilder property is null");
        }
        else if (request.GenerationEnvironment is MultipleContentBuilderEnvironment builderEnvironment)
        {
            multipleContentBuilder = builderEnvironment.Builder;
        }
        else
        {
            throw new NotSupportedException("GenerationEnvironment should be of type IMultipleContentBuilder or IMultipleContentBuilderContainer");
        }

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
