namespace TemplateFramework.Core;

public sealed class TemplateEngine : ITemplateEngine
{
    private readonly ITemplateInitializer _templateInitializer;
    private readonly IEnumerable<ITemplateRenderer> _templateRenderers;

    public TemplateEngine(ITemplateInitializer templateInitializer, IEnumerable<ITemplateRenderer> templateRenderers)
    {
        Guard.IsNotNull(templateInitializer);
        Guard.IsNotNull(templateRenderers);
        _templateInitializer = templateInitializer;
        _templateRenderers = templateRenderers;
    }

    public void Render<TModel>(IRenderTemplateRequest<TModel> request)
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
