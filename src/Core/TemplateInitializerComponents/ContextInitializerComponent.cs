namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ContextInitializerComponent : ITemplateInitializerComponent
{
    private readonly ITemplateProvider _provider;

    public ContextInitializerComponent(ITemplateProvider provider)
    {
        _provider = provider;
    }

    public void Initialize(ITemplateEngineContext context)
    {
        Guard.IsNotNull(context);

        if (context.Template is not ITemplateContextContainer templateContextContainer)
        {
            return;
        }

        var templateContext = context.Context
            ?? new TemplateContext(context.Engine, _provider, context.DefaultFilename, context.Identifier, context.Template!, context.Model);

        templateContextContainer.Context = templateContext;
    }
}
