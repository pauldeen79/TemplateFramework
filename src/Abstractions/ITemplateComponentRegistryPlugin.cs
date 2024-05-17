namespace TemplateFramework.Abstractions;

public interface ITemplateComponentRegistryPlugin
{
    Task Initialize(ITemplateComponentRegistry registry, CancellationToken cancellationToken);
}
