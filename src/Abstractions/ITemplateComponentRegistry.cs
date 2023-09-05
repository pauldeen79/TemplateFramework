namespace TemplateFramework.Abstractions;

public interface ITemplateComponentRegistry
{
    void RegisterComponent(ITemplateProviderComponent component);
}
