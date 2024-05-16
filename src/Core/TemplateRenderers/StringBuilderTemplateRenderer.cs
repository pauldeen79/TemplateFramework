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
    
    public async Task Render(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        if (context.GenerationEnvironment is not StringBuilderEnvironment environment)
        {
            throw new NotSupportedException($"Type of GenerationEnvironment ({context.GenerationEnvironment.GetType().FullName}) is not supported");
        }

        var result = false;
        foreach (var renderer in _renderers)
        {
            result = await renderer.TryRender(context.Template, environment.Builder, cancellationToken).ConfigureAwait(false);
            if (result || cancellationToken.IsCancellationRequested)
            {
                break;
            }
        }

        if (!result && !cancellationToken.IsCancellationRequested)
        {
            var output = context.Template.ToString();

            if (!string.IsNullOrEmpty(output))
            {
                environment.Builder.Append(output);
            }
        }
    }
}
