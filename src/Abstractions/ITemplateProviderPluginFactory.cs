namespace TemplateFramework.Abstractions;

public interface ITemplateProviderPluginFactory
{
    ITemplateProviderPlugin Create(string assemblyName, string className, string currentDirectory);
}
