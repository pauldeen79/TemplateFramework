namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Abstractions;

public interface ITemplateCreator
{
    bool SupportsModel(object? model);
    bool SupportsName(string name);
    object CreateByModel(object? model);
    object CreateByName(string name);
}
