namespace TemplateFramework.Abstractions;

public interface ITemplateComponentRegistryPluginFactory
{
    ITemplateComponentRegistryPlugin Create(string assemblyName, string className, string currentDirectory);
}
