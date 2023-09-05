namespace TemplateFramework.Abstractions;

public interface ITemplateProvider
{
    object Create(ITemplateIdentifier identifier);
    void RegisterComponent(ITemplateProviderComponent component);
    void StartSession();
}
