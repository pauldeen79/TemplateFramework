namespace TemplateFramework.Abstractions;

public interface ITemplateProviderPluginIdentifier : ITemplateIdentifier
{
    string? TemplateProviderAssemblyName { get; }
    string? TemplateProviderClassName { get; }
    string CurrentDirectory { get; }
}
