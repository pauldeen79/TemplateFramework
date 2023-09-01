namespace TemplateFramework.Abstractions;

public interface ITemplateFactory
{
    object Create(Type type);
}
