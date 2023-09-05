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

    public ITemplateParameter[] GetParameters(object templateInstance)
    {
        Guard.IsNotNull(templateInstance);

        return _parameterExtractor.Extract(templateInstance);
    }

    public void Render(IRenderTemplateRequest request)
    {
        Guard.IsNotNull(request);

        var template = request.Context?.Template;
        if (template is null || template is IgnoreThis)
        {
            template = _provider.Create(request.Identifier);
        }

        var engineContext = new TemplateEngineContext(request, this, _provider, template);
        
        _initializer.Initialize(engineContext);

        var renderer = _renderers.FirstOrDefault(x => x.Supports(request.GenerationEnvironment));
        if (renderer is null)
        {
            throw new NotSupportedException($"Type of GenerationEnvironment ({request.GenerationEnvironment.GetType().FullName}) is not supported");
        }

        renderer.Render(engineContext);
    }
}
