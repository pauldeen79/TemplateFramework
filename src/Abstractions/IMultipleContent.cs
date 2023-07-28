namespace TemplateFramework.Abstractions;

public interface IMultipleContent
{
    IReadOnlyCollection<IContent> Contents { get; }
}
