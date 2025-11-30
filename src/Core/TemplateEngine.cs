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

    public async Task<Result<ITemplateParameter[]>> GetParametersAsync(object templateInstance, CancellationToken token)
    {
        Guard.IsNotNull(templateInstance);

        return await _parameterExtractor.ExtractAsync(templateInstance, token).ConfigureAwait(false);
    }

    public async Task<Result> RenderAsync(IRenderTemplateRequest request, CancellationToken token)
    {
        Guard.IsNotNull(request);

        var template = request.Context?.Template;
        if (template is null || template is IIgnoreThis)
        {
            var templateResult = _provider.Create(request.Identifier)
                .EnsureNotNull("TemplateProvider did not create a template instance")
                .EnsureValue("TemplateProvider did not create a template instance");
            if (!templateResult.IsSuccessful())
            {
                return templateResult;
            }
            template = templateResult.Value!;
        }

        var engineContext = new TemplateEngineContext(request, this, _provider, template);

        var result = await _initializer.InitializeAsync(engineContext, token).ConfigureAwait(false);
        if (!result.IsSuccessful())
        {
            return result;
        }

        var renderer = _renderers.FirstOrDefault(x => x.Supports(request.GenerationEnvironment));
        if (renderer is null)
        {
            return Result.NotSupported($"Type of GenerationEnvironment ({request.GenerationEnvironment.GetType().FullName}) is not supported");
        }

        return await renderer.RenderAsync(engineContext, token).ConfigureAwait(false);
    }
}
