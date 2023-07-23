namespace TemplateFramework.Abstractions;

public interface IMultipleContent
{
    string BasePath { get; }
    Encoding Encoding { get; }
    IReadOnlyCollection<IContent> Contents { get; }
}
