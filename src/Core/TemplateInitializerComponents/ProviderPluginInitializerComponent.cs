namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ProviderPluginInitializerComponent : ITemplateInitializerComponent
{
    public void Initialize(ITemplateEngineContext context)
    {
        Guard.IsNotNull(context);

        if (context.Context is not null
            && context.Template is ITemplateProviderPlugin plugin)
        {
            plugin.Initialize(context.Context.Provider);
        }
    }
}
