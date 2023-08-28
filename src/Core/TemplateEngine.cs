namespace TemplateFramework.Core;

public sealed class TemplateEngine : ITemplateEngine
{
    private readonly ITemplateInitializer _templateInitializer;
    private readonly ITemplateParameterExtractor _templateParameterExtractor;
    private readonly IEnumerable<ITemplateRenderer> _templateRenderers;

    public TemplateEngine(
        ITemplateInitializer templateInitializer,
        ITemplateParameterExtractor templateParameterExtractor,
        IEnumerable<ITemplateRenderer> templateRenderers)
    {
        Guard.IsNotNull(templateInitializer);
        Guard.IsNotNull(templateParameterExtractor);
        Guard.IsNotNull(templateRenderers);

        _templateInitializer = templateInitializer;
        _templateParameterExtractor = templateParameterExtractor;
        _templateRenderers = templateRenderers;
    }

    public ITemplateParameter[] GetParameters(object templateInstance)
    {
        Guard.IsNotNull(templateInstance);

        return _templateParameterExtractor.Extract(templateInstance);
    }

    public void Render(IRenderTemplateRequest request)
    {
        Guard.IsNotNull(request);

        _templateInitializer.Initialize(request, this);

        var renderer = _templateRenderers.FirstOrDefault(x => x.Supports(request.GenerationEnvironment));
        if (renderer is null)
        {
            throw new NotSupportedException($"Type of GenerationEnvironment ({request.GenerationEnvironment?.GetType().FullName}) is not supported");
        }

        renderer.Render(request);
    }
}
