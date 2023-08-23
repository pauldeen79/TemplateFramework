namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ContextInitializer : ITemplateInitializerComponent
{
    private readonly ITemplateProvider _provider;

    public ContextInitializer(ITemplateProvider provider)
    {
        _provider = provider;
    }

    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        var context = request.Context ?? new TemplateContext(engine, _provider, request.DefaultFilename, request.Template, request.Model);

        if (request.Template is not ITemplateContextContainer templateContextContainer)
        {
            return;
        }

        templateContextContainer.Context = context;
    }
}
