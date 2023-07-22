namespace TemplateFramework.Abstractions;

public interface ITemplateProvider
{
    bool Supports(ICreateTemplateRequest request);
    object Create(ICreateTemplateRequest request);
}
