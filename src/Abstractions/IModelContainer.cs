namespace TemplateFramework.Abstractions;

public interface IModelContainer<T>
{
    T Model { get; set; }
}
