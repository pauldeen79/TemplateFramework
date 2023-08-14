namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ContextInitializer : ITemplateInitializerComponent
{
    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        var context = request.Context ??new TemplateContext(request.Template, request.Model);

        if (request.Template is not ITemplateContextContainer templateContextContainer)
        {
            return;
        }

        templateContextContainer.Context = context;
    }
}
