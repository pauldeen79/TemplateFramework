namespace TemplateFramework.Core.TemplateRenderers;

public sealed class StringBuilderTemplateRenderer : ITemplateRenderer
{
    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is StringBuilderEnvironment;
    
    public void Render(IRenderTemplateRequest request)
    {
        Guard.IsNotNull(request);

        var environment = request.GenerationEnvironment as StringBuilderEnvironment;
        if (environment is null)
        {
            throw new NotSupportedException($"Type of GenerationEnvironment ({request.GenerationEnvironment?.GetType().FullName}) is not supported");
        }

        var builder = environment.Builder;

        //TODO: Check if we need a wrapper here, when loading external assemblies...
        if (request.Template is IStringBuilderTemplate typedTemplate)
        {
            typedTemplate.Render(builder);
        }
        else if (request.Template is ITextTransformTemplate textTransformTemplate)
        {
            var output = textTransformTemplate.TransformText();
            ApendIfFilled(builder, output);
        }
        else
        {
            var output = request.Template.ToString();
            ApendIfFilled(builder, output);
        }
    }

    private static void ApendIfFilled(StringBuilder builder, string? output)
    {
        if (string.IsNullOrEmpty(output))
        {
            return;
        }
        
        builder.Append(output);
    }
}
