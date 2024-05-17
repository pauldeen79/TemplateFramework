namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ProviderPluginInitializerComponent : ITemplateInitializerComponent
{
    private readonly ITemplateComponentRegistryPluginFactory _factory;

    public ProviderPluginInitializerComponent(ITemplateComponentRegistryPluginFactory factory)
    {
        Guard.IsNotNull(factory);

        _factory = factory;
    }

    public Task Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        if (context.Context is null)
        {
            return Task.CompletedTask;
        }
        
        if (context.Template is ITemplateComponentRegistryPlugin registryPlugin)
        {
            registryPlugin.Initialize(context.Context.TemplateComponentRegistry);
        }

        if (context.Identifier is ITemplateComponentRegistryIdentifier pluginIdentifier
            && pluginIdentifier.PluginAssemblyName is not null
            && pluginIdentifier.PluginClassName is not null)
        {
            var identifierPlugin = _factory.Create(pluginIdentifier.PluginAssemblyName, pluginIdentifier.PluginClassName, pluginIdentifier.CurrentDirectory);
            
            identifierPlugin.Initialize(context.Context.TemplateComponentRegistry);
        }

        return Task.CompletedTask;
    }
}
