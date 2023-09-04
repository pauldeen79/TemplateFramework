namespace TemplateFramework.Core.TemplateIdentifiers;

public class TemplateInstanceIdentifierWithTemplateProvider : TemplateInstanceIdentifier, ITemplateProviderPluginIdentifier
{
    public TemplateInstanceIdentifierWithTemplateProvider(object instance, string? currentDirectory, string? templateProviderAssemblyName, string? templateProviderClassName) : base(instance)
    {
        CurrentDirectory = currentDirectory ?? Directory.GetCurrentDirectory();
        TemplateProviderAssemblyName = templateProviderAssemblyName;
        TemplateProviderClassName = templateProviderClassName;
    }

    public string? TemplateProviderAssemblyName { get; }
    public string? TemplateProviderClassName { get; }
    public string CurrentDirectory { get; }
}
