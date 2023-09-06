namespace TemplateFramework.Abstractions;

public interface ITemplateComponentRegistryPlugin
{
    void Initialize(ITemplateComponentRegistry registry);
}
