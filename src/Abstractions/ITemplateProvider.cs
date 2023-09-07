namespace TemplateFramework.Abstractions;

public interface ITemplateProvider : ITemplateComponentRegistry, ISessionAwareComponent
{
    object Create(ITemplateIdentifier identifier);
}
