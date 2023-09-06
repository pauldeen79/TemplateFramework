namespace TemplateFramework.Abstractions;

public interface ITemplateComponentRegistryIdentifier : ITemplateIdentifier
{
    string? PluginAssemblyName { get; }
    string? PluginClassName { get; }
    string CurrentDirectory { get; }
}
