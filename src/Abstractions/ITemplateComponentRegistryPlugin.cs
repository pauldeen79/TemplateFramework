namespace TemplateFramework.Abstractions;

public interface ITemplateComponentRegistryPlugin
{
    Task<Result> Initialize(ITemplateComponentRegistry registry, CancellationToken cancellationToken);
}
