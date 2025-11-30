namespace TemplateFramework.Abstractions;

public interface ITemplateProviderComponent
{
    Result<object> Create(ITemplateIdentifier identifier);
}
