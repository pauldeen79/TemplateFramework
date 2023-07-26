namespace TemplateFramework.Abstractions;

public interface IMultipleContent
{
    string BasePath { get; }
    IReadOnlyCollection<IContent> Contents { get; }
}
