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
            multipleContentBuilder = containerEnvironment.Builder.MultipleContentBuilder
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
        new SingleContentTemplateRenderer().Render(singleRequest);
        var builderResult = stringBuilder.ToString();

        if (builderResult.Contains(@"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">", StringComparison.InvariantCulture))
        {
            var multipleContents = MultipleContentBuilder.FromString(builderResult);
            foreach (var content in multipleContents.Contents.Select(x => x.Build()))
            {
                multipleContentBuilder.AddContent(content.Filename, content.SkipWhenFileExists, content.Builder);
            }
        }
        else
        {
            multipleContentBuilder.AddContent(request.DefaultFilename, false, new StringBuilder(builderResult));
        }
    }
}
