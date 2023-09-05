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
    
    public void Render(ITemplateEngineContext context)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        var environment = context.GenerationEnvironment as StringBuilderEnvironment;
        if (environment is null)
        {
            throw new NotSupportedException($"Type of GenerationEnvironment ({context.GenerationEnvironment.GetType().FullName}) is not supported");
        }

        if (!_renderers.Any(x => x.TryRender(context.Template, environment.Builder)))
        {
            var output = context.Template.ToString();

            if (!string.IsNullOrEmpty(output))
            {
                environment.Builder.Append(output);
            }
        }
    }
}
