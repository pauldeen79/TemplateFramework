namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ContextInitializerComponent : ITemplateInitializerComponent
{
    public void Initialize(ITemplateEngineContext context)
    {
        Guard.IsNotNull(context);

        if (context.Template is not ITemplateContextContainer templateContextContainer)
        {
            return;
        }

        var templateContext = context.Context
            ?? new TemplateContext(context.Engine, context.ComponentRegistry, context.DefaultFilename, context.Identifier, context.Template!, context.Model);

        templateContextContainer.Context = templateContext;
    }
}
