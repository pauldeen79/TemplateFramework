namespace TemplateFramework.Core.TemplateIdentifiers;

public class TemplateInstanceIdentifierWithTemplateProvider : TemplateInstanceIdentifier, ITemplateComponentRegistryIdentifier
{
    public TemplateInstanceIdentifierWithTemplateProvider(object instance, string? currentDirectory, string? templateProviderAssemblyName, string? templateProviderClassName) : base(instance)
    {
        CurrentDirectory = currentDirectory ?? Directory.GetCurrentDirectory();
        PluginAssemblyName = templateProviderAssemblyName;
        PluginClassName = templateProviderClassName;
    }

    public string? PluginAssemblyName { get; }
    public string? PluginClassName { get; }
    public string CurrentDirectory { get; }
}
