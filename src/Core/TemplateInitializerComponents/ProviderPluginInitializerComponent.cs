namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ProviderPluginInitializerComponent : ITemplateInitializerComponent
{
    private readonly ITemplateProviderPluginFactory _factory;

    public ProviderPluginInitializerComponent(ITemplateProviderPluginFactory factory)
    {
        Guard.IsNotNull(factory);

        _factory = factory;
    }

    public void Initialize(ITemplateEngineContext context)
    {
        Guard.IsNotNull(context);

        if (context.Context is null)
        {
            return;
        }
        
        if (context.Template is ITemplateProviderPlugin providerPlugin)
        {
            providerPlugin.Initialize(context.Context.Provider);
        }

        if (context.Identifier is ITemplateProviderPluginIdentifier pluginIdentifier
            && pluginIdentifier.TemplateProviderAssemblyName is not null
            && pluginIdentifier.TemplateProviderClassName is not null)
        {
            var identifierPlugin = _factory.Create(pluginIdentifier.TemplateProviderAssemblyName, pluginIdentifier.TemplateProviderClassName, pluginIdentifier.CurrentDirectory);
            
            identifierPlugin.Initialize(context.Context.Provider);
        }
    }
}
