namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ProviderPluginInitializerComponent : ITemplateInitializerComponent
{
    private readonly ITemplateComponentRegistryPluginFactory _factory;

    public ProviderPluginInitializerComponent(ITemplateComponentRegistryPluginFactory factory)
    {
        Guard.IsNotNull(factory);

        _factory = factory;
    }

    public async Task<Result> Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        if (context.Context is null)
        {
            return Result.Continue();
        }

        if (context.Template is ITemplateComponentRegistryPlugin registryPlugin)
        {
            return await registryPlugin.Initialize(context.Context.TemplateComponentRegistry, cancellationToken).ConfigureAwait(false);
        }

        if (context.Identifier is ITemplateComponentRegistryIdentifier pluginIdentifier
            && pluginIdentifier.PluginAssemblyName is not null
            && pluginIdentifier.PluginClassName is not null)
        {
            var identifierPlugin = _factory.Create(pluginIdentifier.PluginAssemblyName, pluginIdentifier.PluginClassName, pluginIdentifier.CurrentDirectory);

            return await identifierPlugin.Initialize(context.Context.TemplateComponentRegistry, cancellationToken).ConfigureAwait(false);
        }

        return Result.Continue();
    }
}
