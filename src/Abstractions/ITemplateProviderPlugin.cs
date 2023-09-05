namespace TemplateFramework.Abstractions;

public interface ITemplateProviderPlugin
{
    void Initialize(ITemplateComponentRegistry registry);
}
