namespace TemplateFramework.Core.TemplateRenderers;

public sealed class StringBuilderTemplateRenderer : ISingleContentTemplateRenderer
{
    private readonly IEnumerable<IStringBuilderTemplateRenderer> _renderers;

    public StringBuilderTemplateRenderer(IEnumerable<IStringBuilderTemplateRenderer> renderers)
    {
        Guard.IsNotNull(renderers);

        _renderers = renderers;
    }

    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is StringBuilderEnvironment;
    
    public void Render(IRenderTemplateRequest request)
    {
        Guard.IsNotNull(request);

        var environment = request.GenerationEnvironment as StringBuilderEnvironment;
        if (environment is null)
        {
            throw new NotSupportedException($"Type of GenerationEnvironment ({request.GenerationEnvironment?.GetType().FullName}) is not supported");
        }

        if (!_renderers.Any(x => x.TryRender(request.Template, environment.Builder)))
        {
            var output = request.Template.ToString();

            if (!string.IsNullOrEmpty(output))
            {
                environment.Builder.Append(output);
            }
        }
    }
}
