namespace TemplateFramework.Abstractions;

public interface ITemplateProvider : ITemplateComponentRegistry, ISessionAwareComponent
{
    Result<object> Create(ITemplateIdentifier identifier);
}
