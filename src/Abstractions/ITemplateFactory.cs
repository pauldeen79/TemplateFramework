namespace TemplateFramework.Abstractions;

public interface ITemplateFactory
{
    object CreateByModel(object? model);
    object CreateByName(string name);
}
