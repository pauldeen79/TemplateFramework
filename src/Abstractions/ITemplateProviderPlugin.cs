namespace TemplateFramework.Abstractions;

public interface ITemplateProviderPlugin
{
    void Initialize(ITemplateProvider provider);
}
