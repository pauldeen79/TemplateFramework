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

    public async Task<Result> RenderAsync(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        if (context.GenerationEnvironment is not TEnvironment environment)
        {
            return Result.NotSupported($"Type of GenerationEnvironment ({context.GenerationEnvironment.GetType().FullName}) is not supported");
        }

        foreach (var renderer in _renderers)
        {
            var result = await renderer.TryRenderAsync(context.Template, environment.Builder, cancellationToken).ConfigureAwait(false);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return await DefaultImplementation(context.Template, environment.Builder).ConfigureAwait(false);
    }

    protected abstract Task<Result> DefaultImplementation(object templateInstance, TBuilder builder);
}
