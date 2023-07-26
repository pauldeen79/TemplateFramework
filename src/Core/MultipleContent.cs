namespace TemplateFramework.Core;

internal sealed class MultipleContent : IMultipleContent
{
    public MultipleContent(IEnumerable<IContent> contents)
    {
        Contents = contents.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<IContent> Contents { get; }
}
