namespace TemplateFramework.Core;

public sealed class TemplateEngine : ITemplateEngine
{
    private readonly ITemplateProvider _provider;
    private readonly ITemplateInitializer _initializer;
    private readonly ITemplateParameterExtractor _parameterExtractor;
    private readonly IEnumerable<ITemplateRenderer> _renderers;

    public TemplateEngine(
        ITemplateProvider provider,
        ITemplateInitializer initializer,
        ITemplateParameterExtractor parameterExtractor,
        IEnumerable<ITemplateRenderer> renderers)
    {
        Guard.IsNotNull(provider);
        Guard.IsNotNull(initializer);
        Guard.IsNotNull(parameterExtractor);
        Guard.IsNotNull(renderers);

        _provider = provider;
        _initializer = initializer;
        _parameterExtractor = parameterExtractor;
        _renderers = renderers;
    }

    public async Task<Result<ITemplateParameter[]>> GetParametersAsync(object templateInstance, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(templateInstance);

        return await _parameterExtractor.ExtractAsync(templateInstance, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Result> RenderAsync(IRenderTemplateRequest request, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(request);

        var template = request.Context?.Template;
        if (template is null || template is IIgnoreThis)
        {
            template = _provider.Create(request.Identifier);
            if (template is null)
            {
                return Result.Error("TemplateProvider did not create a template instance");
            }
        }

        var engineContext = new TemplateEngineContext(request, this, _provider, template);

        var result = await _initializer.InitializeAsync(engineContext, cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccessful())
        {
            return result;
        }

        var renderer = _renderers.FirstOrDefault(x => x.Supports(request.GenerationEnvironment));
        if (renderer is null)
        {
            return Result.NotSupported($"Type of GenerationEnvironment ({request.GenerationEnvironment.GetType().FullName}) is not supported");
        }

        return await renderer.RenderAsync(engineContext, cancellationToken).ConfigureAwait(false);
    }
}
