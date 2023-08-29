namespace TemplateFramework.Abstractions;

public interface ITemplateProvider
{
    object Create(ITemplateIdentifier request);
}
