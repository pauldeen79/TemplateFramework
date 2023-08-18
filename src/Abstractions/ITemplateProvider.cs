namespace TemplateFramework.Abstractions;

public interface ITemplateProvider
{
    object Create(ICreateTemplateRequest request);
}
