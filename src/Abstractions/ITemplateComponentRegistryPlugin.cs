namespace TemplateFramework.Abstractions;

public interface ITemplateComponentRegistryPlugin
{
    Task<Result> InitializeAsync(ITemplateComponentRegistry registry, CancellationToken token);
}
