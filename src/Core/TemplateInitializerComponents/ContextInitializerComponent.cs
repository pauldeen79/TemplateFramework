namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ContextInitializerComponent : ITemplateInitializerComponent
{
    public int Order => 1;

    public Task<Result> InitializeAsync(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        if (context.Template is ITemplateEngineContextContainer templateEngineContextContainer)
        {
            templateEngineContextContainer.Context = context;
        }

        if (context.Template is ITemplateContextContainer templateContextContainer)
        {
            var templateContext = context.Context
                ?? new TemplateContext(context.Engine, context.ComponentRegistry, context.DefaultFilename, context.Identifier, context.Template!, context.Model);

            templateContextContainer.Context = templateContext;
        }

        return Task.FromResult(Result.Success());
    }
}
