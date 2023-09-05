namespace TemplateFramework.Abstractions;

public interface ITemplateProvider : ITemplateComponentRegistry
{
    object Create(ITemplateIdentifier identifier);
    void StartSession();
}
