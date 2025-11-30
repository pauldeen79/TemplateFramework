namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Abstractions;

public interface ITemplateCreator
{
    Result<object> CreateByModel(object? model);
    Result<object> CreateByName(string name);
}
