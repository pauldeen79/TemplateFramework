namespace TemplateFramework.Abstractions;

public interface ITemplateProviderComponent : ITemplateProvider
{
    bool Supports(ITemplateIdentifier identifier);
}
