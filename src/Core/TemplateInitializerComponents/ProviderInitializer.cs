namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ProviderInitializer : ITemplateInitializerComponent
{
    private readonly ITemplateProvider _provider;

    public ProviderInitializer(ITemplateProvider provider)
    {
        Guard.IsNotNull(provider);

        _provider = provider;
    }
    
    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        if (request.Template is not ITemplateProviderContainer templateProviderContainer)
        {
            return;
        }

        templateProviderContainer.Provider = _provider;
    }
}
