namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ContextInitializerComponent : ITemplateInitializerComponent
{
    public Task Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        if (context.Template is not ITemplateContextContainer templateContextContainer)
        {
            return Task.CompletedTask;
        }

        var templateContext = context.Context
            ?? new TemplateContext(context.Engine, context.ComponentRegistry, context.DefaultFilename, context.Identifier, context.Template!, context.Model);

        templateContextContainer.Context = templateContext;

        return Task.CompletedTask;
    }
}
