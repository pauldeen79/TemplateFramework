namespace TemplateFramework.Core.TemplateInitializerComponents;

public class EngineInitializer : ITemplateInitializerComponent
{
    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        if (request.Template is not ITemplateEngineContainer templateEngineContainer)
        {
            return;
        }

        templateEngineContainer.Engine = engine;
    }
}
