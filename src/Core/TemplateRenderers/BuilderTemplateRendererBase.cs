namespace TemplateFramework.Core.TemplateRenderers;

public abstract class BuilderTemplateRendererBase<TEnvironment, TBuilder> : ISingleContentTemplateRenderer
    where TEnvironment : GenerationEnvironmentBase<TBuilder>
{
    private readonly IEnumerable<IBuilderTemplateRenderer<TBuilder>> _renderers;

    protected BuilderTemplateRendererBase(IEnumerable<IBuilderTemplateRenderer<TBuilder>> renderers)
    {
        Guard.IsNotNull(renderers);

        _renderers = renderers;
    }

    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is TEnvironment;

    public async Task Render(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        if (context.GenerationEnvironment is not TEnvironment environment)
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
            await DefaultImplementation(context.Template, environment.Builder).ConfigureAwait(false);
        }
    }

    protected abstract Task DefaultImplementation(object templateInstance, TBuilder builder);
}
