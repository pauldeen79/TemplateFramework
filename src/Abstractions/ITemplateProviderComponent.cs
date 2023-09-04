namespace TemplateFramework.Abstractions;

public interface ITemplateProviderComponent
{
    bool Supports(ITemplateIdentifier identifier);
    object Create(ITemplateIdentifier identifier);
}
